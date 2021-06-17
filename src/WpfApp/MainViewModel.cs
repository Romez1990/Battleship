using System.Collections.Immutable;
using Core;
using Core.Field;
using Core.Session;
using WpfApp.GameBattlefield;
using WpfApp.GameBoard;
using WpfApp.GameStart;
using WpfApp.SelectConnectionMethod;
using WpfApp.Toolkit;

namespace WpfApp {
    public class MainViewModel : ViewModel {
        public MainViewModel() {
            SetGameStart();
#if DEBUG
            _player = new("Максим", "Жуков");
            var randomShipPlacement = new RandomShipPlacement();
            _ships = randomShipPlacement.GetShips();

            // SetGameBoard(this, new(_player));

            // SetSelectConnectionMethod(null, new(_ships));
            // ((SelectConnectionMethodViewModel)CurrentViewModel).CreateGame.Execute(null);

            // SetGameSession(this, new(_player));

            SetGameBoard(this, new(_player));
#endif
        }

        private ViewModel _currentViewModel;

        public ViewModel CurrentViewModel {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        private Player _player;
        private Player _enemy;
        private ImmutableArray<Ship> _ships;

        private void SetGameStart() =>
            _currentViewModel = new GameStartViewModel(SetGameBoard);

        private void SetGameBoard(object sender, PlayerCreatedEventArgs e) =>
            CurrentViewModel = new GameBoardViewModel(_player = e.Player, SetSelectConnectionMethod);

        private void SetSelectConnectionMethod(object sender, ShipsCreatedEventArgs e) =>
            CurrentViewModel = new SelectConnectionMethodViewModel(_player, _ships = e.Ships, SetGameSession);

        private void SetGameSession(object sender, GameCreatedEventArgs e) =>
            CurrentViewModel = new BattlefieldViewModel(_player, _enemy = e.Enemy, _ships);
    }
}
