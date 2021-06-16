using System;
using System.Collections.Immutable;
using Core;
using Core.Field;
using WpfApp.GameBattlefield;
using WpfApp.Toolkit;

namespace WpfApp.GameBoard {
    public class GameBoardViewModel : ViewModel {
        public GameBoardViewModel(Player player, EventHandler<ShipsCreatedEventArgs> navigateToSelectConnectionMethod) {
            Player = player;
            NavigateToSelectConnectionMethod += navigateToSelectConnectionMethod;

            _randomShipPlacement = new();
            SetRandomField();

            Random = new(SetRandomField);
            StepNext = new(OnStepNext);
        }

        public Player Player { get; }

        private readonly RandomShipPlacement _randomShipPlacement;

        private Battlefield _battlefield;

        public Battlefield Battlefield {
            get => _battlefield;
            private set => SetProperty(ref _battlefield, value);
        }

        public RelayCommand Random { get; }
        public RelayCommand StepNext { get; }

        private ImmutableArray<Ship> _ships;

        private void SetRandomField() =>
            Battlefield = new(_ships = _randomShipPlacement.GetShips());

        private void OnStepNext() =>
            NavigateToSelectConnectionMethod?.Invoke(this, new(_ships));

        private event EventHandler<ShipsCreatedEventArgs> NavigateToSelectConnectionMethod;
    }
}
