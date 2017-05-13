using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.History;

namespace WorldEditTests.History
{
    [TestFixture]
    public class TileChangeTests
    {
        [TestCase(0, 0, 1, 2)]
        public void Redo(int x, int y, byte oldWall, byte newWall)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var position = new Vector(x, y);
            var change = new TileChange(position, new Tile {Wall = oldWall}, new Tile {Wall = newWall});

            change.Redo(extent);

            Assert.AreEqual(newWall, extent.GetTile(x, y).Wall);
        }

        [Test]
        public void Redo_NullExtent_ThrowsArgumentNullException()
        {
            var change = new TileChange(Vector.Zero, new Tile(), new Tile());

            Assert.Throws<ArgumentNullException>(() => change.Redo(null));
        }

        [TestCase(0, 0, 1, 2)]
        public void Undo(int x, int y, byte oldWall, byte newWall)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var position = new Vector(x, y);
            var change = new TileChange(position, new Tile {Wall = oldWall}, new Tile {Wall = newWall});

            change.Undo(extent);

            Assert.AreEqual(oldWall, extent.GetTile(x, y).Wall);
        }

        [Test]
        public void Undo_NullExtent_ThrowsArgumentNullException()
        {
            var change = new TileChange(Vector.Zero, new Tile(), new Tile());

            Assert.Throws<ArgumentNullException>(() => change.Undo(null));
        }
    }
}
