using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Field;
using Core.Serializers;
using Websocket.Client;

namespace Core.Session {
    public class PlayerConnector {
        public PlayerConnector(IJsonSerializer serializer, EventHandler<GameCreatedEventArgs> gameCreated) {
            _serializer = serializer;
            GameCreated += gameCreated;
            _socket = new(new("ws://127.0.0.1:8000/connect")) {
                ReconnectTimeout = TimeSpan.FromSeconds(3000),
            };
            _socket.ReconnectionHappened.Subscribe(info =>
                Debug.Print($"Reconnection happened, type: {info.Type}"));
        }

        private readonly IJsonSerializer _serializer;
        private readonly WebsocketClient _socket;

        private async Task Connect() {
            await _socket.Start();
        }

        public async Task<ConnectionCode> CreateGame(Player player, ImmutableArray<Ship> ships) {
            var completionSource = new TaskCompletionSource<ConnectionCode>();
            IDisposable subscription = null;
            subscription = _socket.MessageReceived.Subscribe(msg => {
                var connectionResult = _serializer.DeserializeDynamic(msg.Text);
                switch ((string)connectionResult["message_type"]) {
                    case "connection_code":
                        var connectionCode = _serializer.DeserializeObject<ConnectionCode>(connectionResult);
                        completionSource.SetResult(connectionCode);
                        break;
                    case "game_connected":
                        var enemy = _serializer.DeserializeObject<ConnectionToGameResult>(connectionResult).Enemy;
                        subscription.Dispose();
                        GameCreated?.Invoke(this, new(this, enemy));
                        break;
                    default:
                        throw new("Unexpected message type");
                }
            });
            await Connect();
            var message = new CreateGameMessage(new(player, ships, null));
            var text = _serializer.Serialize(message);
            _socket.Send(text);
            return await completionSource.Task;
        }

        private event EventHandler<GameCreatedEventArgs> GameCreated;

        public async Task<ConnectionToGameResult> ConnectToGame(Player player, ImmutableArray<Ship> ships,
            string connectionCode) {
            var completionSource = new TaskCompletionSource<ConnectionToGameResult>();
            IDisposable subscription = null;
            subscription = _socket.MessageReceived.Subscribe(msg => {
                var connectionToGameResult = _serializer.Deserialize<ConnectionToGameResult>(msg.Text);
                subscription.Dispose();
                completionSource.SetResult(connectionToGameResult);
            });
            await Connect();
            var message = new CreateGameMessage(new(player, ships, connectionCode));
            var text = _serializer.Serialize(message);
            _socket.Send(text);
            return await completionSource.Task;
        }
    }
}
