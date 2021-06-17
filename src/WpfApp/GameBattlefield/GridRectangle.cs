using System.ComponentModel;
using Core.Geometry;

namespace WpfApp.GameBattlefield {
    public record GridRectangle : Rectangle {
        public GridRectangle(Orientation orientation, int offset, int length, int thickness) : base(
            orientation switch {
                Orientation.Horizontal => new(0, offset),
                Orientation.Vertical => new(offset, 0),
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            },
            orientation switch {
                Orientation.Horizontal => new(length, thickness),
                Orientation.Vertical => new(thickness, length),
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            }
        ) { }
    }
}
