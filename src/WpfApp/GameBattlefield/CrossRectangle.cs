using System;
using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record CrossRectangle : Rectangle {
        public CrossRectangle(Vector coordinates, int cellSize, int thickness, Func<int, int> calculateOffset) : base(
            coordinates.Map(calculateOffset),
            new(cellSize, cellSize)
        ) {
            Thickness = thickness;
            Length = cellSize;
        }

        public int Thickness { get; }
        public int Length { get; }
    }
}
