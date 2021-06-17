using Core.Geometry;

namespace Core.Field {
    public record ShipBorder : Rectangle {
        public ShipBorder(Ship ship) : base(ship.Coordinates - 1, new ShipSize(ship) + 2) { }
        public ShipBorder(Rectangle rectangle) : base(rectangle) { }

        public new ShipBorder StretchStart(Vector size) =>
            new(base.StretchStart(size));

        public bool Collides(Ship ship) =>
            Collides(new Rectangle(ship.Coordinates, new ShipSize(ship)));
    }
}
