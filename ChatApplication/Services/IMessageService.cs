using ChatApplication.Hubs;
using ChatApplication.Viewmodels;

namespace ChatApplication.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageViewModelDTO>> GetMessages(string username, string clientName);
        Task<IEnumerable<MessageViewModelDTO>> GetMessages(string username);
    }
}
