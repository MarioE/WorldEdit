using NUnit.Framework;
using WorldEdit.Regions;

namespace WorldEdit.Tests.Regions
{
    [TestFixture]
    public class RectangularRegionTests
    {
        [TestCase(0, 0, 20, 20, -1, 0, false)]
        [TestCase(0, 0, 20, 20, 21, 0, false)]
        [TestCase(0, 0, 20, 20, 0, -1, false)]
        [TestCase(0, 0, 20, 20, 0, 21, false)]
        [TestCase(0, 0, 20, 20, 5, 6, true)]
        public void Contains(int x, int y, int x2, int y2, int testX, int testY, bool expected)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            Assert.That(region.Contains(new Vector(testX, testY)), Is.EqualTo(expected));
        }

        [TestCase(5, 4, 8, 10, 2, -2, 5, 6, 6, 10)]
        [TestCase(8, 4, 5, 10, 2, -2, 6, 6, 5, 10)]
        [TestCase(8, 10, 5, 4, 2, -2, 6, 10, 5, 6)]
        public void Contract(int x, int y, int x2, int y2, int deltaX, int deltaY, int expectedX, int expectedY,
            int expectedX2, int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Contract(new Vector(deltaX, deltaY));

            Assert.That(region2.Position1, Is.EqualTo(new Vector(expectedX, expectedY)));
            Assert.That(region2.Position2, Is.EqualTo(new Vector(expectedX2, expectedY2)));
        }

        [TestCase(5, 4, 8, 10, 2, -2, 5, 2, 10, 10)]
        [TestCase(8, 4, 5, 10, 2, -2, 10, 2, 5, 10)]
        [TestCase(8, 10, 5, 4, 2, -2, 10, 10, 5, 2)]
        public void Expand(int x, int y, int x2, int y2, int deltaX, int deltaY, int expectedX, int expectedY,
            int expectedX2, int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Expand(new Vector(deltaX, deltaY));

            Assert.That(region2.Position1, Is.EqualTo(new Vector(expectedX, expectedY)));
            Assert.That(region2.Position2, Is.EqualTo(new Vector(expectedX2, expectedY2)));
        }

        [TestCase(5, 4, 8, 10, 1, 6, 5, 7, 9)]
        [TestCase(8, 4, 5, 10, 1, 7, 5, 6, 9)]
        [TestCase(8, 10, 5, 4, 1, 7, 9, 6, 5)]
        public void Inset(int x, int y, int x2, int y2, int delta, int expectedX, int expectedY, int expectedX2,
            int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Inset(delta);

            Assert.That(region2.Position1, Is.EqualTo(new Vector(expectedX, expectedY)));
            Assert.That(region2.Position2, Is.EqualTo(new Vector(expectedX2, expectedY2)));
        }

        [TestCase(0, 0, 10, 10, 0, 0)]
        [TestCase(4, 10, 6, 11, 4, 10)]
        public void LowerBound(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            Assert.That(region.LowerBound, Is.EqualTo(new Vector(expectedX, expectedY)));
        }

        [TestCase(5, 4, 8, 10, 1, 4, 3, 9, 11)]
        [TestCase(8, 4, 5, 10, 1, 9, 3, 4, 11)]
        [TestCase(8, 10, 5, 4, 1, 9, 11, 4, 3)]
        public void Outset(int x, int y, int x2, int y2, int delta, int expectedX, int expectedY, int expectedX2,
            int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Outset(delta);

            Assert.That(region2.Position1, Is.EqualTo(new Vector(expectedX, expectedY)));
            Assert.That(region2.Position2, Is.EqualTo(new Vector(expectedX2, expectedY2)));
        }

        [TestCase(3, 4)]
        public void Position1(int x, int y)
        {
            var region = new RectangularRegion(new Vector(x, y), Vector.Zero);

            Assert.That(region.Position1, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(3, 4)]
        public void Position2(int x, int y)
        {
            var region = new RectangularRegion(Vector.Zero, new Vector(x, y));

            Assert.That(region.Position2, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(5, 4, 8, 10, 2, -2, 7, 2, 10, 8)]
        public void Shift(int x, int y, int x2, int y2, int deltaX, int deltaY, int expectedX, int expectedY,
            int expectedX2, int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Shift(new Vector(deltaX, deltaY));

            Assert.That(region2.Position1, Is.EqualTo(new Vector(expectedX, expectedY)));
            Assert.That(region2.Position2, Is.EqualTo(new Vector(expectedX2, expectedY2)));
        }

        [TestCase(0, 0, 10, 10, 11, 11)]
        [TestCase(4, 10, 6, 11, 7, 12)]
        public void UpperBound(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            Assert.That(region.UpperBound, Is.EqualTo(new Vector(expectedX, expectedY)));
        }
    }
}
