using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Core;
using Core.Field;
using Core.Geometry;
using WpfApp.Toolkit;

namespace WpfApp.GameBoard {
    public class GameBoardViewModel : ViewModel {
        public GameBoardViewModel(Player player) {
            _player = player;

            FieldSize = Field.Size.Map(CalculateGridOffset) + GridThickness;

            Random = new(SetRandomField);
            StepNext = new(OnStepNext);

            _randomShipPlacement = new();
            SetRandomField();
            Grid = MakeGrid();
        }

        public RelayCommand Random { get; }
        public RelayCommand StepNext { get; }

        private readonly Player _player;

        public string PlayerName => $@"Игрок {_player.FirstName} {_player.LastName}";

        private readonly RandomShipPlacement _randomShipPlacement;

        public Vector FieldSize { get; }

        public const int CellSize = 50;
        public const int GridThickness = 1;

        private ImmutableArray<ShipRectangle> _ships;

        public ImmutableArray<ShipRectangle> Ships {
            get => _ships;
            private set => SetProperty(ref _ships, value);
        }

        private void SetRandomField() {
            Ships = _randomShipPlacement.GetShips()
                .Map(ship => new ShipRectangle(ship, CalculateGridOffset))
                .ToImmutableArray();
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

        private void OnStepNext() {

        }
    }
}
