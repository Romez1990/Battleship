using Core.Field;
using NUnit.Framework;

namespace Core.Test.Field {
    public class RandomShipPlacementTest {
        [SetUp]
        public void Setup() {
            _randomShipPlacement = new();
        }

        private RandomShipPlacement _randomShipPlacement;

        [Test]
        public void GetField() {
            // var field = _randomShipPlacement.GetField();
        }
    }
}
