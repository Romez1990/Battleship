using System;

namespace Core.Session {
    public class GameCreatedEventArgs : EventArgs {
        public GameCreatedEventArgs(PlayerConnector playerConnector, Player enemy) {
            PlayerConnector = playerConnector;
            Enemy = enemy;
        }

        public PlayerConnector PlayerConnector { get; }
        public Player Enemy { get; }
    }
}
