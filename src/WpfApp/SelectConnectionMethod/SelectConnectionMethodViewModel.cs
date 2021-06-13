using Core;
using WpfApp.Toolkit;

namespace WpfApp.SelectConnectionMethod {
    public class SelectConnectionMethodViewModel : ViewModel {
        public SelectConnectionMethodViewModel(Player player) {
            _player = player;
            CreateGame = new(OnCreateGame);
            ConnectToGame = new(OnConnectToGame);
        }

        private readonly Player _player;

        public RelayCommand CreateGame { get; }
        public RelayCommand ConnectToGame { get; }

        public void OnCreateGame() { }

        public void OnConnectToGame() { }
    }
}
