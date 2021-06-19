using System;
using System.Collections.Immutable;
using System.Windows.Threading;
using Core.Connection;
using Core.Field;
using Core.PlayerData;
using WpfApp.GameSession;
using WpfApp.GameStart;
using WpfApp.PlacementOfShips;
using WpfApp.Scoreboard;
using WpfApp.SelectConnectionMethod;
using WpfApp.Toolkit;

namespace WpfApp {
    public class MainViewModel : ViewModel {
        public MainViewModel() {
            SetGameStart();
#if DEBUG
            _player = new("Валида", "Насирова");
            _enemy = new("Иван", "Иванов");
            _ships = new RandomShipPlacement().GetShips();

            // SetPlacementOfShips(this, new(_player));
            SetSelectConnectionMethod(this, new(_ships));
            // SetGameSession(this, new(null, true, _enemy));
            // SetScoreboard(this, EventArgs.Empty);
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
            _currentViewModel = new GameStartViewModel(SetPlacementOfShips);

        private void SetPlacementOfShips(object sender, PlayerCreatedEventArgs e) =>
            CurrentViewModel = new PlacementOfShipsViewModel(_player = e.Player, SetSelectConnectionMethod);

        private void SetSelectConnectionMethod(object sender, ShipsPlacedEventArgs e) =>
            CurrentViewModel = new SelectConnectionMethodViewModel(_player, _ships = e.Ships, SetGameSession);

        private void SetGameSession(object sender, GameCreatedEventArgs e) =>
            Dispatcher.CurrentDispatcher.Invoke(() =>
                CurrentViewModel = new GameSessionViewModel(e.Socket, _player, _enemy = e.Enemy, _ships, e.IsPlayerGoing));

        private void SetScoreboard(object sender, EventArgs e) =>
            CurrentViewModel = new ScoreboardViewModel();
    }
}
