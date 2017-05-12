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
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new LimitedExtent(null, -1));
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var limitedExtent = new LimitedExtent(new MockExtent {Tiles = tiles}, -1);

            Assert.AreEqual(1, limitedExtent[x, y].Type);
        }

        [TestCase(20, 10)]
        public void LowerBound(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var limitedExtent = new LimitedExtent(extent, -1);

            Assert.AreEqual(extent.LowerBound, limitedExtent.LowerBound);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt_LimitNotReached(int x, int y)
        {
            var limitedExtent = new LimitedExtent(new MockExtent {Tiles = new ITile[20, 10]}, -1);

            limitedExtent[x, y] = new Tile {Wall = 1};

            Assert.AreEqual(1, limitedExtent[x, y].Wall);
        }

        [TestCase(1, 0, 0)]
        public void SetTileIntInt_LimitObeyed(int limit, int x, int y)
        {
            var limitedExtent = new LimitedExtent(new MockExtent {Tiles = new ITile[20, 10]}, 1);
            for (var i = 0; i < limit; ++i)
            {
                limitedExtent[0, 0] = new Tile();
            }

            limitedExtent[x, y] = new Tile {Wall = 1};

            Assert.AreNotEqual(1, limitedExtent[x, y].Wall);
        }

        [TestCase(20, 10)]
        public void UpperBound(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var limitedExtent = new LimitedExtent(extent, -1);

            Assert.AreEqual(extent.UpperBound, limitedExtent.UpperBound);
        }
    }
}
