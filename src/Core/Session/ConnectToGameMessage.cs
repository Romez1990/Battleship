using Core.Sockets;

namespace Core.Session {
    public record ConnectToGameMessage : Message<PlayerData> {
        public ConnectToGameMessage(PlayerData playerData) : base(Type, playerData) { }

        private const string Type = "connect_to_game";
    }
}
