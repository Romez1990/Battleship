using System;
using System.Collections.Immutable;
using Core.Field;

namespace WpfApp.PlacementOfShips {
    public class ShipsPlacedEventArgs : EventArgs {
        public ShipsPlacedEventArgs(ImmutableArray<Ship> ships) {
            Ships = ships;
        }

        public ImmutableArray<Ship> Ships { get; }
    }
}
