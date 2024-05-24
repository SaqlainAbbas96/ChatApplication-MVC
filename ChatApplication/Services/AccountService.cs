using ChatApplication.DTOs;
using ChatApplication.Models;
using ChatApplication.Repositories;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChatApplication.Services
{
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtAuthenticationService _jwtAuthenticationService;
        private readonly IUserConnectionService _userConnectionService;

        public AccountService(
            IHttpContextAccessor httpContextAccessor, 
            IAccountRepository accountRepository, 
            IJwtAuthenticationService jwtAuthenticationService,
            IUserConnectionService userConnectionService)
        {
            _httpContextAccessor = httpContextAccessor;
            _accountRepository = accountRepository;
            _jwtAuthenticationService = jwtAuthenticationService;
            _userConnectionService = userConnectionService;
        }

        public async Task<string> Register(UserDTO dto)
        {
            User user = new User();

            PasswordHash(dto.password, out byte[] passwordHash, out byte[] passwordSalt);

            user.email = dto.email;
            user.username = dto.username;
            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;

            var res = await _accountRepository.RegisterUser(user);
            return res;
        }

        public async Task<string> Authenticate(LoginDTO dto)
        {
            var user = await _accountRepository.Checkuser(dto.email, dto.password);

            if (user is not null)
            {
                Global.userId = user.id;
                var userRole = await _accountRepository.GetRole(user.id);

                bool isPasswordCorrect = VerifyHashPassword(dto.password, user.passwordHash, user.passwordSalt);

                if (isPasswordCorrect)
                {
                    var res = _jwtAuthenticationService.GenerateToken(user.id, dto.email, userRole);

                    // Add user's ConnectionId to the service when they login
                    //_userConnectionService.AddConnection(user.id.ToString(), connectionId);

                    // Get the user ID of the connected user (assuming you have a way to identify users)
                    var userId = user.id; // Example: Get user ID from claims

                    // Add user connection
                    //await _userConnectionService.AddUserConnection(userId.ToString(), "xE5kv_o3yK1d10uioSMiPw");

                    var session = _httpContextAccessor.HttpContext.Session;
                    session.SetString("Email", dto.email);
                    session.SetString("Username", user.username);
                    session.SetString("Role", userRole);
                    return res;
                }
                else
                    return "Invalid Password";
            }
            else
                return "Invalid Credentials";
        }

        public async Task<string> LogoutUser()
        {
            var session = _httpContextAccessor.HttpContext.Session;

            // Remove user's ConnectionId from the service when they logout
           // var userId = Global.userId.ToString(); // Assuming Global.userId is set during login
            //_userConnectionService.RemoveConnection(userId, connectionId);


            // Clear session data related to authentication
            session.Remove("Email");
            session.Remove("Role");
            session.Remove("Username");

            return "Logout Successfully";
        }

        public void PasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var h = new HMACSHA512())
            {
                passwordSalt = h.Key;
                passwordHash = h.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public bool VerifyHashPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var h = new HMACSHA512(passwordSalt))
            {
                var hash = h.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return hash.SequenceEqual(passwordHash);
            }
        }
    }
}
