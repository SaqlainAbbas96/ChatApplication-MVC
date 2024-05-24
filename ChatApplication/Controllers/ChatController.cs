using ChatApplication.Hubs;
using ChatApplication.Models;
using ChatApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplication.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ChatController : Controller
    {
        public readonly IHubContext<ChatHub> _hubContext;
        private readonly IUserService _userService;
        private readonly IChatService _chatService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMessageService _messageService;
        public ChatController(
            IHubContext<ChatHub> hubContext, 
            IUserService userService, 
            IHttpContextAccessor httpContextAccessor,
            IChatService chatService,
            IMessageService messageService)
        {
            _hubContext = hubContext;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _chatService = chatService;
            _messageService = messageService;
        }

        [HttpGet("test")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("OpenChatRoom")]
        public async Task<IActionResult> OpenChatRoom(int userId, string connectionId)
        {
            // Retrieve username from session
            string username = _httpContextAccessor.HttpContext.Session.GetString("Username");

            // Pass username to the layout view
            ViewBag.Username = username;
            
            var client = await _userService.GetClientName(userId);

            ViewBag.ClientId = client.userId;
            ViewBag.ClientName = client.username;
            ViewBag.ConnectionId = connectionId;

            var messages = await _messageService.GetMessages(username, client.username);

            return View(messages);
        }


        [HttpGet("OpenGroupChatRoom")]
        public async Task<IActionResult> OpenGroupChatRoom(int userId)
        {
            // Retrieve username from session
            string username = _httpContextAccessor.HttpContext.Session.GetString("Username");

            // Pass username to the layout view
            ViewBag.Username = username;

            var client = await _userService.GetClientName(userId);

            ViewBag.ClientId = client.userId;
            ViewBag.ClientName = client.username;

            var messages = await _messageService.GetMessages(username);

            return View(messages);
        }

        public record SendMessageDTO(string User, string ClientName, string Message);


        //Now need to pass username instead of hardcode receiverConnectionId, get username of chatroom
        //example: Hello! Welcome to the Chat Room of Client B (get client b username)
        //pass as parameter and read key of client b from db before sending message
        [HttpPost("send-message")]
        public async Task<IActionResult> SendToUser([FromBody] SendMessageDTO messageDTO)
        {
            try
            {
                // Call the service method to send the message
                string res = await _chatService.SendMessageToUser(messageDTO.User, messageDTO.ClientName, messageDTO.Message);
                //await _hubContext.Clients.Client(messageDTO.ReceiverConnectionId).SendAsync("ReceiveMessage", messageDTO.User, messageDTO.Message);

                if (res == "Message sent successfully") 
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to send message");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /*
         * 1. create chat service to call hub and call service in SendToUser
         * 2. create user mapping bind key with the user (can create another table for that)
         * 2. save chat into database
         */

        public record SendMessageToAllDTO(string User, string Message);

        [HttpPost("send-message-to-all")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageToAllDTO messageDTO)
        {
            try
            {
                string res = await _chatService.SendMessageToGroup(messageDTO.User, messageDTO.Message);

                //await _hubContext.Clients.All.SendAsync("ReceiveMessage", messageDTO.User, messageDTO.Message);
                if (res == "Message sent successfully")
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Failed to send message");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
