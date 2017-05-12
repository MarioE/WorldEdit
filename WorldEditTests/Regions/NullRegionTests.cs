using NUnit.Framework;
using WorldEdit;
using WorldEdit.Regions;

namespace WorldEditTests.Regions
{
    [TestFixture]
    public class NullRegionTests
    {
        [TestCase(-1, -1)]
        [TestCase(50, 73)]
        public void Contains(int x, int y)
        {
            var region = new NullRegion();

            Assert.IsFalse(region.Contains(new Vector(x, y)));
        }

        [TestCase(1, 1)]
        public void Contract(int deltaX, int deltaY)
        {
            var region = new NullRegion();

            Assert.IsInstanceOf<NullRegion>(region.Contract(new Vector(deltaX, deltaY)));
        }

        [Test]
        public void Dimensions()
        {
            var region = new NullRegion();

            Assert.AreEqual(Vector.Zero, region.Dimensions);
        }

        [TestCase(1, 1)]
        public void Expand(int deltaX, int deltaY)
        {
            var region = new NullRegion();

            Assert.IsInstanceOf<NullRegion>(region.Expand(new Vector(deltaX, deltaY)));
        }

        [TestCase(1)]
        public void Inset(int delta)
        {
            var region = new NullRegion();

            Assert.IsInstanceOf<NullRegion>(region.Inset(delta));
        }

        [Test]
        public void LowerBound()
        {
            var region = new NullRegion();

            Assert.AreEqual(Vector.Zero, region.LowerBound);
        }

        [TestCase(1)]
        public void Outset(int delta)
        {
            var region = new NullRegion();

            Assert.IsInstanceOf<NullRegion>(region.Outset(delta));
        }

        [TestCase(1, 1)]
        public void Shift(int deltaX, int deltaY)
        {
            var region = new NullRegion();

            Assert.IsInstanceOf<NullRegion>(region.Shift(new Vector(deltaX, deltaY)));
        }

        [Test]
        public void UpperBound()
        {
            var region = new NullRegion();

            Assert.AreEqual(Vector.Zero, region.UpperBound);
        }
    }
}
