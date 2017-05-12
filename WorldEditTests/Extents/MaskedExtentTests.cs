using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Extents;
using WorldEdit.Masks;
using WorldEdit.Templates;
using TTile = Terraria.Tile;

namespace WorldEditTests.Extents
{
    [TestFixture]
    public class MaskedExtentTests
    {
        [Test]
        public void Ctor_NullExtent_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MaskedExtent(null, new NullMask()));
        }

        [Test]
        public void Ctor_NullMask_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MaskedExtent(new MockExtent(), null));
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var maskedExtent = new MaskedExtent(new MockExtent {Tiles = tiles}, new NullMask());

            Assert.AreEqual(1, maskedExtent[x, y].Type);
        }

        [TestCase(20, 10)]
        public void LowerBound(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var maskedExtent = new MaskedExtent(extent, new NullMask());

            Assert.AreEqual(extent.LowerBound, maskedExtent.LowerBound);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            var maskedExtent = new MaskedExtent(new MockExtent {Tiles = new ITile[20, 10]},
                new TemplateMask(new Wall(1)));
            maskedExtent[x, y] = new Tile {Wall = 2};

            Assert.AreNotEqual(2, maskedExtent[x, y].Wall);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt_MaskObeyed(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {wall = 1};
            var maskedExtent = new MaskedExtent(new MockExtent {Tiles = tiles}, new TemplateMask(new Wall(1)));
            maskedExtent[x, y] = new Tile {Wall = 2};

            Assert.AreEqual(2, maskedExtent[x, y].Wall);
        }

        [TestCase(20, 10)]
        public void UpperBound(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var maskedExtent = new MaskedExtent(extent, new NullMask());

            Assert.AreEqual(extent.UpperBound, maskedExtent.UpperBound);
        }
    }
}
