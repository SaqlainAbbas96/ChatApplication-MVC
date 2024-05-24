using ChatApplication.Repositories;
using ChatApplication.Viewmodels;

namespace ChatApplication.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<MessageViewModelDTO>> GetMessages(string username, string clientName) 
        {
            int userId = await _messageRepository.GetSenderIdByName(username);
            int receiverId = await _messageRepository.GetReceiverIdByName(clientName);

            var res = await _messageRepository.GetMessages(userId, receiverId);
 
            List<MessageViewModelDTO> messages = new List<MessageViewModelDTO>();

            foreach (var item in res) 
            {
                MessageViewModelDTO msgs = new MessageViewModelDTO();
                msgs.senderName = item.sendername;
                msgs.receiverName = item.receivername;
                msgs.content = item.content;

                messages.Add(msgs);
            }

            return messages;
        }

        public async Task<IEnumerable<MessageViewModelDTO>> GetMessages(string username)
        {
            int userId = await _messageRepository.GetSenderIdByName(username);

            var res = await _messageRepository.GetMessages(userId);

            List<MessageViewModelDTO> messages = new List<MessageViewModelDTO>();

            foreach (var item in res)
            {
                MessageViewModelDTO msgs = new MessageViewModelDTO();
                msgs.senderName = item.sendername;
                msgs.receiverName = item.receivername;
                msgs.content = item.content;

                messages.Add(msgs);
            }

            return messages;
        }
    }
}
