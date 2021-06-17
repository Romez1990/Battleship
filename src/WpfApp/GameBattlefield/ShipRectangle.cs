using System;
using Core.Field;
using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record ShipRectangle : Rectangle {
        public ShipRectangle(Ship ship, Func<int, int> calculateOffset) : base(
            ship.Coordinates.Map(calculateOffset),
            new ShipSize(ship).Map(calculateOffset)
        ) { }
    }
}
