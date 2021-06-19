using System;
using Core.PlayerData;
using Websocket.Client;

namespace Core.Connection {
    public class GameCreatedEventArgs : EventArgs {
        public GameCreatedEventArgs(WebsocketClient socket, bool? isPlayerGoing, Player enemy) {
            Socket = socket;
            _isPlayerGoing = isPlayerGoing;
            Enemy = enemy;
        }

        public WebsocketClient Socket { get; }

        private readonly bool? _isPlayerGoing;
        public bool IsPlayerGoing => _isPlayerGoing ?? throw new("isPlayerGoing is null");

        public Player Enemy { get; }
    }
}
