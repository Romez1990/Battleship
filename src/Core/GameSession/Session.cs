using System;
using System.Threading.Tasks;
using Core.Connection;
using Core.Geometry;
using Core.Serializers;
using Websocket.Client;

namespace Core.GameSession {
    public class Session {
        public Session(WebsocketClient socket) {
            _socket = socket;
        }

        private readonly WebsocketClient _socket;
        private readonly IJsonSerializer _serializer = new JsonSerializer();

        public bool IsPlayerGoing { get; set; }

        public async Task<object> Go(Vector coordinates) {
            var completionSource = new TaskCompletionSource<object>();
            IDisposable subscription = null;
            subscription = _socket.MessageReceived.Subscribe(msg => {
                subscription.Dispose();
                var connectionToGameResult = _serializer.Deserialize<ConnectionToGameResult>(msg.Text);
                completionSource.SetResult(connectionToGameResult);
            });

            var moveMessage = new MoveMessage(coordinates);
            var text = _serializer.Serialize(moveMessage);
            _socket.Send(text);
            return await completionSource.Task;
        }
    }
}
