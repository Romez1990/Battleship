using System;
using System.ComponentModel;
using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record GridRectangle : Rectangle {
        public GridRectangle(int i, Orientation orientation, int length, int thickness, Func<int, int> calculateOffset) : base(
            orientation switch {
                Orientation.Horizontal => new(calculateOffset(0), calculateOffset(i)),
                Orientation.Vertical => new(calculateOffset(i), calculateOffset(0)),
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            },
            orientation switch {
                Orientation.Horizontal => new(length, thickness),
                Orientation.Vertical => new(thickness, length),
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            }) { }
    }
}
