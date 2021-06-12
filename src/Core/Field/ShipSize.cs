using System.ComponentModel;
using Core.Geometry;

namespace Core.Field {
    public record ShipSize : Vector {
        public ShipSize(Ship ship) : base(ship.Orientation switch {
            Orientation.Horizontal => new Vector(ship.Size, 1),
            Orientation.Vertical => new Vector(1, ship.Size),
            _ => throw new InvalidEnumArgumentException(nameof(ship.Orientation), (int)ship.Orientation,
                ship.Orientation.GetType()),
        }) { }
    }
}
