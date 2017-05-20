using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.History;

namespace WorldEdit.Tests.History
{
    [TestFixture]
    public class TileChangeTests
    {
        [TestCase(1)]
        public void GetNewTile(byte wall)
        {
            var change = new TileChange(Vector.Zero, new Tile(), new Tile {Wall = wall});

            Assert.AreEqual(wall, change.NewTile.Wall);
        }

        [TestCase(1)]
        public void GetOldTile(byte wall)
        {
            var change = new TileChange(Vector.Zero, new Tile {Wall = wall}, new Tile());

            Assert.AreEqual(wall, change.OldTile.Wall);
        }

        [TestCase(0, 1)]
        public void GetPosition(int x, int y)
        {
            var change = new TileChange(new Vector(x, y), new Tile(), new Tile());

            Assert.AreEqual(new Vector(x, y), change.Position);
        }

        [TestCase(0, 0, 1, 2)]
        public void Redo(int x, int y, byte oldWall, byte newWall)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var position = new Vector(x, y);
            var change = new TileChange(position, new Tile {Wall = oldWall}, new Tile {Wall = newWall});

            Assert.IsTrue(change.Redo(extent));
            Assert.AreEqual(newWall, extent.GetTile(x, y).Wall);
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
    }
}
