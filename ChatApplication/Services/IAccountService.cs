using ChatApplication.DTOs;

namespace ChatApplication.Services
{
    public interface IAccountService
    {
        Task<string> Register(UserDTO dto);
        Task<string> Authenticate(LoginDTO dto);
        Task<string> LogoutUser();
    }
}
