/*
 * the reason we don't have messaging here is because Client doesn't contain User which is a part of
 * the server side UserMessage class and both projects work with the messages differently
 * so I've decided to keep them separated
 * - Archy (Disco)
 */
using SausageChat.Core.Messaging;

namespace SausageChat.Core
{
    public static class SausageHelper
    {
        public static string CommandToString(MessageType Ct)
        {
            switch(Ct)
            {
                case MessageType.NameChanged:
                    return "<NC>";
                case MessageType.UserList:
                    return "<UL>";
                case MessageType.UserBanned:
                    return "<UB>";
                case MessageType.UserKicked:
                    return "<UK>";
                case MessageType.UserMuted:
                    return "<UM>";
                case MessageType.UserBlackListed:
                    return "<UBL>";
                case MessageType.UserListAppend:
                    return "<ULA>";
                case MessageType.UserUnmuted:
                    return "<UU>";
                case MessageType.IpRequest:
                    return "<IR>";
                case MessageType.UserMessage:
                    return "<UMS>";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Checks if str has the message code for Ct
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Ct">Command</param>
        /// <returns></returns>
        public static bool Contains(this string str, MessageType Ct) => str.Substring(0,4).Contains(CommandToString(Ct));

        /// <summary>
        /// Turns the CommandType to string using the CommandToString method
        /// </summary>
        /// <param name="Ct">Command that will get parsed</param>
        /// <returns>Message code for the given command</returns>
        public static string ToStr(this MessageType Ct) => CommandToString(Ct);
    }
}
