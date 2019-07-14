namespace SausageChat.Messaging
{
    public interface IMessage
    {
        string Content { get; set; }
        string FormatMessage();
        string ToString();
    }
}
