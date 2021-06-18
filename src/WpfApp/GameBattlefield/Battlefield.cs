using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Core.Field;
using Core.Geometry;
using LanguageExt;
using WpfApp.Toolkit;
using static LanguageExt.Prelude;

namespace WpfApp.GameBattlefield {
    public class Battlefield : Bindable {
        public Battlefield(IEnumerable<Ship> ships) {
            FieldSize = Field.Size.Map(ShipOffset) + GridThickness + LabelSize;

            GraphicObjects = new() {
                new CollectionContainer { Collection = new[] { new BackgroundRectangle(FieldSize) } },
                new CollectionContainer { Collection = CreateGridLines() },
                new CollectionContainer { Collection = CreateRowLabels() },
                new CollectionContainer { Collection = _shipRectangles = new(ships.Map(ToShipRectangle)) },
                new CollectionContainer { Collection = _crossRectangles },
            };
        }

        public const int CellSize = 50;
        public const int GridThickness = 1;
        public const int LabelSize = 40;
        public const int LabelMargin = 10;
        public const int CrossThickness = 4;

        public Vector FieldSize { get; }

        public CompositeCollection GraphicObjects { get; }

        private ImmutableArray<GridRectangle> CreateGridLines() =>
            CreateGridLinesInRow(Field.Size.X, Orientation.Horizontal)
                .Concat(CreateGridLinesInRow(Field.Size.Y, Orientation.Vertical))
                .ToImmutableArray();

        private IEnumerable<GridRectangle> CreateGridLinesInRow(int count, Orientation orientation) =>
            Enumerable.Range(0, count + 1)
                .Map(i => new GridRectangle(i, orientation, GridLineOffset(count) - GridLineOffset(0) + GridThickness,
                    GridThickness,
                    GridLineOffset));

        private ImmutableArray<RowLabel> CreateRowLabels() =>
            CreateRowLabelsInRow(Field.Size.X, Orientation.Horizontal)
                .Concat(CreateRowLabelsInRow(Field.Size.Y, Orientation.Vertical))
                .ToImmutableArray();

        private IEnumerable<RowLabel> CreateRowLabelsInRow(int count, Orientation orientation) =>
            Enumerable.Range(0, count)
                .Map(i => new RowLabel(GetLabel(i, orientation), i, orientation, LabelSize, LabelMargin,
                    ShipOffset));

        private string GetLabel(int i, Orientation orientation) =>
            orientation switch {
                Orientation.Horizontal => (i + 1).ToString(),
                Orientation.Vertical => ((char)(i + 'А')).ToString(),
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            };

        private readonly ObservableCollection<ShipRectangle> _shipRectangles;

        private ShipRectangle ToShipRectangle(Ship ship) =>
            new(ship, CellSize, GridThickness, ShipOffset);

        public void AddShip(Ship ship) =>
            _shipRectangles.Add(ToShipRectangle(ship));

        private readonly List<Cross> _crosses = new();
        private readonly ObservableCollection<CrossRectangle> _crossRectangles = new();

        public void AddCross(Vector coordinates) {
            var cross = new Cross(coordinates);
            if (_crosses.Contains(cross)) return;
            _crosses.Add(cross);
            _crossRectangles.Add(new(cross, CellSize, CrossThickness, ShipOffset));
        }

        public Option<Vector> CalculateCoordinates(Vector coordinates) {
            var location = coordinates.Map(CalculateCoordinate);
            var insideBattlefield = location.Every(n => n is >= 0 and < 10);
            if (insideBattlefield) return location;
            return None;
        }

        private int ShipOffset(int i) =>
            LabelSize + i * (CellSize + GridThickness) + GridThickness;

        private int GridLineOffset(int i) =>
            LabelSize + i * (CellSize + GridThickness);

        private int CalculateCoordinate(int offset) {
            var dry = offset - LabelSize - GridThickness;
            if (dry < 0) return -1;
            return dry / (CellSize + GridThickness);
        }
    }
}
