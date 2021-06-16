using Core;
using WpfApp.Toolkit;

namespace WpfApp.CreateGame {
    public class CreateGameViewModel : ViewModel {
        public CreateGameViewModel(Player player) {
            Player = player;
        }

        public Player Player { get; }
    }
}
