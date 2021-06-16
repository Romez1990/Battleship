using System;
using Core.Field;

namespace WpfApp.GameBattlefield {
    public record ShipRectangle : FilledRectangle {
        public ShipRectangle(Ship ship, Func<int, int> calculateOffset) : base(
            ship.Coordinates.Map(calculateOffset),
            new ShipSize(ship).Map(calculateOffset),
            "Blue"
        ) { }
    }
}
