using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Extents;
using WorldEdit.Core.Masks;
using WorldEdit.Core.Templates;
using TTile = Terraria.Tile;

namespace WorldEdit.Tests.Extents
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

        [TestCase(20, 10)]
        public void Dimensions(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var maskedExtent = new MaskedExtent(extent, new NullMask());

            Assert.AreEqual(extent.Dimensions, maskedExtent.Dimensions);
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var maskedExtent = new MaskedExtent(new MockExtent {Tiles = tiles}, new NullMask());

            Assert.AreEqual(1, maskedExtent.GetTile(x, y).Type);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt_MaskFailed(int x, int y)
        {
            var maskedExtent = new MaskedExtent(new MockExtent {Tiles = new ITile[20, 10]},
                new TemplateMask(Wall.AdamantiteBeam));

            Assert.IsFalse(maskedExtent.SetTile(x, y, new Tile {Wall = 2}));
            Assert.AreNotEqual(2, maskedExtent.GetTile(x, y).Wall);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt_MaskPassed(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {wall = 32};
            var maskedExtent = new MaskedExtent(new MockExtent {Tiles = tiles}, new TemplateMask(Wall.AdamantiteBeam));

            Assert.IsTrue(maskedExtent.SetTile(x, y, new Tile {Wall = 2}));
            Assert.AreEqual(2, maskedExtent.GetTile(x, y).Wall);
        }
    }
}
