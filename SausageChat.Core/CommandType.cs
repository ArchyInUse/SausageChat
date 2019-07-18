namespace SausageChat.Core
{
    public enum CommandType
    {
        // when a user joins, a complete list of the current users online gets sent to him
        OnJoinUserList,
        // when a new user joins, a message is sent (to everyone but him) to add him to the list.
        UserListAppend,
        // when a user gets banned, this flag is used to notify everyone (including the current user)
        UserBanned,
        // when a user gets kicked, this flag is used to notify everyone (including the current user)
        UserKicked,
        // when a user gets muted, this flag is used to notify everyone (including the current user)
        UserMuted,
        // when a user gets unmuted, this flag is used to notify everyone (including the current user)
        UserUnmuted,
        // name change flag to announce to everyone
        NameChanged,
        // user blacklisted won't notify everyone, but just return it to the black listed client
        UserBlackListed,
        // when a user disconnects, this flag is used to notify everyone
        UserDisconnect,
        // for friend requests, we save Ips, so we need to make a request (this is used for DMing)
        IpRequest
    }
}
