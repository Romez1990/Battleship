using System;
using Core.Field;
using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record ShipRectangle : Rectangle {
        public ShipRectangle(Ship ship, int cellSize, int gridThickness, Func<int, int> calculateOffset) : base(
            ship.Coordinates.Map(calculateOffset),
            new ShipSize(ship) * (cellSize + gridThickness) - gridThickness
        ) { }
    }
}
