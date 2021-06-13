using System;
using Core;

namespace WpfApp.GameStart {
    public class PlayerCreatedEventArgs : EventArgs {
        public PlayerCreatedEventArgs(Player player) {
            Player = player;
        }

        public Player Player { get; }
    }
}
