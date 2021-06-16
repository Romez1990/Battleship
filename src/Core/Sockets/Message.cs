namespace Core.Sockets {
    public record Message<T>(string MessageType, T Data);
}
