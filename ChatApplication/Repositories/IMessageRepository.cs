using ChatApplication.Models;
using ChatApplication.Viewmodels;

namespace ChatApplication.Repositories
{
    public interface IMessageRepository
    {
        Task<int> GetSenderIdByName(string sender);
        Task<int> GetReceiverIdByName(string receiver);
        Task<string> SaveMessage(Message message);

        Task<IEnumerable<Message>> GetMessages(int userId, int clientId);
        Task<IEnumerable<Message>> GetMessages(int userId);
    }
}
