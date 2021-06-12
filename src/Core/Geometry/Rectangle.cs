namespace Core.Geometry {
    public record Rectangle(Vector Coordinates, Vector Size, Vector EndCoordinates) {
        public Rectangle(Vector coordinates, Vector size) : this(coordinates, size, coordinates + size) { }

        public int X => Coordinates.X;
        public int Y => Coordinates.Y;
        public int Width => Size.X;
        public int Height => Size.Y;

        public bool Collides(Rectangle other) {
            var collidesOnX = Coordinates.X < other.EndCoordinates.X && other.Coordinates.X < EndCoordinates.X;
            var collidesOnY = Coordinates.Y < other.EndCoordinates.Y && other.Coordinates.Y < EndCoordinates.Y;
            return collidesOnX && collidesOnY;
        }

        public bool Collides(Vector coordinates) =>
            Collides(new Rectangle(coordinates, Vector.Unit));
    }
}
