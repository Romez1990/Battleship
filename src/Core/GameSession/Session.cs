using System;
using System.Threading.Tasks;
using Core.Geometry;
using Core.Serializers;
using WebSocketSharp;

namespace Core.GameSession {
    public class Session {
        public Session(WebSocket socket, bool isPlayerGoing,
            EventHandler<GetEnemyShotEventArgs> getEnemyShotHandler,
            EventHandler<GetEnemyAnswerEventArgs> getEnemyAnswerHandler, EventHandler playerTurn) {
            _socket = socket;
            IsPlayerGoing = isPlayerGoing;
            OnGetEnemyShot += getEnemyShotHandler;
            OnGetEnemyAnswer += getEnemyAnswerHandler;
            OnPlayerTurn += playerTurn;

            _socket.OnMessage += GetEnemyShot;
        }

        private readonly WebSocket _socket;
        private readonly IJsonSerializer _serializer = new JsonSerializer();

        public bool IsPlayerGoing { get; set; }

        public async Task<ShotResult> Go(Vector coordinates) {
            var completionSource = new TaskCompletionSource<ShotResult>();
            void Handler(object sender, MessageEventArgs e) {
                _socket.OnMessage -= Handler;
                var shotResult = _serializer.Deserialize<ShotResult>(e.Data);
                if (!shotResult.Hit) {
                    EnemyGo();
                }

                completionSource.SetResult(shotResult);
            }
            _socket.OnMessage += Handler;

            var moveMessage = new MoveMessage(coordinates);
            var text = _serializer.Serialize(moveMessage);
            _socket.Send(text);
            return await completionSource.Task;
        }

        public async Task<AnswerResult> Answer(int answerIndex) {
            var completionSource = new TaskCompletionSource<AnswerResult>();
            void Handler(object sender, MessageEventArgs e) {
                _socket.OnMessage -= Handler;
                var result = _serializer.Deserialize<AnswerResult>(e.Data);
                completionSource.SetResult(result);
            }
            _socket.OnMessage += Handler;

            var message = new AnswerMessage(answerIndex);
            var text = _serializer.Serialize(message);
            _socket.Send(text);
            return await completionSource.Task;
        }

        private void EnemyGo() {
            var message = new EnemyGoMessage();
            var text = _serializer.Serialize(message);
            _socket.Send(text);
            IsPlayerGoing = false;
        }

        private void GetEnemyShot(object sender, MessageEventArgs e) {
            if (IsPlayerGoing) return;
            var result = _serializer.DeserializeDynamic(e.Data);
            switch ((string)result["message_type"]) {
                case "get_enemy_shot":
                    var enemyShot = _serializer.DeserializeObject<GetEnemyShot>(result);
                    OnGetEnemyShot?.Invoke(this, new(enemyShot));
                    break;
                case "get_enemy_answer":
                    var enemyAnswer = _serializer.DeserializeObject<GetEnemyAnswer>(result);
                    OnGetEnemyAnswer?.Invoke(this, new(enemyAnswer));
                    break;
                case "player_turn":
                    IsPlayerGoing = true;
                    OnPlayerTurn?.Invoke(this, EventArgs.Empty);
                    break;
                default:
                    throw new("Unexpected message type");
            }
        }

        private event EventHandler<GetEnemyShotEventArgs> OnGetEnemyShot;
        private event EventHandler<GetEnemyAnswerEventArgs> OnGetEnemyAnswer;
        private event EventHandler OnPlayerTurn;
    }
}
