using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Masks;

namespace WorldEditTests.Masks
{
    [TestFixture]
    public class NullMaskTests
    {
        [TestCase(10, 10, 4, 5, true)]
        public void Test(int extentWidth, int extentHeight, int x, int y, bool expected)
        {
            var extent = new MockExtent {Tiles = new ITile[extentWidth, extentHeight]};
            var mask = new NullMask();

            Assert.AreEqual(expected, mask.Test(extent, new Vector(x, y)));
        }
    }
}
