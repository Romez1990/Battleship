using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core;
using Core.Field;
using Core.Serializers;
using Core.Session;
using WpfApp.Toolkit;

namespace WpfApp.SelectConnectionMethod {
    public class SelectConnectionMethodViewModel : Form {
        public SelectConnectionMethodViewModel(Player player, ImmutableArray<Ship> ships,
            EventHandler<GameCreatedEventArgs> navigateToGame) {
            _player = player;
            _ships = ships;
            NavigateToGame += navigateToGame;
            _playerConnector = new(new JsonSerializer(), GameCreated);
            CreateGame = new(OnCreateGame, () => CanCreateGame);
        }

        private readonly Player _player;
        private readonly ImmutableArray<Ship> _ships;

        private readonly PlayerConnector _playerConnector;

        public RelayCommand CreateGame { get; }

        private string _inputGameCode = "";

        [Required]
        public string InputGameCode {
            get => _inputGameCode;
            set => SetProperty(ref _inputGameCode, value);
        }

        private string _createdInputGameCode = "";

        [Required]
        public string CreatedGameCode {
            get => _createdInputGameCode == "" ? "" : $"Ваш код {_createdInputGameCode}";
            set => SetProperty(ref _createdInputGameCode, value);
        }

        private bool _canCreateGame = true;

        private bool CanCreateGame {
            get => _canCreateGame;
            set {
                _canCreateGame = value;
                CreateGame.RaiseCanExecuteChanged();
            }
        }

        private async void OnCreateGame() {
            CanCreateGame = false;
            var sessionCode = await _playerConnector.CreateGame(_player, _ships);
            CreatedGameCode = sessionCode.Code;
        }

        private void GameCreated(object sender, GameCreatedEventArgs e) =>
            NavigateToGame?.Invoke(this, e);

        protected override async void OnSubmit() {
            var (isConnected, player) = await _playerConnector.ConnectToGame(_player, _ships, InputGameCode);
            if (isConnected)
                GameCreated(this, new(player));

            Errors[nameof(InputGameCode)] = isConnected
                ? Enumerable.Empty<string>().ToImmutableArray()
                : new[] { "Неверный код" }.ToImmutableArray();
        }

        private event EventHandler<GameCreatedEventArgs> NavigateToGame;
    }
}
