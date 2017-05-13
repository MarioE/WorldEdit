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

            Assert.IsTrue(change.Redo(extent));
            Assert.AreEqual(newWall, extent.GetTile(x, y).Wall);
        }

        [TestCase(-1, -1)]
        [TestCase(20, -1)]
        public void Redo_False(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var position = new Vector(x, y);
            var change = new TileChange(position, new Tile(), new Tile());

            Assert.IsFalse(change.Redo(extent));
        }

        [TestCase(0, 0, 1, 2)]
        public void Undo(int x, int y, byte oldWall, byte newWall)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var position = new Vector(x, y);
            var change = new TileChange(position, new Tile {Wall = oldWall}, new Tile {Wall = newWall});

            Assert.IsTrue(change.Undo(extent));
            Assert.AreEqual(oldWall, extent.GetTile(x, y).Wall);
        }

        [TestCase(-1, -1)]
        [TestCase(20, -1)]
        public void Undo_False(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var position = new Vector(x, y);
            var change = new TileChange(position, new Tile(), new Tile());

            Assert.IsFalse(change.Undo(extent));
        }
    }
}
