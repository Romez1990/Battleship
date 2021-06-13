using Core;
using WpfApp.GameBoard;
using WpfApp.GameStart;
using WpfApp.SelectConnectionMethod;
using WpfApp.Toolkit;

namespace WpfApp {
    public class MainViewModel : ViewModel {
        public MainViewModel() {
            SetGameStart();
            SetGameBoard(null, new(new("Максим", "Жуков")));
        }

        private ViewModel _currentViewModel;

        public ViewModel CurrentViewModel {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        private void SetGameStart() =>
            _currentViewModel = new GameStartViewModel(SetGameBoard);

        private Player _player;

        private void SetGameBoard(object sender, PlayerCreatedEventArgs e) =>
            CurrentViewModel = new GameBoardViewModel(_player = e.Player, SetSelectConnectionMethod);

        private void SetSelectConnectionMethod(object sender, ShipsCreatedEventArgs e) =>
            CurrentViewModel = new SelectConnectionMethodViewModel(_player);
    }
}
