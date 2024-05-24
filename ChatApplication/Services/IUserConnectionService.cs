using System.Collections.Concurrent;

namespace ChatApplication.Services
{
    public interface IUserConnectionService
    {
        Task AddUserConnection(string userId, string connectionId);
        Task RemoveUserConnection(string userId, string connectionId);
        Task<string> GetConnectionIdForUser(string userId);
    }

    public class UserConnectionService : IUserConnectionService
    {
        private readonly ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();
        //private readonly Dictionary<string, HashSet<string>> userConnections = new Dictionary<string, HashSet<string>>();

        public async Task AddUserConnection(string userId, string connectionId)
        {
            //if (!userConnections.ContainsKey(userId))
            //{
            //    userConnections[userId] = new HashSet<string>();
            //}

            //userConnections[userId].Add(connectionId);
            _userConnections.AddOrUpdate(userId, connectionId, (key, existingConnectionId) => connectionId);
        }

        public async Task RemoveUserConnection(string userId, string connectionId)
        {
            //if (userConnections.ContainsKey(userId))
            //{
            //    userConnections[userId].Remove(connectionId);

            //    if (userConnections[userId].Count == 0)
            //    {
            //        userConnections.Remove(userId);
            //    }
            //}
            _userConnections.TryRemove(userId, out _);
        }

        public async Task<string> GetConnectionIdForUser(string userId)
        {
            //if (userConnections.ContainsKey(userId))
            //{
            //    // For simplicity, assume the user has only one active connection
            //    return userConnections[userId].FirstOrDefault();
            //}

            //return null;
            _userConnections.TryGetValue(userId, out string connectionId);
            return connectionId;
        }
    }

}
