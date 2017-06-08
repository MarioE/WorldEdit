using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Regions;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class ClipboardTests
    {
        [TestCase(-3, 4)]
        public void AddTileEntity_False(int x, int y)
        {
            var clipboard = new Clipboard(new Tile?[10, 10]);
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(x, y));

            Assert.That(!clipboard.AddTileEntity(tileEntity));
        }

        [TestCase(3, 4)]
        public void AddTileEntityGetTileEntities(int x, int y)
        {
            var clipboard = new Clipboard(new Tile?[10, 10]);
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(x, y));

            Assert.That(clipboard.AddTileEntity(tileEntity));
            var tileEntities = clipboard.GetTileEntities().ToList();
            Assert.That(tileEntities, Has.Count.EqualTo(1));
            Assert.That(tileEntities[0].Position, Is.EqualTo(new Vector(x, y)));
        }

        [Test]
        public void CopyFrom()
        {
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == Vector.One);
            Mock.Get(tileEntity)
                .Setup(e => e.WithPosition(Vector.Zero))
                .Returns((Vector v) => Mock.Of<ITileEntity>(e => e.Position == v));
            var tile = new Tile {WallId = 1};
            var extent = Mock.Of<Extent>(e => e.Dimensions == new Vector(5, 5) &&
                                              e.GetTileEntities() == new[] {tileEntity} &&
                                              e.GetTile(It.IsAny<Vector>()) == tile);
            var region = Mock.Of<Region>(r => r.LowerBound == Vector.One && r.UpperBound == new Vector(5, 5));
            Mock.Get(region).Setup(r => r.Contains(It.IsAny<Vector>())).Returns((Vector v) => true);
            Mock.Get(region)
                .As<IEnumerable<Vector>>()
                .Setup(r => r.GetEnumerator())
                .Returns(() => region.GetEnumerator());

            var clipboard = Clipboard.CopyFrom(extent, region);

            for (var x = 0; x < 4; ++x)
            {
                for (var y = 0; y < 4; ++y)
                {
                    Assert.That(clipboard.GetTile(new Vector(x, y)), Is.EqualTo(tile));
                }
            }
            var tileEntities = clipboard.GetTileEntities().ToList();
            Assert.That(tileEntities, Has.Count.EqualTo(1));
            Assert.That(tileEntities[0].Position, Is.EqualTo(Vector.Zero));
        }

        [Test]
        public void CopyFrom_NullExtent_ThrowsArgumentNullException()
        {
            var region = Mock.Of<Region>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => Clipboard.CopyFrom(null, region), Throws.ArgumentNullException);
        }

        [Test]
        public void CopyFrom_NullRegion_ThrowsArgumentNullException()
        {
            var extent = Mock.Of<Extent>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => Clipboard.CopyFrom(extent, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_NullTiles_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new Clipboard(null), Throws.ArgumentNullException);
        }

        [TestCase(10, 10)]
        public void Dimensions(int width, int height)
        {
            var clipboard = new Clipboard(new Tile?[width, height]);

            Assert.That(clipboard.Dimensions, Is.EqualTo(new Vector(width, height)));
        }

        [TestCase(1, 1)]
        public void GetTileIntInt(int x, int y)
        {
            var tile = new Tile {WallId = 1};
            var tiles = new Tile?[10, 10];
            tiles[x, y] = tile;
            var clipboard = new Clipboard(tiles);

            Assert.That(clipboard.GetTile(new Vector(x, y)), Is.EqualTo(tile));
        }

        [Test]
        public void PasteTo()
        {
            var tile = new Tile {WallId = 1};
            var tiles = new Tile?[4, 4];
            for (var x = 0; x < 3; ++x)
            {
                for (var y = 0; y < 3; ++y)
                {
                    tiles[x, y] = tile;
                }
            }
            var clipboard = new Clipboard(tiles);
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == Vector.Zero);
            Mock.Get(tileEntity)
                .Setup(e => e.WithPosition(Vector.One))
                .Returns((Vector v) => Mock.Of<ITileEntity>(e => e.Position == v));
            clipboard.AddTileEntity(tileEntity);
            var extent = Mock.Of<Extent>(e => e.Dimensions == new Vector(5, 5) &&
                                              e.AddTileEntity(
                                                  Match.Create<ITileEntity>(n => n.Position == Vector.One)) &&
                                              e.SetTile(It.IsAny<Vector>(), It.IsAny<Tile>()));

            Assert.That(clipboard.PasteTo(extent, Vector.One), Is.EqualTo(10));
            for (var x = 1; x < 4; ++x)
            {
                for (var y = 1; y < 4; ++y)
                {
                    var x1 = x;
                    var y1 = y;
                    Mock.Get(extent).Verify(e => e.SetTile(new Vector(x1, y1), tile));
                }
            }
            Mock.Get(extent).Verify(e => e.AddTileEntity(Match.Create<ITileEntity>(n => n.Position == Vector.One)));
        }

        [Test]
        public void PasteTo_NullExtent_ThrowsArgumentNullException()
        {
            var clipboard = new Clipboard(new Tile?[0, 0]);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => clipboard.PasteTo(null, Vector.Zero), Throws.ArgumentNullException);
        }

        [TestCase(3, 4)]
        public void RemoveTileEntityGetTileEntities(int x, int y)
        {
            var clipboard = new Clipboard(new Tile?[10, 10]);
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(x, y));
            clipboard.AddTileEntity(tileEntity);

            Assert.That(clipboard.RemoveTileEntity(tileEntity));
            Assert.That(clipboard.GetTileEntities(), Is.Empty);
        }

        [TestCase(1, 1)]
        public void SetTileIntInt(int x, int y)
        {
            var tiles = new Tile?[10, 10];
            var clipboard = new Clipboard(tiles);
            var tile = new Tile {WallId = 1};

            Assert.That(clipboard.SetTile(new Vector(x, y), tile));
            Assert.That(tiles[x, y], Is.EqualTo(tile));
        }
    }
}
