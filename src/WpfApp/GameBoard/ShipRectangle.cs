using System;
using Core.Field;

namespace WpfApp.GameBoard {
    public record ShipRectangle : FilledRectangle {
        public ShipRectangle(Ship ship, Func<int, int> calculateOffset) : base(
            ship.Coordinates.Map(calculateOffset),
            new ShipSize(ship).Map(calculateOffset),
            "Blue"
        ) { }
    }
}
