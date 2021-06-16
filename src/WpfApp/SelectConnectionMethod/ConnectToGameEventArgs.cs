using System;

namespace WpfApp.SelectConnectionMethod {
    public class ConnectToGameEventArgs : EventArgs {
        public ConnectToGameEventArgs(string gameCode) {
            GameCode = gameCode;
        }

        public string GameCode { get; }
    }
}
