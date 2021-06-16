using System;
using System.ComponentModel.DataAnnotations;
using Core;
using WpfApp.Toolkit;

namespace WpfApp.GameStart {
    public class GameStartViewModel : Form {
        public GameStartViewModel(EventHandler<PlayerCreatedEventArgs> navigateToGameBoard) {
            NavigateToGameBoard += navigateToGameBoard;
            Validate = new(ValidateAllProperties);
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

        public RelayCommand Validate { get; }

        protected override void OnSubmit() {
            var player = new Player(FirstName, LastName);
            NavigateToGameBoard?.Invoke(this, new(player));
        }

        private event EventHandler<PlayerCreatedEventArgs> NavigateToGameBoard;
    }
}
