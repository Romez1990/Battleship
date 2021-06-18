using System.Collections.Immutable;
using Core;
using Core.Connection;
using Core.Field;
using Core.PlayerData;
using Core.Serializers;
using WpfApp.GameSession;
using WpfApp.GameStart;
using WpfApp.PlacementOfShips;
using WpfApp.SelectConnectionMethod;
using WpfApp.Toolkit;

namespace WpfApp {
    public class MainViewModel : ViewModel {
        public MainViewModel() {
            SetGameStart();
#if DEBUG
            _player = new("Максим", "Жуков");
            _ships = new RandomShipPlacement().GetShips();

            // SetPlacementOfShips(this, new(_player));

            // SetSelectConnectionMethod(this, new(_ships));

            SetGameSession(this, new(new(new JsonSerializer(), (sender, args) => { }), _player));
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
        private PlayerConnector _playerConnector;

        private void SetGameStart() =>
            _currentViewModel = new GameStartViewModel(SetPlacementOfShips);

        private void SetPlacementOfShips(object sender, PlayerCreatedEventArgs e) =>
            CurrentViewModel = new PlacementOfShipsViewModel(_player = e.Player, SetSelectConnectionMethod);

        private void SetSelectConnectionMethod(object sender, ShipsPlacedEventArgs e) =>
            CurrentViewModel = new SelectConnectionMethodViewModel(_player, _ships = e.Ships, SetGameSession);

        private void SetGameSession(object sender, GameCreatedEventArgs e) =>
            CurrentViewModel = new GameSessionViewModel(_playerConnector = e.PlayerConnector, _player, _enemy = e.Enemy, _ships);
    }
}
