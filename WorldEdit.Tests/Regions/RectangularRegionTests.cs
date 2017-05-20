using NUnit.Framework;
using WorldEdit.Core;
using WorldEdit.Core.Regions;

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

            Assert.AreEqual(expected, region.Contains(new Vector(testX, testY)));
        }

        [TestCase(5, 4, 8, 10, 2, -2, 5, 6, 6, 10)]
        [TestCase(8, 4, 5, 10, 2, -2, 6, 6, 5, 10)]
        [TestCase(8, 10, 5, 4, 2, -2, 6, 10, 5, 6)]
        public void Contract(int x, int y, int x2, int y2, int deltaX, int deltaY, int expectedX, int expectedY,
            int expectedX2, int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Contract(new Vector(deltaX, deltaY));

            Assert.AreEqual(new Vector(expectedX, expectedY), region2.Position1);
            Assert.AreEqual(new Vector(expectedX2, expectedY2), region2.Position2);
        }

        [TestCase(5, 4, 8, 10, 2, -2, 5, 2, 10, 10)]
        [TestCase(8, 4, 5, 10, 2, -2, 10, 2, 5, 10)]
        [TestCase(8, 10, 5, 4, 2, -2, 10, 10, 5, 2)]
        public void Expand(int x, int y, int x2, int y2, int deltaX, int deltaY, int expectedX, int expectedY,
            int expectedX2, int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Expand(new Vector(deltaX, deltaY));

            Assert.AreEqual(new Vector(expectedX, expectedY), region2.Position1);
            Assert.AreEqual(new Vector(expectedX2, expectedY2), region2.Position2);
        }

        [Test]
        public void GetCanContract()
        {
            var region = new RectangularRegion(Vector.Zero, Vector.Zero);

            Assert.IsTrue(region.CanContract);
        }

        [Test]
        public void GetCanExpand()
        {
            var region = new RectangularRegion(Vector.Zero, Vector.Zero);

            Assert.IsTrue(region.CanExpand);
        }

        [Test]
        public void GetCanShift()
        {
            var region = new RectangularRegion(Vector.Zero, Vector.Zero);

            Assert.IsTrue(region.CanShift);
        }

        [TestCase(5, 4, 8, 10, 1, 6, 5, 7, 9)]
        [TestCase(8, 4, 5, 10, 1, 7, 5, 6, 9)]
        [TestCase(8, 10, 5, 4, 1, 7, 9, 6, 5)]
        public void Inset(int x, int y, int x2, int y2, int delta, int expectedX, int expectedY, int expectedX2,
            int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Inset(delta);

            Assert.AreEqual(new Vector(expectedX, expectedY), region2.Position1);
            Assert.AreEqual(new Vector(expectedX2, expectedY2), region2.Position2);
        }

        [TestCase(0, 0, 10, 10, 0, 0)]
        [TestCase(4, 10, 6, 11, 4, 10)]
        public void LowerBound(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            Assert.AreEqual(new Vector(expectedX, expectedY), region.LowerBound);
        }

        [TestCase(5, 4, 8, 10, 1, 4, 3, 9, 11)]
        [TestCase(8, 4, 5, 10, 1, 9, 3, 4, 11)]
        [TestCase(8, 10, 5, 4, 1, 9, 11, 4, 3)]
        public void Outset(int x, int y, int x2, int y2, int delta, int expectedX, int expectedY, int expectedX2,
            int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Outset(delta);

            Assert.AreEqual(new Vector(expectedX, expectedY), region2.Position1);
            Assert.AreEqual(new Vector(expectedX2, expectedY2), region2.Position2);
        }

        [TestCase(5, 4, 8, 10, 2, -2, 7, 2, 10, 8)]
        public void Shift(int x, int y, int x2, int y2, int deltaX, int deltaY, int expectedX, int expectedY,
            int expectedX2, int expectedY2)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            var region2 = (RectangularRegion)region.Shift(new Vector(deltaX, deltaY));

            Assert.AreEqual(new Vector(expectedX, expectedY), region2.Position1);
            Assert.AreEqual(new Vector(expectedX2, expectedY2), region2.Position2);
        }

        [TestCase(0, 0, 10, 10, 11, 11)]
        [TestCase(4, 10, 6, 11, 7, 12)]
        public void UpperBound(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            Assert.AreEqual(new Vector(expectedX, expectedY), region.UpperBound);
        }
    }
}
