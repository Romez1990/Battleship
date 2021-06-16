using System;
using System.Linq;
using Core.Field;
using NUnit.Framework;

namespace Core.Test.Field {
    public class PossibleLocationsTest {
        [SetUp]
        public void Setup() { }

        private readonly Random _random = new();

        [Test]
        public void GetField() {
            var possibleLocations = new PossibleLocations(_random, 4, Enumerable.Empty<Ship>());
        }
    }
}
