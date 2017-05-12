using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Masks;

namespace WorldEditTests.Masks
{
    [TestFixture]
    public class NegatedMaskTests
    {
        [Test]
        public void Ctor_NullMask_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new NegatedMask(null));
        }

        [TestCase(10, 10, 4, 5)]
        public void Test_False(int extentWidth, int extentHeight, int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[extentWidth, extentHeight]};
            var mask = new NegatedMask(new NullMask());

            Assert.IsFalse(mask.Test(extent, new Vector(x, y)));
        }

        [TestCase(10, 10, 4, 5)]
        public void Test_True(int extentWidth, int extentHeight, int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[extentWidth, extentHeight]};
            var mask = new NegatedMask(new NegatedMask(new NullMask()));

            Assert.IsTrue(mask.Test(extent, new Vector(x, y)));
        }
    }
}
