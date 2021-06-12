using Core.Field;
using Core.Geometry;
using NUnit.Framework;

namespace Core.Test.Field {
    public class ShipBorderTest {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Collides_ReturnsTrue_WhenShipCollides() {
            var ship1 = new Ship(new(1, 1), 4, Orientation.Horizontal);
            var ship2 = new Ship(new(2, 0), 2, Orientation.Vertical);
            var shipBorder = new ShipBorder(ship1);

            var result = shipBorder.Collides(ship2);

            Assert.IsTrue(result);
        }
    }
}
