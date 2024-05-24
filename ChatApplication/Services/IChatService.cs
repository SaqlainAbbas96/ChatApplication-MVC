namespace ChatApplication.Services
{
    public interface IChatService
    {
        //Task<string> SendMessageToUser(string user, string receiverConnectionId, string message);

        Task<string> SendMessageToUser(string senderUserId, string clientName, string message);
        Task<string> SendMessageToGroup(string senderUser, string message);

    }
}
