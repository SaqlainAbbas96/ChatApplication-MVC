using ChatApplication.DTOs;
using ChatApplication.Models;
using ChatApplication.Repositories;
using ChatApplication.Viewmodels;

namespace ChatApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<UserViewModelDTO>> GetAllUsers()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var email = session.GetString("Email");

            var res = await _userRepository.GetAllUsers(email);

            List<UserViewModelDTO> users = new List<UserViewModelDTO>();

            foreach (var user in res) 
            {
                UserViewModelDTO userDto = new UserViewModelDTO();
                userDto.username = user.username;
                userDto.userId = user.id;

                users.Add(userDto);
            }

            return users;
        }

        public async Task<UserViewModelDTO> GetClientName(int userId) 
        {
            var client = await _userRepository.GetClientName(userId);

            UserViewModelDTO userDto = new UserViewModelDTO();
            userDto.username = client.username;
            userDto.userId = client.id;

            return userDto;
        }

        public async Task<int> GetUserId(string username)
        {
            var userId = await _userRepository.GetUserId(username);
            return userId;
        }
    }
}
