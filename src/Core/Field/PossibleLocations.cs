using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using Core.Geometry;

namespace Core.Field {
    public class PossibleLocations {
        public PossibleLocations(Random random, int shipSize, IEnumerable<Ship> otherShips) {
            _random = random;
            _shipSize = shipSize;
            _possibleHorizontalLocations = CreatePossibleHorizontalLocations();
            _possibleVerticalLocations = CreatePossibleVerticalLocations();
            RemoveShipLocations(otherShips);
        }

        private readonly Random _random;

        private readonly int _shipSize;

        private readonly List<Vector> _possibleHorizontalLocations;
        private readonly List<Vector> _possibleVerticalLocations;

        private List<Vector> CreatePossibleHorizontalLocations() =>
            CreatePossibleLocations(Field.Size - new Vector(_shipSize - 1, 0));

        private List<Vector> CreatePossibleVerticalLocations() =>
            CreatePossibleLocations(Field.Size - new Vector(0, _shipSize - 1));

        private List<Vector> CreatePossibleLocations(Vector size) {
            var row = Enumerable.Range(0, size.X);
            return Enumerable.Range(0, size.Y)
                .Map(y => row.Map(x => new Vector(x, y)))
                .Flatten()
                .ToList();
        }

        private void RemoveShipLocations(IEnumerable<Ship> ships) {
            var shipBorders = ships.Map(ship => new ShipBorder(ship)).ToList();
            RemoveShipLocationsFromPossibleLocations(_possibleHorizontalLocations, shipBorders);
            RemoveShipLocationsFromPossibleLocations(_possibleVerticalLocations, shipBorders);
        }

        private void RemoveShipLocationsFromPossibleLocations(List<Vector> possibleLocations,
            List<ShipBorder> shipBorders) {
            possibleLocations.RemoveAll(coordinates =>
                shipBorders.Any(shipBorder => shipBorder.Collides(coordinates)));
        }

        public Ship GetRandomShip() {
            var orientation = GetRandomOrientation();
            var possibleLocations = GetPossibleLocations(orientation);
            var coordinates = GetRandomCoordinates(possibleLocations);
            var ship = new Ship(coordinates, _shipSize, orientation);
            RemoveShipLocations(new[] {ship});
            return ship;
        }

        private readonly ImmutableArray<Orientation> _orientations = typeof(Orientation).GetEnumValues()
            .Cast<Orientation>().ToImmutableArray();

        private Orientation GetRandomOrientation() {
            var index = _random.Next(_orientations.Length);
            return _orientations[index];
        }

        private Vector GetRandomCoordinates(IReadOnlyList<Vector> possibleLocations) {
            var index = _random.Next(possibleLocations.Count);
            return possibleLocations[index];
        }

        private List<Vector> GetPossibleLocations(Orientation orientation) =>
            orientation switch {
                Orientation.Horizontal => _possibleHorizontalLocations,
                Orientation.Vertical => _possibleVerticalLocations,
                _ => throw new InvalidEnumArgumentException(nameof(orientation), (int)orientation,
                    orientation.GetType()),
            };
    }
}
