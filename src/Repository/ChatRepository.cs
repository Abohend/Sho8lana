using AutoMapper;
using Microsoft.EntityFrameworkCore;
using src.Data;
using src.Models;
using src.Models.Dto.Chat;
using src.Models.Dto.Message;

namespace src.Repository
{
    public class ChatRepository
    {
        private readonly Context _db;
        private readonly IMapper _mapper;

        public ChatRepository(Context db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }

        public List<string> ReadGroupChats(string userId)
        {
            return _db.GroupChats
                .Include(gch => gch.Members)
                .Where(gch => gch.Members!.Select(m => m.Id).Contains(userId))
                .Select(gch => gch.Id)
                .ToList();
        }

        public List<Message> ReadMessages(RequstMessageDto requestmsg)
        {
            if (requestmsg.ChatType == MessageType.Group)
            {
                return _db.Messages
                    .Where(msg => msg.ReceiverGroupId == requestmsg.receiverId)
                    .ToList();
            }
            else
            {
                return _db.Messages
                    .Where(msg => (msg.SenderId == requestmsg.senderId && msg.ReceiverUserId == requestmsg.receiverId) ||
                                  (msg.SenderId == requestmsg.receiverId && msg.ReceiverUserId == requestmsg.senderId))
                    .ToList();
            }
                
        }

        public void AddMessage(CreateMessageDto messageDto)
        {
            var message = _mapper.Map<Message>(messageDto);
            _db.Messages.Add(message);
            _db.SaveChanges();
        }

        public GroupChat AddGroupChat(CreateGroupChatDto groupChatDto)
        {
            var groupChat = _mapper.Map<GroupChat>(groupChatDto);
            _db.GroupChats.Add(groupChat);
            _db.SaveChanges();
            return groupChat;
        }
    }
}
