using System;
using System.Threading.Tasks;
using Core.Geometry;
using Core.Serializers;
using Websocket.Client;

namespace Core.GameSession {
    public class Session {
        public Session(WebsocketClient socket, bool isPlayerGoing, EventHandler<GetEnemyShotEventArgs> getEnemyShotHandler, EventHandler<GetEnemyAnswerEventArgs> getEnemyAnswerHandler, EventHandler playerTurn) {
            _socket = socket;
            IsPlayerGoing = isPlayerGoing;
            OnGetEnemyShot += getEnemyShotHandler;
            OnGetEnemyAnswer += getEnemyAnswerHandler;
            OnPlayerTurn += playerTurn;

            _socket.MessageReceived.Subscribe(GetEnemyShot);
        }

        private readonly WebsocketClient _socket;
        private readonly IJsonSerializer _serializer = new JsonSerializer();

        public bool IsPlayerGoing { get; set; }

        public async Task<ShotResult> Go(Vector coordinates) {
            var completionSource = new TaskCompletionSource<ShotResult>();
            IDisposable subscription = null;
            subscription = _socket.MessageReceived.Subscribe(msg => {
                subscription.Dispose();
                var shotResult = _serializer.Deserialize<ShotResult>(msg.Text);
                if (!shotResult.Hit) {
                    EnemyGo();
                }

                completionSource.SetResult(shotResult);
            });

            var moveMessage = new MoveMessage(coordinates);
            var text = _serializer.Serialize(moveMessage);
            _socket.Send(text);
            return await completionSource.Task;
        }

        public async Task<AnswerResult> Answer(int answerIndex) {
            var completionSource = new TaskCompletionSource<AnswerResult>();
            IDisposable subscription = null;
            subscription = _socket.MessageReceived.Subscribe(msg => {
                subscription.Dispose();
                var result = _serializer.Deserialize<AnswerResult>(msg.Text);
                EnemyGo();
                completionSource.SetResult(result);
            });
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

        private void GetEnemyShot(ResponseMessage msg) {
            if (IsPlayerGoing) return;
            var result = _serializer.DeserializeDynamic(msg.Text);
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
