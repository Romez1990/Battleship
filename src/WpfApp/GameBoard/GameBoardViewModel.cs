using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core;
using Core.Field;
using Core.Geometry;
using WpfApp.Toolkit;

namespace WpfApp.GameBoard {
    public class GameBoardViewModel : ViewModel {
        public GameBoardViewModel(Player player, EventHandler<ShipsCreatedEventArgs> navigateToSelectConnectionMethod) {
            Player = player;
            NavigateToSelectConnectionMethod += navigateToSelectConnectionMethod;

            FieldSize = Field.Size.Map(CalculateGridOffset) + GridThickness;

            Random = new(SetRandomField);
            StepNext = new(OnStepNext);

            _randomShipPlacement = new();
            SetRandomField();
            Grid = MakeGrid();
        }

        public RelayCommand Random { get; }
        public RelayCommand StepNext { get; }

        public Player Player { get; }

        private readonly RandomShipPlacement _randomShipPlacement;

        public Vector FieldSize { get; }

        public const int CellSize = 50;
        public const int GridThickness = 1;

        private ImmutableArray<Ship> _ships;

        private ImmutableArray<ShipRectangle> _shipRectangleRectangles;

        public ImmutableArray<ShipRectangle> ShipRectangles {
            get => _shipRectangleRectangles;
            private set => SetProperty(ref _shipRectangleRectangles, value);
        }

        private void SetRandomField() =>
            ShipRectangles = (_ships = _randomShipPlacement.GetShips())
                .Map(ship => new ShipRectangle(ship, CalculateGridOffset))
                .ToImmutableArray();

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

        private void OnStepNext() =>
            NavigateToSelectConnectionMethod?.Invoke(this, new(_ships));

        private event EventHandler<ShipsCreatedEventArgs> NavigateToSelectConnectionMethod;
    }
}
