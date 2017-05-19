using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Extents;
using TTile = Terraria.Tile;

namespace WorldEditTests.Extents
{
    [TestFixture]
    public class LimitedExtentTests
    {
        [Test]
        public void Ctor_NullExtent_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LimitedExtent(null, -1));
        }

        [TestCase(20, 10)]
        public void Dimensions(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var limitedExtent = new LimitedExtent(extent, -1);

            Assert.AreEqual(extent.Dimensions, limitedExtent.Dimensions);
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var limitedExtent = new LimitedExtent(new MockExtent {Tiles = tiles}, -1);

            Assert.AreEqual(1, limitedExtent.GetTile(x, y).Type);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt_LimitNotReached(int x, int y)
        {
            var limitedExtent = new LimitedExtent(new MockExtent {Tiles = new ITile[20, 10]}, -1);

            Assert.IsTrue(limitedExtent.SetTile(x, y, new Tile {Wall = 1}));
            Assert.AreEqual(1, limitedExtent.GetTile(x, y).Wall);
        }

        [TestCase(1, 0, 0)]
        public void SetTileIntInt_LimitObeyed(int limit, int x, int y)
        {
            var limitedExtent = new LimitedExtent(new MockExtent {Tiles = new ITile[20, 10]}, 1);
            for (var i = 0; i < limit; ++i)
            {
                limitedExtent.SetTile(0, 0, new Tile());
            }

            Assert.IsFalse(limitedExtent.SetTile(x, y, new Tile {Wall = 1}));
            Assert.AreNotEqual(1, limitedExtent.GetTile(x, y).Wall);
        }
    }
}
