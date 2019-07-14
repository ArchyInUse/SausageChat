namespace SausageChat.Messaging
{
    class ServerMessage : IMessage
    {
        public string Content { get; set; }

        public ServerMessage(string content)
        {
            Content = content;
        }

        public string FormatMessage() => ToString();

        public override string ToString() => Content;
    }
}
