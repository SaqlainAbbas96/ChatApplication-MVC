using ChatApplication.Models;
using ChatApplication.Repositories;
using ChatApplication.Services;
using Microsoft.AspNetCore.SignalR;
using static ChatApplication.Hubs.ChatHub;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatApplication.Hubs
{

    public class Client
    {
        public string ConnectionId { get; set; }
        public string Username { get; set; }
    }
    public class ClientSource
    {
        public static List<Client> Clients { get; } = new List<Client>(); // Property ReadOnly olarak tanımlandı.
    }


    public class ChatHub : Hub
    {
        private readonly IUserConnectionService _userConnectionService;
        private readonly DBContext _context;
        private readonly IUserRepository _userRepository;

        public ChatHub(IUserConnectionService userConnectionService, DBContext context, IUserRepository userRepository)
        {
            _userConnectionService = userConnectionService;
            _context = context;
            _userRepository = userRepository;
        }
       
        //Need to call this when user enters the chatroom
        public async Task<string> GetUsernameAsync(string username)
        {
            Client client = new Client { ConnectionId = Context.ConnectionId, Username = username };

            // Callerı (Sisteme dahil olan kullanıcıyı) mevcuttaki tüm clientların tutulduğu listeye ekler.
            ClientSource.Clients.Add(client);

            // Sisteme bir clientın dahil olduğunu caller (dahil olan client) dışındaki tüm clientlara bildiriyor..
            await Clients.Others.SendAsync("clientJoined", username);

            // Yeni kullanıcının da eklendiği güncel listeyi tüm clientlara bildirir..
            await GetClientsAsync();

            // Sisteme eklenmiş oda/grup listesi sisteme giriş yapan kullanıcıya (caller) bildirilir..
            //await Clients.Caller.SendAsync("groups", GroupSource.Groups);

            //
            _userRepository.AddAvailableUsers(client);

            // Return the connection ID of the newly added client
            return Context.ConnectionId;

        }
        public async Task GetClientsAsync()
        {
            // Sisteme dahil olan clientı kendisi de dahil olmak üzere tüm clientlara bildirir..
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }


        //public override async Task OnConnectedAsync()
        //{
        //    var userName = Context.GetHttpContext().Request.Query["user"].ToString();
        //    if (!string.IsNullOrEmpty(userName))
        //    {
        //        _connections.Add(userName, Context.ConnectionId);
        //    }
        //    await base.OnConnectedAsync();
        //}

        public override Task OnConnectedAsync()
        {
            // Set ConnectionId into HttpContext.Items
            Context.Items["ConnectionId"] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }


        //private string IdentityName
        //{
        //    get { return Context.User.Identity.Name; }
        //}
        //public override Task OnConnectedAsync()
        //{
        //    try
        //    {
        //        var user = _context.User.Where(u => u.username == IdentityName).FirstOrDefault();
        //        var userViewModel = _mapper.Map<ApplicationUser, UserViewModel>(user);
        //        userViewModel.Device = GetDevice();
        //        userViewModel.CurrentRoom = "";

        //        if (!_Connections.Any(u => u.UserName == IdentityName))
        //        {
        //            _Connections.Add(userViewModel);
        //            _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
        //        }

        //        Clients.Caller.SendAsync("getProfileInfo", userViewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
        //    }
        //    return base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception exception)
        //{
        //    // Get the user ID of the disconnected user (assuming you have a way to identify users)
        //    var userId = Context.User.Identity.Name; // Example: Get user ID from claims

        //    // Remove user connection
        //    await _userConnectionService.RemoveUserConnection(userId, Context.ConnectionId);

        //    await base.OnDisconnectedAsync(exception);
        //}

        //public override Task OnConnectedAsync()
        //{
        //    // Set ConnectionId into HttpContext.Items
        //    Context.Items["ConnectionId"] = Context.ConnectionId;
        //    return base.OnConnectedAsync();
        //}

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendToUserOld(string user, string receiverConnectionId, string message)
        {
            await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
        }

        //public async Task SendToUser(string user, string receiverUserId, string message)
        //{
        //    string receiverConnectionId = _userConnectionService.GetConnectionId(receiverUserId);
        //    if (receiverConnectionId != null)
        //    {
        //        await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
        //    }
        //}


        //Shows connectionId on View
        //Generates connectionId when page loads


        public async Task<string> GetConnectionId(string username)
        //public string GetConnectionId(string username) 
        {
            Client client = new Client { ConnectionId = Context.ConnectionId, Username = username };

            // Callerı (Sisteme dahil olan kullanıcıyı) mevcuttaki tüm clientların tutulduğu listeye ekler.
            ClientSource.Clients.Add(client);

            // Sisteme bir clientın dahil olduğunu caller (dahil olan client) dışındaki tüm clientlara bildiriyor..
            await Clients.Others.SendAsync("clientJoined", username);

            // Yeni kullanıcının da eklendiği güncel listeyi tüm clientlara bildirir..
            await GetClientsAsync();


            // Sisteme eklenmiş oda/grup listesi sisteme giriş yapan kullanıcıya (caller) bildirilir..
            //await Clients.Caller.SendAsync("groups", GroupSource.Groups);


            _userRepository.AddAvailableUsers(client);

            // Return the connection ID of the newly added client
            return Context.ConnectionId;
                        
        }

        //public string GetConnectionId() => Context.ConnectionId;
   
    }
}
