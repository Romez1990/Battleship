using System;
using Core.Field;
using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record CrossRectangle : Rectangle {
        public CrossRectangle(Cross cross, int cellSize, int thickness, Func<int, int> calculateOffset) : base(
            cross.Map(calculateOffset),
            new(cellSize)
        ) {
            Thickness = thickness;
            Length = cellSize;
        }

        public int Thickness { get; }
        public int Length { get; }
    }
}
