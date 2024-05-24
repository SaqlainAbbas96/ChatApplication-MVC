using ChatApplication.Hubs;
using ChatApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApplication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _db;
        public UserRepository(DBContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> GetAllUsers(string email)
        {
            var res = _db.User
                .Where(u => u.email != email)
                //.Select(x => x.username)
                .ToList();
            return res;
        }

        public async Task<User> GetClientName(int userId)
        {
            var client = _db.User
                .Where(x => x.id == userId)
                //.Select(x => x.username)
                .SingleOrDefault();

            return client;
        }



        public async Task<string> AddAvailableUsers(Client client)
        {
            // Check if a client with the same username already exists
            var existingClient = _db.AvailableUsers.Where(u => u.username == client.Username).FirstOrDefault();
            if (existingClient != null)
            {
                // Update the existing client's connection ID
                existingClient.connectionid = client.ConnectionId;
                _db.Update(existingClient); // Mark the entity as modified
            }
            else
            {
                // Add a new client
                AvailableUsers newUser = new AvailableUsers
                {
                    username = client.Username,
                    connectionid = client.ConnectionId
                };
                _db.AvailableUsers.Add(newUser);
            }

            //AvailableUsers a = new AvailableUsers();
            //a.username = client.Username;
            //a.connectionid = client.ConnectionId;
            //_db.AvailableUsers.Add(a);
            //_db.SaveChanges();

            // Save changes to the database
            await _db.SaveChangesAsync();

            return "saved";

        }

        public async Task<string> GetAvailableUserConnectionId(string clientName)
        {
            return
                _db.AvailableUsers
                .Where(x => x.username == clientName)
                .Select(x => x.connectionid)
                .FirstOrDefault();
        }

        public async Task<int> GetUserId(string username)
        {
            int userId = _db.User
                .Where(x => x.username == username)
                .Select(x => x.id)
                .FirstOrDefault();

            return userId;
        }

        //public async Task<List<User>> GetUsernameById(int senderUserId, int receiverUserId)
        //{
        //    var res = _db.User
        //        .Where(x => x.id == senderUserId || x.id == receiverUserId)
        //        .ToList();
        //    return res;
        //}

        //public async Task<string> GetUsernameById(int id)
        //{
        //    var res = _db.User
        //        .Where(x => x.id == id)
        //        .Select(x => x.username)
        //        .FirstOrDefault();
        //    return res;
        //}
    }
}
