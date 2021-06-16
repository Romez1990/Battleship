using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Websocket.Client;

namespace Core.Session {
    public class PlayerConnector {
        public PlayerConnector(EventHandler gameCreated) {
            GameCreated += gameCreated;
            _socket = new(new("ws://127.0.0.1:8000/connect")) {
                ReconnectTimeout = TimeSpan.FromSeconds(30),
            };
            _socket.ReconnectionHappened.Subscribe(info =>
                Debug.Print($"Reconnection happened, type: {info.Type}"));
        }

        private readonly WebsocketClient _socket;

        public async Task<SessionCode> CreateGame() {
            var completionSource = new TaskCompletionSource<SessionCode>();
            _socket.MessageReceived.Subscribe(msg => {
                if (msg.Text == "code") {
                    completionSource.SetResult(new("code"));
                } else if (msg.Text == "found") {
                    Debug.Print($"Message received: {msg}");
                    GameCreated?.Invoke(this, EventArgs.Empty);
                } else {
                    throw new();
                }
            });
            await Connect();
            _socket.Send("{ message: \"123\" }");
            return await completionSource.Task;
        }

        private event EventHandler GameCreated;

        private async Task Connect() {
            await _socket.Start();
        }
    }
}
