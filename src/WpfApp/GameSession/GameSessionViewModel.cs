using System.Collections.Immutable;
using System.Linq;
using Core;
using Core.Field;
using WpfApp.GameBattlefield;
using WpfApp.Toolkit;

namespace WpfApp.GameSession {
    public class GameSessionViewModel : ViewModel {
        public GameSessionViewModel(Player player, Player enemy, ImmutableArray<Ship> ships) {
            Player = player;
            Enemy = enemy;
            Ships = ships;
            Score = new();
            PlayerBattlefield = new(ships);
            EnemyBattlefield = new(Enumerable.Empty<Ship>());
        }

        public Player Player { get; }
        public Player Enemy { get; }
        public ImmutableArray<Ship> Ships { get; }

        public Score Score { get; }

        public Battlefield PlayerBattlefield { get; }
        public Battlefield EnemyBattlefield { get; }
    }
}
