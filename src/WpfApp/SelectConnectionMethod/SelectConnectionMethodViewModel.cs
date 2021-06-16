using System;
using System.ComponentModel.DataAnnotations;
using Core.Session;
using WpfApp.Toolkit;

namespace WpfApp.SelectConnectionMethod {
    public class SelectConnectionMethodViewModel : Form {
        public SelectConnectionMethodViewModel(EventHandler navigateToCreateGame,
            EventHandler<ConnectToGameEventArgs> navigateToConnectToGame) {
            NavigateToCreateGame += navigateToCreateGame;
            NavigateToConnectToGame += navigateToConnectToGame;
            _playerConnector = new(GameCreated);
            CreateGame = new(OnCreateGame);
        }

        private readonly PlayerConnector _playerConnector;

        public RelayCommand CreateGame { get; }

        private string _gameCode = "";

        [Required]
        public string GameCode {
            get => _gameCode;
            set => SetProperty(ref _gameCode, value);
        }

        public async void OnCreateGame() {
            var sessionCode = await _playerConnector.CreateGame();
        }

        private void GameCreated(object sender, EventArgs e) =>
            NavigateToCreateGame?.Invoke(this, EventArgs.Empty);

        protected override void OnSubmit() =>
            NavigateToConnectToGame?.Invoke(this, new(GameCode));

        private event EventHandler NavigateToCreateGame;
        private event EventHandler<ConnectToGameEventArgs> NavigateToConnectToGame;
    }
}
