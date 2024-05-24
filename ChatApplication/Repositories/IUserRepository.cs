using ChatApplication.Hubs;
using ChatApplication.Models;
using ChatApplication.Viewmodels;

namespace ChatApplication.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers(string email);
        Task<User> GetClientName(int userId);

        Task<string> AddAvailableUsers(Client client);
        Task<string> GetAvailableUserConnectionId(string clientName);

        Task<int> GetUserId(string username);

        //Task<List<User>> GetUsernameById(int senderUserId, int receiverUserId);
        //Task<string> GetUsernameById(int id);

    }
}
