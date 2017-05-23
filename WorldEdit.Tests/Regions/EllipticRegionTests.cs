using NUnit.Framework;
using WorldEdit.Regions;

namespace WorldEdit.Tests.Regions
{
    [TestFixture]
    public class EllipticRegionTests
    {
        [TestCase(0, 0, 5, 7, 0, -8, false)]
        [TestCase(0, 0, 5, 7, 0, -7, true)]
        [TestCase(0, 0, 5, 7, 0, 7, true)]
        [TestCase(0, 0, 5, 7, 0, 8, false)]
        [TestCase(0, 0, 5, 7, 4, 3, true)]
        [TestCase(0, 0, 5, 7, 5, 0, true)]
        public void Contains(int x, int y, int radiusX, int radiusY, int testX, int testY, bool expected)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            Assert.That(region.Contains(new Vector(testX, testY)), Is.EqualTo(expected));
        }

        [TestCase(0, 0, 4, 6, -2, -2, 2, 4)]
        [TestCase(0, 0, 4, 6, 2, 2, 2, 4)]
        [TestCase(0, 0, 4, 6, 2, 0, 2, 6)]
        public void Contract(int x, int y, int radiusX, int radiusY, int deltaX, int deltaY, int expectedRadiusX,
            int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Contract(new Vector(deltaX, deltaY));

            Assert.That(region2.Radius, Is.EqualTo(new Vector(expectedRadiusX, expectedRadiusY)));
        }

        [TestCase(0, 0, 4, 6, -2, -2, 6, 8)]
        [TestCase(0, 0, 4, 6, 2, 2, 6, 8)]
        [TestCase(0, 0, 4, 6, 2, 0, 6, 6)]
        public void Expand(int x, int y, int radiusX, int radiusY, int deltaX, int deltaY, int expectedRadiusX,
            int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Expand(new Vector(deltaX, deltaY));

            Assert.That(region2.Radius, Is.EqualTo(new Vector(expectedRadiusX, expectedRadiusY)));
        }

        [TestCase(3, 5)]
        public void GetCenter(int x, int y)
        {
            var region = new EllipticRegion(new Vector(x, y), Vector.Zero);

            Assert.That(region.Center, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(2, 3)]
        [TestCase(2, -3)]
        [TestCase(-2, 3)]
        public void GetRadius(int radiusX, int radiusY)
        {
            var region = new EllipticRegion(Vector.Zero, new Vector(radiusX, radiusY));

            Assert.That(region.Radius, Is.EqualTo(Vector.Abs(new Vector(radiusX, radiusY))));
        }

        [TestCase(0, 0, 4, 6, 1, 3, 5)]
        [TestCase(0, 0, 4, 6, 2, 2, 4)]
        public void Inset(int x, int y, int radiusX, int radiusY, int delta, int expectedRadiusX, int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Inset(delta);

            Assert.That(region2.Radius, Is.EqualTo(new Vector(expectedRadiusX, expectedRadiusY)));
        }

        [TestCase(0, 0, 10, 10, -10, -10)]
        [TestCase(4, 10, 6, 11, -2, -1)]
        public void LowerBound(int x, int y, int radiusX, int radiusY, int expectedX, int expectedY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            Assert.That(region.LowerBound, Is.EqualTo(new Vector(expectedX, expectedY)));
        }

        [TestCase(0, 0, 4, 6, 1, 5, 7)]
        [TestCase(0, 0, 4, 6, 2, 6, 8)]
        public void Outset(int x, int y, int radiusX, int radiusY, int delta, int expectedRadiusX, int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Outset(delta);

            Assert.That(region2.Radius, Is.EqualTo(new Vector(expectedRadiusX, expectedRadiusY)));
        }

        [TestCase(0, 0, 4, 6, -2, -2, -2, -2)]
        [TestCase(0, 0, 4, 6, 2, 2, 2, 2)]
        [TestCase(0, 0, 4, 6, 2, 0, 2, 0)]
        public void Shift(int x, int y, int radiusX, int radiusY, int deltaX, int deltaY, int expectedX, int expectedY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Shift(new Vector(deltaX, deltaY));

            Assert.That(region2.Center, Is.EqualTo(new Vector(expectedX, expectedY)));
        }

        [TestCase(0, 0, 10, 10, 11, 11)]
        [TestCase(4, 10, 6, 11, 11, 22)]
        public void UpperBound(int x, int y, int radiusX, int radiusY, int expectedX, int expectedY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            Assert.That(region.UpperBound, Is.EqualTo(new Vector(expectedX, expectedY)));
        }
    }
}
