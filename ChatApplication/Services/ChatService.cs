
using ChatApplication.Hubs;
using ChatApplication.Models;
using ChatApplication.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Services
{
    public class ChatService : IChatService
    {
        private readonly IUserConnectionService _userMappingService;
        public readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserRepository _userRepository;       //Check if need to create chatRepo instead
        private readonly IMessageRepository _messageRepository;

        public ChatService(
            IHubContext<ChatHub> hubContext, 
            IUserConnectionService userMappingService, 
            IUserRepository userRepository,
            IMessageRepository messageRepository)
        {
            _hubContext = hubContext;
            _userMappingService = userMappingService;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        //public async Task<string> SendMessageToUser(string user, string receiverConnectionId, string message)
        //{
        //    await _hubContext.Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", user, message);
        //    return "Sent Message";
        //}


        //Now need to pass username instead of hardcode receiverConnectionId, get username of chatroom
        //example: Hello! Welcome to the Chat Room of Client B (get client b username)
        //pass as parameter and read key of client b from db before sending message
        public async Task<string> SendMessageToUser(string senderUser, string clientName, string message)
        {
            try
            {
                //senderUserId = "6";
                //receiverUserId = "7";
                // Retrieve receiver's connection ID using user mapping service
                //string receiverConnectionId = await _userMappingService.GetConnectionIdForUser(receiverUserId);

                //if (receiverConnectionId != null)
                //{

                //Fetch receiverConnectionId from db where username == clientName
                string receiverConnectionId  = await _userRepository.GetAvailableUserConnectionId(clientName);

                // Send message to the receiver's connection
                await _hubContext
                    .Clients
                    .Client(receiverConnectionId)
                    .SendAsync("ReceiveMessage", senderUser, message);

                int senderUserId = await _messageRepository.GetSenderIdByName(senderUser);
                int receiverUserId = await _messageRepository.GetReceiverIdByName(clientName);

                //string senderName = await _userRepository.GetUsernameById(senderUserId);
                //string receiverName = await _userRepository.GetUsernameById(receiverUserId);

                Message msg = new Message();
                msg.senderid = senderUserId;
                msg.sendername = senderUser;
                msg.receivername = clientName;
                msg.receiverid = receiverUserId;
                msg.datetime = DateTime.Now;
                msg.content = message;

                await _messageRepository.SaveMessage(msg);
                                
                return "Message sent successfully";
            }
            catch (Exception ex)
            {
                return $"Failed to send message: {ex.Message}";
            }
        }

        public async Task<string> SendMessageToGroup(string senderUser, string message)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", senderUser, message);
                int senderUserId = await _messageRepository.GetSenderIdByName(senderUser);

                Message msg = new Message();
                msg.senderid = senderUserId;
                msg.sendername = senderUser;
                msg.receivername = "group";
                msg.receiverid = 0;
                msg.datetime = DateTime.Now;
                msg.content = message;

                await _messageRepository.SaveMessage(msg);

                return "Message sent successfully";
            }
            catch (Exception ex)
            {
                return $"Failed to send message: {ex.Message}";
            }
        }

    }
}
