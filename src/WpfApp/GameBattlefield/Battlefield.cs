using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Core.Field;
using Core.Geometry;
using WpfApp.Toolkit;

namespace WpfApp.GameBattlefield {
    public class Battlefield : Bindable {
        public Battlefield(IEnumerable<Ship> ships) {
            FieldSize = Field.Size.Map(CalculateOffset) + GridThickness + LabelSize;

            GraphicObjects = new() {
                new CollectionContainer { Collection = CreateGridLines() },
                new CollectionContainer { Collection = CreateRowLabels() },
                new CollectionContainer { Collection = _shipRectangles = new(ships.Map(ToShipRectangle)) },
            };
        }

        public const int LabelSize = 40;
        public const int LabelMargin = 10;
        public const int CellSize = 50;
        public const int GridThickness = 1;

        public Vector FieldSize { get; }

        public CompositeCollection GraphicObjects { get; }

        private ImmutableArray<GridRectangle> CreateGridLines() =>
            CreateGridLinesInRow(Field.Size.X, Orientation.Horizontal)
                .Concat(CreateGridLinesInRow(Field.Size.Y, Orientation.Vertical))
                .ToImmutableArray();

        private IEnumerable<GridRectangle> CreateGridLinesInRow(int count, Orientation orientation) =>
            Enumerable.Range(0, count + 1)
                .Map(i => new GridRectangle(i, orientation, CalculateOffset(count) - CalculateOffset(0), GridThickness,
                    CalculateOffset));

        private ImmutableArray<RowLabel> CreateRowLabels() =>
            CreateRowLabelsInRow(Field.Size.X, Orientation.Horizontal)
                .Concat(CreateRowLabelsInRow(Field.Size.Y, Orientation.Vertical))
                .ToImmutableArray();

        private IEnumerable<RowLabel> CreateRowLabelsInRow(int count, Orientation orientation) =>
            Enumerable.Range(0, count)
                .Map(i => new RowLabel(GetLabel(i, orientation), i, orientation, LabelSize, LabelMargin, CalculateOffset));

        private string GetLabel(int i, Orientation orientation) =>
            orientation switch {
                Orientation.Horizontal => (i + 1).ToString(),
                Orientation.Vertical => ((char)(i + 'А')).ToString(),
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            };

        private readonly ObservableCollection<ShipRectangle> _shipRectangles;

        private ShipRectangle ToShipRectangle(Ship ship) =>
            new(ship, CellSize, CalculateOffset);

        public void AddShip(Ship ship) {
            _shipRectangles.Add(ToShipRectangle(ship));
        }

        private int CalculateOffset(int i) =>
            LabelSize + i * (CellSize + GridThickness);
    }
}
