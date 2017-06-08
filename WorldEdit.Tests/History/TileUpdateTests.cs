using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.History;

namespace WorldEdit.Tests.History
{
    [TestFixture]
    public class TileUpdateTests
    {
        [Test]
        public void GetNewTile()
        {
            var newTile = new Tile {WallId = 1};
            var change = new TileUpdate(Vector.Zero, new Tile(), newTile);

            Assert.That(change.NewTile, Is.EqualTo(newTile));
        }

        [Test]
        public void GetOldTile()
        {
            var oldTile = new Tile {WallId = 1};
            var change = new TileUpdate(Vector.Zero, oldTile, new Tile());

            Assert.That(change.OldTile, Is.EqualTo(oldTile));
        }

        [TestCase(3, 4)]
        public void GetPosition(int x, int y)
        {
            var change = new TileUpdate(new Vector(x, y), new Tile(), new Tile());

            Assert.That(change.Position, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(3, 4)]
        public void Redo(int x, int y)
        {
            var position = new Vector(x, y);
            var newTile = new Tile {WallId = 1};
            var change = new TileUpdate(position, new Tile(), newTile);
            var extent = Mock.Of<Extent>(e => e.SetTile(position, It.IsAny<Tile>()));

            Assert.That(change.Redo(extent));
            Mock.Get(extent).Verify(e => e.SetTile(position, newTile), Times.Once);
        }

        [TestCase(3, 4)]
        public void Undo(int x, int y)
        {
            var position = new Vector(x, y);
            var oldTile = new Tile {WallId = 1};
            var change = new TileUpdate(position, oldTile, new Tile());
            var extent = Mock.Of<Extent>(e => e.SetTile(position, It.IsAny<Tile>()));

            Assert.That(change.Undo(extent));
            Mock.Get(extent).Verify(e => e.SetTile(position, oldTile), Times.Once);
        }
    }
}
