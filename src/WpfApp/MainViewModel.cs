using WpfApp.GameBoard;
using WpfApp.GameStart;
using WpfApp.Toolkit;

namespace WpfApp {
    public class MainViewModel : ViewModel {
        public MainViewModel() {
            var gameStartViewModel = new GameStartViewModel();
            gameStartViewModel.NavigateToGameBoard += NavigateToGameBoard;
            _currentViewModel = gameStartViewModel;
            _currentViewModel = new GameBoardViewModel(new("123", "123"));
        }

        private ViewModel _currentViewModel;

        public ViewModel CurrentViewModel {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        private void NavigateToGameBoard(object sender, PlayerCreatedEventArgs e) {
            CurrentViewModel = new GameBoardViewModel(e.Player);
        }
    }
}
