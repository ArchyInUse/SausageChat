namespace SausageChat.Messaging
{
    class ServerMessage : IMessage
    {
        public string Content { get; set; }

        public ServerMessage(string content)
        {
            Content = content;
        }

        public string FormatMessage() => ToString(); //basically  sends formated message to the another string called "tostring". 

        public override string ToString() => Content; //Afterwards, that is sent to content. 
    }
}
