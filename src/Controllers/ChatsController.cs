using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;
using src.Models.Dto.Message;
using src.Repository;
using System.Security.Claims;

namespace src.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ChatRepository _chatRepo;

        public ChatsController(ChatRepository chatRepo)
        {
            _chatRepo = chatRepo;
        }

        [HttpGet("{userId}")]
        public IActionResult Get(string userId)
        {
            var groupChats = _chatRepo.ReadGroupChats(userId);
            return Ok(new Response(200, result: groupChats));
        }

        [HttpGet]
        public IActionResult Get(RequstMessageDto requestmsg)
        {
            requestmsg.senderId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var messages = _chatRepo.ReadMessages(requestmsg);
            return Ok(new Response(200, messages));
        }
    }
}
