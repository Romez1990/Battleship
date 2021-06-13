using System;
using System.Collections.Immutable;
using Core.Field;

namespace WpfApp.GameBoard {
    public class ShipsCreatedEventArgs : EventArgs {
        public ShipsCreatedEventArgs(ImmutableArray<Ship> ships) {
            Ships = ships;
        }

        public ImmutableArray<Ship> Ships { get; }
    }
}
