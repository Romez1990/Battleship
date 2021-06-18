using Core.Geometry;
using Core.Sockets;

namespace Core.GameSession {
    public record MoveMessage : Message<MoveData> {
        public MoveMessage(Vector coordinates) : base("move", new(coordinates)) { }
    }

    public record MoveData(Vector Coordinates);
}
