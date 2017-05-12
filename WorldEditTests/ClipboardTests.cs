using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Regions;

namespace WorldEditTests
{
    [TestFixture]
    public class ClipboardTests
    {
        [Test]
        public void CopyFrom()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            for (var x = 0; x < 20; ++x)
            {
                for (var y = 0; y < 10; ++y)
                {
                    extent[x, y] = new Tile {Wall = (byte)(x * y)};
                }
            }
            var region = new RectangularRegion(Vector.Zero, new Vector(10, 5));

            var clipboard = Clipboard.CopyFrom(extent, region);

            Assert.AreEqual(Vector.Zero, clipboard.LowerBound);
            Assert.AreEqual(new Vector(11, 6), clipboard.UpperBound);
            for (var x = 0; x < 10; ++x)
            {
                for (var y = 0; y < 5; ++y)
                {
                    Assert.AreEqual(x * y, clipboard[x, y].Wall);
                }
            }
        }

        [Test]
        public void CopyFrom_NullExtent_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Clipboard.CopyFrom(null, new NullRegion()));
        }

        [Test]
        public void CopyFrom_NullRegion_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Clipboard.CopyFrom(new MockExtent(), null));
        }

        [Test]
        public void CopyFrom_OutOfBounds()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            for (var x = 0; x < 20; ++x)
            {
                for (var y = 0; y < 10; ++y)
                {
                    extent[x, y] = new Tile {Wall = (byte)(x * y)};
                }
            }
            var region = new RectangularRegion(new Vector(19, 9), new Vector(29, 14));

            var clipboard = Clipboard.CopyFrom(extent, region);

            Assert.AreEqual(Vector.Zero, clipboard.LowerBound);
            Assert.AreEqual(new Vector(11, 6), clipboard.UpperBound);
            Assert.AreEqual(19 * 9, clipboard[0, 0].Wall);
        }

        [Test]
        public void Ctor_NullTiles_ThrowsArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new Clipboard(null));
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new Tile?[20, 10];
            tiles[x, y] = new Tile {Type = 1};
            var clipboard = new Clipboard(tiles);

            Assert.AreEqual(1, clipboard[x, y].Type);
        }

        [TestCase(20, 10)]
        public void LowerBound(int width, int height)
        {
            var clipboard = new Clipboard(new Tile?[width, height]);

            Assert.AreEqual(Vector.Zero, clipboard.LowerBound);
        }

        [Test]
        public void PasteTo()
        {
            var clipboard = new Clipboard(new Tile?[10, 5]);
            for (var x = 0; x < 10; ++x)
            {
                for (var y = 0; y < 5; ++y)
                {
                    clipboard[x, y] = new Tile {Wall = (byte)(x * y)};
                }
            }
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            clipboard.PasteTo(extent, Vector.Zero);

            for (var x = 0; x < 10; ++x)
            {
                for (var y = 0; y < 5; ++y)
                {
                    Assert.AreEqual(x * y, extent[x, y].Wall);
                }
            }
        }

        [Test]
        public void PasteTo_NullExtent_ThrowsArgumentNullException()
        {
            var clipboard = new Clipboard(new Tile?[0, 0]);

            Assert.Throws<ArgumentNullException>(() => clipboard.PasteTo(null, Vector.Zero));
        }

        [Test]
        public void PasteTo_OutOfBounds()
        {
            var clipboard = new Clipboard(new Tile?[10, 5]);
            for (var x = 0; x < 10; ++x)
            {
                for (var y = 0; y < 5; ++y)
                {
                    clipboard[x, y] = new Tile {Wall = (byte)(x * y)};
                }
            }
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            clipboard.PasteTo(extent, new Vector(19, 9));

            Assert.AreEqual(0, extent[19, 9].Wall);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            var clipboard = new Clipboard(new Tile?[20, 10]);

            clipboard[x, y] = new Tile {Wall = 1};

            Assert.AreEqual(1, clipboard[x, y].Wall);
        }

        [TestCase(20, 10)]
        public void UpperBound(int width, int height)
        {
            var clipboard = new Clipboard(new Tile?[width, height]);

            Assert.AreEqual(new Vector(width, height), clipboard.UpperBound);
        }
    }
}
