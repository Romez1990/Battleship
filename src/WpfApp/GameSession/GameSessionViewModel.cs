using System.Collections.Immutable;
using System.Linq;
using Core;
using Core.Connection;
using Core.Field;
using Core.PlayerData;
using WpfApp.GameBattlefield;
using WpfApp.Toolkit;

namespace WpfApp.GameSession {
    public class GameSessionViewModel : ViewModel {
        public GameSessionViewModel(PlayerConnector playerConnector, Player player, Player enemy,
            ImmutableArray<Ship> ships) {
            _playerConnector = playerConnector;
            Player = player;
            Enemy = enemy;
            Ships = ships;
            Score = new();
            PlayerBattlefield = new(ships);
            EnemyBattlefield = new(Enumerable.Empty<Ship>());
            EnemyCanvasClick = new(OnEnemyCanvasClick);
        }

        private readonly PlayerConnector _playerConnector;

        public RelayCommand EnemyCanvasClick { get; }

        public Player Player { get; }
        public Player Enemy { get; }
        public ImmutableArray<Ship> Ships { get; }

        public Score Score { get; }

        public string WhoIsGoing => _playerConnector.IsPlayerGoing ? "Ваш ход" : "Противник ходит";

        public Battlefield PlayerBattlefield { get; }
        public Battlefield EnemyBattlefield { get; }

        public int EnemyCanvasPositionX { get; set; }
        public int EnemyCanvasPositionY { get; set; }

        private void OnEnemyCanvasClick() {
            PlayerBattlefield.CalculateCoordinates(new(EnemyCanvasPositionX, EnemyCanvasPositionY))
                .IfSome(coordinates => {
                    if (_playerConnector.IsPlayerGoing) {
                        _playerConnector.Go(coordinates);
                        EnemyBattlefield.AddCross(coordinates);
                    }
                });
        }
    }
}
