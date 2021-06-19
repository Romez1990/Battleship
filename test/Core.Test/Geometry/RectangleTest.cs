using Core.Geometry;
using NUnit.Framework;

namespace Core.Test.Geometry {
    public class RectangleTest {
        [SetUp]
        public void SetUp() { }

        [Test]
        public void Collides_ReturnsTrue_WhenShipCollides() {
            var rectangle = new Rectangle(new(1, 1), new(2, 2));

            Assert.IsTrue(rectangle.Collides(new Vector(1, 1)));
            Assert.IsTrue(rectangle.Collides(new Vector(1, 2)));
            Assert.IsTrue(rectangle.Collides(new Vector(2, 1)));
            Assert.IsTrue(rectangle.Collides(new Vector(2, 2)));

            Assert.IsFalse(rectangle.Collides(new Vector(0, 0)));
            Assert.IsFalse(rectangle.Collides(new Vector(0, 1)));
            Assert.IsFalse(rectangle.Collides(new Vector(0, 2)));
            Assert.IsFalse(rectangle.Collides(new Vector(0, 3)));
            Assert.IsFalse(rectangle.Collides(new Vector(1, 0)));
            Assert.IsFalse(rectangle.Collides(new Vector(1, 3)));
            Assert.IsFalse(rectangle.Collides(new Vector(2, 0)));
            Assert.IsFalse(rectangle.Collides(new Vector(2, 3)));
            Assert.IsFalse(rectangle.Collides(new Vector(3, 0)));
            Assert.IsFalse(rectangle.Collides(new Vector(3, 1)));
            Assert.IsFalse(rectangle.Collides(new Vector(3, 2)));
            Assert.IsFalse(rectangle.Collides(new Vector(3, 3)));
        }
    }
}
