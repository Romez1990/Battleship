using Core.Sockets;

namespace Core.Session {
    public record CreateGameMessage : Message<PlayerData> {
        public CreateGameMessage(PlayerData playerData) : base(Type, playerData) { }

        private const string Type = "create_game";
    }
}
