
using ChatApplication.Models;
using ChatApplication.Viewmodels;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ChatApplication.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DBContext _db;
        public MessageRepository(DBContext db)
        {
            _db = db;
        }

        public async Task<int> GetSenderIdByName(string sender)
        {
            int senderId = _db.User
                .Where(x => x.username == sender)
                .Select(x => x.id)
                .FirstOrDefault();

            return senderId;
        }

        public async Task<int> GetReceiverIdByName(string receiver)
        {
            int receiverId = _db.User
                           .Where(x => x.username == receiver)
                           .Select(x => x.id)
                           .FirstOrDefault();

            return receiverId;
        }

        public async Task<string> SaveMessage(Message message)
        {
            _db.Message.Add(message);
            _db.SaveChanges();

            return "Saved";
        }

        public async Task<IEnumerable<Message>> GetMessages(int userId, int clientId)
        {
            var messages = _db.Message
                .Where(x => x.senderid == userId && 
                        x.receiverid == clientId || 
                        x.senderid == clientId && 
                        x.receiverid == userId)
                .ToList()
                .OrderBy(x=>x.id);

            return messages;
        }

        public async Task<IEnumerable<Message>> GetMessages(int userId)
        {
            var messages = _db.Message
                .Where(x => x.receiverid == 0)
                .ToList()
                .OrderBy(x => x.id);

            return messages;
        }
    }
}
