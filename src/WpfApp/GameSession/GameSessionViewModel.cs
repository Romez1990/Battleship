using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Core.Field;
using Core.GameSession;
using Core.PlayerData;
using Websocket.Client;
using WpfApp.GameBattlefield;
using WpfApp.Toolkit;

namespace WpfApp.GameSession {
    public class GameSessionViewModel : ViewModel {
        public GameSessionViewModel(WebsocketClient socket, Player player, Player enemy,
            ImmutableArray<Ship> ships) {
            _session = new(socket);
            Player = player;
            Enemy = enemy;
            Ships = ships;
            Score = new();
            PlayerBattlefield = new(ships);
            EnemyBattlefield = new(Enumerable.Empty<Ship>());
            EnemyCanvasClick = new(OnEnemyCanvasClick);
        }

        private readonly Session _session;

        public RelayCommand EnemyCanvasClick { get; }

        public Player Player { get; }
        public Player Enemy { get; }
        public ImmutableArray<Ship> Ships { get; }

        public Score Score { get; }

        public string WhoIsGoing => _session.IsPlayerGoing ? "Ваш ход" : "Противник ходит";

        public Battlefield PlayerBattlefield { get; }
        public Battlefield EnemyBattlefield { get; }

        public int EnemyCanvasPositionX { get; set; }
        public int EnemyCanvasPositionY { get; set; }

        private void OnEnemyCanvasClick() {
            if (!_session.IsPlayerGoing) return;
            PlayerBattlefield.CalculateCoordinates(new(EnemyCanvasPositionX, EnemyCanvasPositionY))
                .IfSome(async coordinates => {
                    if (EnemyBattlefield.CrossAlreadyExists(coordinates)) return;
                    var shotResult = await _session.Go(coordinates);
                    EnemyBattlefield.AddCross(coordinates);
                    if (shotResult.Destroyed) {
                        EnemyBattlefield.AddShip(shotResult.DestroyedShip);
                    }
                    Debug.Print(shotResult.Question.Text);
                });
        }
    }
}
