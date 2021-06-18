using Core.PlayerData;

namespace Core.Connection {
    public record ConnectionToGameResult(bool IsConnected, Player Enemy, bool? Go);
}
