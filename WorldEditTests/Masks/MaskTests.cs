using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;

namespace WorldEditTests.Masks
{
    [TestFixture]
    public class MaskTests
    {
        [Test]
        public void Test_NullExtent_ThrowsArgumentNullException()
        {
            var mask = new MockMask();

            Assert.Throws<ArgumentNullException>(() => mask.Test(null, Vector.Zero));
        }

        [TestCase(10, 10, -1, -1)]
        [TestCase(10, 10, 21, 11)]
        public void Test_OutOfBounds_False(int extentWidth, int extentHeight, int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var mask = new MockMask();

            Assert.IsFalse(mask.Test(extent, new Vector(x, y)));
        }
    }
}
