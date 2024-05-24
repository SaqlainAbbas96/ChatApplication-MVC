using ChatApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Linq;

namespace ChatApplication.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;

        }

        public async Task<IActionResult> Index()
        {
            // Retrieve username from session
            
            string username = _httpContextAccessor.HttpContext.Session.GetString("Username");
            int userId = await _userService.GetUserId(username);

            // Pass username to the layout view
            ViewBag.Username = username;
            ViewBag.UserId = userId;

            var res = await _userService.GetAllUsers();
            return View(res);
        }

        [HttpGet("get-username")]
        public async Task<string> GetUsername()
        {
            var username = _httpContextAccessor.HttpContext.Session.GetString("Username");
            return username;
            //var handler = new JwtSecurityTokenHandler();
            //var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            //string Name = jsonToken?.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value!;

            //if (Name != null)
            //{
            //    string username = Name;
            //    return Ok(username);
            //}
            //else
            //{
            //    // Username claim not found in the token
            //    return BadRequest("Username claim not found in the token.");
            //}
        }

    }
}
