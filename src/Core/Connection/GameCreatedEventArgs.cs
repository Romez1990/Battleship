using System;
using Core.PlayerData;
using Websocket.Client;

namespace Core.Connection {
    public class GameCreatedEventArgs : EventArgs {
        public GameCreatedEventArgs(WebsocketClient socket, bool? isPlayerGoing, Player enemy) {
            Socket = socket;
            IsPlayerGoing = isPlayerGoing;
            Enemy = enemy;
        }

        public WebsocketClient Socket { get; }
        public bool? IsPlayerGoing { get; }
        public Player Enemy { get; }
    }
}
