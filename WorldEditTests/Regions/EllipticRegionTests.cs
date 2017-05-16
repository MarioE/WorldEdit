using NUnit.Framework;
using WorldEdit;
using WorldEdit.Regions;

namespace WorldEditTests.Regions
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

            Assert.AreEqual(expected, region.Contains(new Vector(testX, testY)));
        }

        [TestCase(0, 0, 4, 6, -2, -2, 2, 4)]
        [TestCase(0, 0, 4, 6, 2, 2, 2, 4)]
        [TestCase(0, 0, 4, 6, 2, 0, 2, 6)]
        public void Contract(int x, int y, int radiusX, int radiusY, int deltaX, int deltaY, int expectedRadiusX,
            int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Contract(new Vector(deltaX, deltaY));

            Assert.AreEqual(new Vector(expectedRadiusX, expectedRadiusY), region2.Radius);
        }

        [TestCase(0, 0, 4, 6, -2, -2, 6, 8)]
        [TestCase(0, 0, 4, 6, 2, 2, 6, 8)]
        [TestCase(0, 0, 4, 6, 2, 0, 6, 6)]
        public void Expand(int x, int y, int radiusX, int radiusY, int deltaX, int deltaY, int expectedRadiusX,
            int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Expand(new Vector(deltaX, deltaY));

            Assert.AreEqual(new Vector(expectedRadiusX, expectedRadiusY), region2.Radius);
        }

        [TestCase(0, 0, 4, 6, 1, 3, 5)]
        [TestCase(0, 0, 4, 6, 2, 2, 4)]
        public void Inset(int x, int y, int radiusX, int radiusY, int delta, int expectedRadiusX, int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Inset(delta);

            Assert.AreEqual(new Vector(expectedRadiusX, expectedRadiusY), region2.Radius);
        }

        [TestCase(0, 0, 4, 6, 1, 5, 7)]
        [TestCase(0, 0, 4, 6, 2, 6, 8)]
        public void Outset(int x, int y, int radiusX, int radiusY, int delta, int expectedRadiusX, int expectedRadiusY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Outset(delta);

            Assert.AreEqual(new Vector(expectedRadiusX, expectedRadiusY), region2.Radius);
        }

        [TestCase(0, 0, 4, 6, -2, -2, -2, -2)]
        [TestCase(0, 0, 4, 6, 2, 2, 2, 2)]
        [TestCase(0, 0, 4, 6, 2, 0, 2, 0)]
        public void Shift(int x, int y, int radiusX, int radiusY, int deltaX, int deltaY, int expectedX, int expectedY)
        {
            var region = new EllipticRegion(new Vector(x, y), new Vector(radiusX, radiusY));

            var region2 = (EllipticRegion)region.Shift(new Vector(deltaX, deltaY));

            Assert.AreEqual(new Vector(expectedX, expectedY), region2.Center);
        }
    }
}
