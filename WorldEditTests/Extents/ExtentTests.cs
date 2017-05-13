using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;

namespace WorldEditTests.Extents
{
    [TestFixture]
    public class ExtentTests
    {
        [TestCase(0, 0)]
        public void GetTile_Vector(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.AreEqual(extent.GetTile(x, y), extent.GetTile(new Vector(x, y)));
        }

        [TestCase(20, 10, -1, 1, false)]
        [TestCase(20, 10, 21, 1, false)]
        [TestCase(20, 10, 5, 7, true)]
        public void IsInBounds(int width, int height, int x, int y, bool expected)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};

            Assert.AreEqual(expected, extent.IsInBounds(new Vector(x, y)));
        }

        [TestCase(0, 0)]
        public void SetTile_Vector(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.IsTrue(extent.SetTile(new Vector(x, y), new Tile {Wall = 1}));
            Assert.AreEqual(1, extent.GetTile(x, y).Wall);
        }
    }
}
