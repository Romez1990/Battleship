using System;
using System.ComponentModel;
using System.Windows;
using Core.Geometry;
using Vector = Core.Geometry.Vector;

namespace WpfApp.GameBattlefield {
    public record RowLabel : Rectangle {
        public RowLabel(string label, int i, Orientation orientation, int size, int margin,
            Func<int, int> calculateOffset) : base(
            orientation switch {
                Orientation.Horizontal => new Vector(0, calculateOffset(i)),
                Orientation.Vertical => new Vector(calculateOffset(i), 0),
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            } + margin,
            new Vector(size) - 2 * margin
        ) {
            Label = label;
            HorizontalAlignment = orientation switch {
                Orientation.Horizontal => TextAlignment.Right,
                Orientation.Vertical => TextAlignment.Center,
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            };
            VerticalAlignment = orientation switch {
                Orientation.Horizontal => VerticalAlignment.Center,
                Orientation.Vertical => VerticalAlignment.Bottom,
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            };
        }

        public string Label { get; }
        public TextAlignment HorizontalAlignment { get; }
        public VerticalAlignment VerticalAlignment { get; }
    }
}
