using System.Collections.Immutable;
using System.Linq;
using Core;
using Core.Field;
using WpfApp.Toolkit;

namespace WpfApp.GameBattlefield {
    public class BattlefieldViewModel : ViewModel {
        public BattlefieldViewModel(Player player, Player enemy, ImmutableArray<Ship> ships) {
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
