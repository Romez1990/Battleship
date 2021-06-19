using Core.Sockets;

namespace Core.GameSession {
    public record EnemyGoMessage : Message<object> {
        public EnemyGoMessage() : base("enemy_go", null) { }
    }

}
