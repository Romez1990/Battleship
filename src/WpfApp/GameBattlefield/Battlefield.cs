using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using Core.Field;
using Core.Geometry;
using WpfApp.Toolkit;

namespace WpfApp.GameBattlefield {
    public class Battlefield : Bindable {
        public Battlefield(IEnumerable<Ship> ships) {
            FieldSize = Field.Size.Map(CalculateOffset) + GridThickness;

            GraphicObjects = new() {
                new CollectionContainer { Collection = CreateGridLines() },
                new CollectionContainer { Collection = _shipRectangles = new(ships.Map(ToShipRectangle)) },
            };
        }

        public const int CellSize = 50;
        public const int GridThickness = 1;

        public Vector FieldSize { get; }

        public CompositeCollection GraphicObjects { get; }

        private ImmutableArray<GridRectangle> CreateGridLines() =>
            CreateGridLinesByOrientation(Field.Size.X, Orientation.Horizontal)
                .Concat(CreateGridLinesByOrientation(Field.Size.Y, Orientation.Vertical))
                .ToImmutableArray();

        private IEnumerable<GridRectangle> CreateGridLinesByOrientation(int count, Orientation orientation) =>
            Enumerable.Range(0, count + 1)
                .Map(CalculateOffset)
                .Map(offset => new GridRectangle(orientation, offset, CalculateOffset(count), GridThickness));

        private readonly ObservableCollection<ShipRectangle> _shipRectangles;

        private ShipRectangle ToShipRectangle(Ship ship) =>
            new(ship, CalculateOffset);

        public void AddShip(Ship ship) {
            _shipRectangles.Add(ToShipRectangle(ship));
        }

        private int CalculateOffset(int i) =>
            i * (CellSize + GridThickness);
    }
}
