using System;
using NUnit.Framework;
using WorldEdit.Core;
using WorldEdit.Core.Regions;

namespace WorldEdit.Tests.Regions
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
        public void Contract_ThrowsInvalidOperationException(int deltaX, int deltaY)
        {
            var region = new NullRegion();

            Assert.Throws<InvalidOperationException>(() => region.Contract(new Vector(deltaX, deltaY)));
        }

        [TestCase(1, 1)]
        public void Expand_ThrowsInvalidOperationException(int deltaX, int deltaY)
        {
            var region = new NullRegion();

            Assert.Throws<InvalidOperationException>(() => region.Expand(new Vector(deltaX, deltaY)));
        }

        [Test]
        public void GetCanContract()
        {
            var region = new NullRegion();

            Assert.IsFalse(region.CanContract);
        }

        [Test]
        public void GetCanExpand()
        {
            var region = new NullRegion();

            Assert.IsFalse(region.CanExpand);
        }

        [Test]
        public void GetCanShift()
        {
            var region = new NullRegion();

            Assert.IsFalse(region.CanShift);
        }

        [TestCase(1)]
        public void Inset_ThrowsInvalidOperationException(int delta)
        {
            var region = new NullRegion();

            Assert.Throws<InvalidOperationException>(() => region.Inset(delta));
        }

        [Test]
        public void LowerBound()
        {
            var region = new NullRegion();

            Assert.AreEqual(Vector.Zero, region.LowerBound);
        }

        [TestCase(1)]
        public void Outset_ThrowsInvalidOperationException(int delta)
        {
            var region = new NullRegion();

            Assert.Throws<InvalidOperationException>(() => region.Outset(delta));
        }

        [TestCase(1, 1)]
        public void Shift_ThrowsInvalidOperationException(int deltaX, int deltaY)
        {
            var region = new NullRegion();

            Assert.Throws<InvalidOperationException>(() => region.Shift(new Vector(deltaX, deltaY)));
        }

        [Test]
        public void UpperBound()
        {
            var region = new NullRegion();

            Assert.AreEqual(Vector.Zero, region.UpperBound);
        }
    }
}
