using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Core;
using WpfApp.Toolkit;

namespace WpfApp.GameStart {
    public class GameStartViewModel : Form {
        public GameStartViewModel() {
            StepNext = new RelayCommand(OnStepNext);
            Validate = new RelayCommand(ValidateAllProperties);
        }

        private string _firstName = "";
        private string _lastName = "";

        [Required]
        public string FirstName {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        [Required]
        public string LastName {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public ICommand StepNext { get; }
        public ICommand Validate { get; }

        private void OnStepNext() {
            var player = new Player(FirstName, LastName);
            NavigateToGameBoard?.Invoke(this, new(player));
        }

        public event EventHandler<PlayerCreatedEventArgs> NavigateToGameBoard;
    }

    public class PlayerCreatedEventArgs : EventArgs {
        public PlayerCreatedEventArgs(Player player) {
            Player = player;
        }

        public Player Player { get; }
    }
}
