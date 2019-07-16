using SausageChat.Networking;
namespace SausageChat.Messaging
{
    class UserMessage : IMessage
    {
        public string Content { get; set; } //allows to get or set the string.
        public User Author { get; set; } // will allow us to get or set the user. User is custom defined type.

        public UserMessage(User author, string content)
        {
            Content = content;
            Author = author;
        }

        public string FormatMessage()
        {
            return ToString();  //this will show what is the current string of "toString". 
        }

        public override string ToString()
        {
            return $"<{Author}>:{Content}";
        }
    }
}
