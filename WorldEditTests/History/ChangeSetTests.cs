using System;
using System.Linq;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.History;

namespace WorldEditTests.History
{
    [TestFixture]
    public class ChangeSetTests
    {
        [Test]
        public void Add()
        {
            var changeSet = new ChangeSet();
            var change = new TileChange(Vector.Zero, new Tile(), new Tile());

            changeSet.Add(change);

            var changes = changeSet.ToList();
            Assert.AreEqual(1, changes.Count);
            Assert.AreEqual(change, changes[0]);
        }

        [Test]
        public void Add_NullChange_ThrowsArgumentNullException()
        {
            var changeSet = new ChangeSet();

            Assert.Throws<ArgumentNullException>(() => changeSet.Add(null));
        }

        [TestCase(1, 2, 3)]
        public void Redo(byte firstWall, byte secondWall, byte thirdWall)
        {
            var tiles = new ITile[20, 10];
            var extent = new MockExtent {Tiles = tiles};
            var changeSet = new ChangeSet
            {
                new TileChange(Vector.Zero, new Tile {Wall = firstWall}, new Tile {Wall = secondWall}),
                new TileChange(Vector.Zero, new Tile {Wall = secondWall}, new Tile {Wall = thirdWall})
            };

            Assert.AreEqual(2, changeSet.Redo(extent));
            Assert.AreEqual(thirdWall, extent.GetTile(0, 0).Wall);
        }

        [Test]
        public void Redo_NullExtent_ThrowsArgumentNullException()
        {
            var changeSet = new ChangeSet();

            Assert.Throws<ArgumentNullException>(() => changeSet.Redo(null));
        }

        [TestCase(1, 2, 3)]
        public void Undo(byte firstWall, byte secondWall, byte thirdWall)
        {
            var tiles = new ITile[20, 10];
            var extent = new MockExtent {Tiles = tiles};
            var changeSet = new ChangeSet
            {
                new TileChange(Vector.Zero, new Tile {Wall = firstWall}, new Tile {Wall = secondWall}),
                new TileChange(Vector.Zero, new Tile {Wall = secondWall}, new Tile {Wall = thirdWall})
            };

            Assert.AreEqual(2, changeSet.Undo(extent));
            Assert.AreEqual(firstWall, extent.GetTile(0, 0).Wall);
        }

        [Test]
        public void Undo_NullExtent_ThrowsArgumentNullException()
        {
            var changeSet = new ChangeSet();

            Assert.Throws<ArgumentNullException>(() => changeSet.Undo(null));
        }
    }
}
