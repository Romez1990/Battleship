using System;
using System.Diagnostics;
using Websocket.Client;

namespace Core {
    public class ConnectTest {
        public async void Connect() {
            var socket = new WebsocketClient(new("ws://127.0.0.1:8000/test")) {
                ReconnectTimeout = TimeSpan.FromSeconds(30),
            };
            socket.ReconnectionHappened.Subscribe(info =>
                Debug.Print($"Reconnection happened, type: {info.Type}"));

            socket.MessageReceived.Subscribe(msg => Debug.Print($"Message received: {msg}"));
            await socket.Start();

            socket.Send("{ message: \"123\" }");
        }
    }
}
