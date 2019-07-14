namespace SausageChat.Networking
{
    /// <summary>
    /// result from a server command (kick, mute, ban etc) on a user
    /// </summary>
    enum ServerCommandResult
    {
        Success,
        UserNotFound,
        UserIsNull
    }
}
