using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Core.Field {
    public class RandomShipPlacement {
        private readonly Random _random = new();

        public ImmutableArray<Ship> GetShips() {
            var ships = new List<Ship>();
            AddShips(ships, 4, 1);
            AddShips(ships, 3, 2);
            AddShips(ships, 2, 3);
            AddShips(ships, 1, 4);
            return ships.ToImmutableArray();
        }

        private void AddShips(ICollection<Ship> ships, int size, int count) {
            var possibleLocations = new PossibleLocations(_random, size, ships);
            foreach (var _ in Enumerable.Range(0, count)) {
                var ship = possibleLocations.GetRandomShip();
                ships.Add(ship);
            }
        }
    }
}
