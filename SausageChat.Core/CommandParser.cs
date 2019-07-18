/*
 * the reason we don't have messaging here is because Client doesn't contain User which is a part of
 * the server side UserMessage class and both projects work with the messages differently
 * so I've decided to keep them separated
 * - Archy
 */

namespace SausageChat.Core
{
    public static class CommandParser
    {
        public static string ToString(CommandType Ct)
        {
            switch(Ct)
            {
                case CommandType.NameChanged:
                    return "<NC>";
                case CommandType.OnJoinUserList:
                    return "<UL>";
                case CommandType.UserBanned:
                    return "<UB>";
                case CommandType.UserKicked:
                    return "<UK>";
                case CommandType.UserMuted:
                    return "<UM>";
                case CommandType.UserBlackListed:
                    return "<UBL>";
                case CommandType.UserListAppend:
                    return "<ULA>";
                case CommandType.UserUnmuted:
                    return "<UU>";
                case CommandType.IpRequest:
                    return "<IR>";
                default:
                    return null;
            }
        }
    }
}
