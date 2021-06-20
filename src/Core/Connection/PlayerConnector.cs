using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks;
using Core.Field;
using Core.PlayerData;
using Core.Serializers;
using WebSocketSharp;

namespace Core.Connection {
    public class PlayerConnector {
        public PlayerConnector(EventHandler<GameCreatedEventArgs> gameCreated) {
            GameCreated += gameCreated;
#if DEBUG
            var host = "127.0.0.1:8000";
            // host = "smart-battleship.herokuapp.com";
#else
            var host = "smart-battleship.herokuapp.com";
#endif
            _socket = new($"ws://{host}/connect");
            _socket.OnClose += (sender, e) =>
                Debug.Print($"Closed: {e.Reason}");
            _socket.OnError += (sender, e) =>
                Debug.Print($"Closed: {e.Message}");
        }

        private readonly IJsonSerializer _serializer = new JsonSerializer();
        private readonly WebSocket _socket;

        private void Connect() {
            _socket.ConnectAsync();
        }

        public async Task<ConnectionCode> CreateGame(Player player, ImmutableArray<Ship> ships) {
            var completionSource = new TaskCompletionSource<ConnectionCode>();
            void Handler(object sender, MessageEventArgs e) {
                var connectionResult = _serializer.DeserializeDynamic(e.Data);
                switch ((string)connectionResult["message_type"]) {
                    case "connection_code":
                        var connectionCode = _serializer.DeserializeObject<ConnectionCode>(connectionResult);
                        completionSource.SetResult(connectionCode);
                        break;
                    case "game_connected":
                        var result = _serializer.DeserializeObject<ConnectionToGameResult>(connectionResult);
                        _socket.OnMessage -= Handler;
                        GameCreated?.Invoke(this, new(_socket, result.Go, result.Enemy));
                        break;
                    default:
                        throw new("Unexpected message type");
                }
            }
            _socket.OnMessage += Handler;
            Connect();
            var message = new CreateGameMessage(new(player, ships, null));
            var text = _serializer.Serialize(message);
            _socket.Send(text);
            return await completionSource.Task;
        }

        private event EventHandler<GameCreatedEventArgs> GameCreated;

        public async Task<ConnectionToGameResult> ConnectToGame(Player player, ImmutableArray<Ship> ships,
            string connectionCode) {
            var completionSource = new TaskCompletionSource<ConnectionToGameResult>();
            void Handler(object sender, MessageEventArgs e) {
                _socket.OnMessage -= Handler;
                var connectionToGameResult = _serializer.Deserialize<ConnectionToGameResult>(e.Data);
                completionSource.SetResult(connectionToGameResult);
                if (!connectionToGameResult.IsConnected) return;
                GameCreated?.Invoke(this, new(_socket, connectionToGameResult.Go, connectionToGameResult.Enemy));
            }
            _socket.OnMessage += Handler;
            Connect();
            var message = new CreateGameMessage(new(player, ships, connectionCode));
            var text = _serializer.Serialize(message);
            _socket.Send(text);
            return await completionSource.Task;
        }
    }
}
