using Microsoft.AspNetCore.SignalR;
using Sho8lana.DataAccess.Data;
using Sho8lana.Entities.Models;
using Sho8lana.Entities.Models.Dto.Chat;

namespace Sho8lana.API.Hubs
{
    //Todo: // authorize the hub
    public class ChatHub : Hub
    {
        private readonly ChatRepository _chatRepo;

        public ChatHub(ChatRepository chatRepo)
        {
            this._chatRepo = chatRepo;
        }

        public override Task OnConnectedAsync()
        {
            // add connected user to his groups
            var connectionId = Context.ConnectionId;

            var groupsId = _chatRepo.ReadGroupChats(Context.UserIdentifier!);

            foreach (var groupId in groupsId)
            {
                Groups.AddToGroupAsync(connectionId, groupId);
            }

            return base.OnConnectedAsync();
        }

        // Method for sending messages
        public async Task SendMessage(CreateMessageDto messagedto)
        {
            messagedto.SenderId = Context.UserIdentifier!;
            if (messagedto.MessageType == MessageType.Individual && messagedto.ReceiverId != null)
            {
                //send to user
                await Clients.User(messagedto.ReceiverId).SendAsync("ReceiveMessage", messagedto);
            }
            else if (messagedto.MessageType == MessageType.Group && messagedto.ReceiverId != null)
            {
                //send to group
                await Clients.Group(messagedto.ReceiverId).SendAsync("ReceiveMessage", messagedto);
            }
            // save to db
            _chatRepo.AddMessage(messagedto);
        }

        // Method for adding a user to a group
        public async Task AddToGroup(CreateGroupChatDto groupDto)
        {
            groupDto.AdminId = Context.UserIdentifier!;

            // save to db
            var group = _chatRepo.AddGroupChat(groupDto);

            // add admin to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, group.Id);       
        }
    }
}
