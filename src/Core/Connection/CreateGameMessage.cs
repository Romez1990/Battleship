using Core.Sockets;

namespace Core.Connection {
    public record CreateGameMessage : Message<PlayerData> {
        public CreateGameMessage(PlayerData playerData) : base(Type, playerData) { }

        private const string Type = "create_game";
    }
}
