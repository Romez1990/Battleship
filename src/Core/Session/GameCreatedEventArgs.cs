using System;

namespace Core.Session {
    public class GameCreatedEventArgs : EventArgs {
        public GameCreatedEventArgs(Player enemy) {
            Enemy = enemy;
        }

        public Player Enemy { get; }
    }
}
