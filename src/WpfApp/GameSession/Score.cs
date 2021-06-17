using WpfApp.Toolkit;

namespace WpfApp.GameSession {
    public class Score : Bindable {
        private int _player = 0;
        private int _enemy = 0;

        public int Player {
            get => _player;
            set => SetProperty(ref _player, value);
        }

        public int Enemy {
            get => _enemy;
            set => SetProperty(ref _enemy, value);
        }

        public override string ToString() =>
            $"{Player}:{Enemy}";
    }
}
