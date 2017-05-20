using System;
using System.Linq;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core.Extents;
using WorldEdit.Core.History;
using TTile = Terraria.Tile;

namespace WorldEdit.Core.Tests.Extents
{
    [TestFixture]
    public class LoggedExtentTests
    {
        [Test]
        public void Ctor_NullChangeSet_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LoggedExtent(null, new ChangeSet()));
        }

        [Test]
        public void Ctor_NullExtent_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LoggedExtent(new MockExtent(), null));
        }

        [TestCase(20, 10)]
        public void Dimensions(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};
            var loggedExtent = new LoggedExtent(extent, new ChangeSet());

            Assert.AreEqual(extent.Dimensions, loggedExtent.Dimensions);
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var loggedExtent = new LoggedExtent(new MockExtent {Tiles = tiles}, new ChangeSet());

            Assert.AreEqual(1, loggedExtent.GetTile(x, y).Type);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(new MockExtent {Tiles = new ITile[20, 10]}, changeSet);

            Assert.IsTrue(loggedExtent.SetTile(x, y, new Tile {Wall = 1}));
            var changes = changeSet.ToList();
            Assert.AreEqual(1, changes.Count);
            var change = (TileChange)changes[0];
            Assert.AreEqual(new Vector(x, y), change.Position);
            Assert.AreEqual(0, change.OldTile.Wall);
            Assert.AreEqual(1, change.NewTile.Wall);
        }
    }
}
