using Core.Geometry;

namespace Core.Field {
    public record Ship(Vector Coordinates, int Size, Orientation Orientation) {
        public int X => Coordinates.X;
        public int Y => Coordinates.Y;
    }
}
