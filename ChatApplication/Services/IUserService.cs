using ChatApplication.Models;
using ChatApplication.Viewmodels;

namespace ChatApplication.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModelDTO>> GetAllUsers();
        Task<UserViewModelDTO> GetClientName(int userId);
        Task<int> GetUserId(string username);
    }
}
