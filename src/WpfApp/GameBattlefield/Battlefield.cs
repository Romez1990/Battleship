using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using Core.Field;
using Core.Geometry;
using WpfApp.GameBoard;
using WpfApp.Toolkit;

namespace WpfApp.GameBattlefield {
    public class Battlefield : Bindable {
        public Battlefield(IEnumerable<Ship> ships) {
            // var shipsList = ships.ToList();
            // _ships = new(shipsList);
            ShipRectangles = new(ships.Map(ToShipRectangle));
            Grid = MakeGrid();
            FieldSize = Field.Size.Map(CalculateGridOffset) + GridThickness;
        }

        // private readonly ObservableCollection<Ship> _ships;

        private ShipRectangle ToShipRectangle(Ship ship) =>
            new(ship, CalculateGridOffset);

        public Vector FieldSize { get; }

        public const int CellSize = 50;
        public const int GridThickness = 1;

        public ObservableCollection<ShipRectangle> ShipRectangles { get; }

        public void AddShip(Ship ship) {
            // _ships.Add(ship);
            ShipRectangles.Add(ToShipRectangle(ship));
        }

        public ImmutableArray<GridRectangle> Grid { get; }

        private ImmutableArray<GridRectangle> MakeGrid() =>
            CreateGridLines(Field.Size.X, Orientation.Horizontal)
                .Concat(CreateGridLines(Field.Size.Y, Orientation.Vertical))
                .ToImmutableArray();

        private IEnumerable<GridRectangle> CreateGridLines(int count, Orientation orientation) =>
            Enumerable.Range(0, count + 1)
                .Map(CalculateGridOffset)
                .Map(offset => new GridRectangle(orientation, offset, CalculateGridOffset(count), GridThickness));



        private int CalculateGridOffset(int i) =>
            i * (CellSize + GridThickness);
    }
}
