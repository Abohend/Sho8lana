namespace src.Models.Dto.Chat
{
    public class CreateGroupChatDto
    {
        public string Name { get; set; } = string.Empty;
        public string AdminId { get; internal set; } = string.Empty;
    }
}
