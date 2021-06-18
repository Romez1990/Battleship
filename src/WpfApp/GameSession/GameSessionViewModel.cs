using System.Collections.Immutable;
using System.Linq;
using Core;
using Core.Field;
using Core.Session;
using WpfApp.GameBattlefield;
using WpfApp.Toolkit;

namespace WpfApp.GameSession {
    public class GameSessionViewModel : ViewModel {
        public GameSessionViewModel(PlayerConnector playerConnector, Player player, Player enemy, ImmutableArray<Ship> ships) {
            _playerConnector = playerConnector;
            Player = player;
            Enemy = enemy;
            Ships = ships;
            Score = new();
            PlayerBattlefield = new(ships);
            EnemyBattlefield = new(Enumerable.Empty<Ship>());
        }

        private readonly PlayerConnector _playerConnector;

        public Player Player { get; }
        public Player Enemy { get; }
        public ImmutableArray<Ship> Ships { get; }

        public Score Score { get; }

        public Battlefield PlayerBattlefield { get; }
        public Battlefield EnemyBattlefield { get; }
    }
}
