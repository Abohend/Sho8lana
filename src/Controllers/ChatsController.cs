using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sho8lana.DataAccess.Data;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Message;
using System.Security.Claims;

namespace Sho8lana.API.Controllers
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
