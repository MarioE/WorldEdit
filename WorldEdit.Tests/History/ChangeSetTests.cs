using System;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.History;

namespace WorldEdit.Tests.History
{
    [TestFixture]
    public class ChangeSetTests
    {
        [Test]
        public void Add_NullChange_ThrowsArgumentNullException()
        {
            using (var changeSet = new ChangeSet())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => changeSet.Add(null), Throws.ArgumentNullException);
            }
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_TileUpdateCapacityNotPositive_ThrowsArgumentOutOfRangeException(int tileUpdateCapacity)
        {
            Assert.That(() => new ChangeSet(tileUpdateCapacity), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Redo()
        {
            using (var changeSet = new ChangeSet())
            {
                var newTile = new Tile {WallId = 1};
                changeSet.Add(new TileUpdate(Vector.Zero, new Tile(), newTile));
                var extent = Mock.Of<Extent>(e => e.SetTile(Vector.Zero, newTile));

                Assert.That(changeSet.Redo(extent), Is.EqualTo(1));
                Mock.Get(extent).Verify(e => e.SetTile(Vector.Zero, newTile), Times.Once);
            }
        }

        [Test]
        public void Redo_Files()
        {
            using (var changeSet = new ChangeSet(1))
            {
                var newTile = new Tile {WallId = 1};
                changeSet.Add(new TileUpdate(Vector.Zero, new Tile(), newTile));
                changeSet.Add(new TileUpdate(Vector.Zero, new Tile(), newTile));
                var extent = Mock.Of<Extent>(e => e.SetTile(Vector.Zero, newTile));

                Assert.That(changeSet.Redo(extent), Is.EqualTo(2));
                Mock.Get(extent).Verify(e => e.SetTile(Vector.Zero, newTile), Times.Exactly(2));
            }
        }

        [Test]
        public void Redo_NullExtent_ThrowsArgumentNullException()
        {
            using (var changeSet = new ChangeSet())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => changeSet.Redo(null), Throws.ArgumentNullException);
            }
        }

        [Test]
        public void Undo()
        {
            using (var changeSet = new ChangeSet())
            {
                var oldTile = new Tile {WallId = 1};
                changeSet.Add(new TileUpdate(Vector.Zero, oldTile, new Tile()));
                var extent = Mock.Of<Extent>(e => e.SetTile(Vector.Zero, oldTile));

                Assert.That(changeSet.Undo(extent), Is.EqualTo(1));
                Mock.Get(extent).Verify(e => e.SetTile(Vector.Zero, oldTile), Times.Once);
            }
        }

        [Test]
        public void Undo_Files()
        {
            using (var changeSet = new ChangeSet(1))
            {
                var oldTile = new Tile {WallId = 1};
                changeSet.Add(new TileUpdate(Vector.Zero, oldTile, new Tile()));
                changeSet.Add(new TileUpdate(Vector.Zero, oldTile, new Tile()));
                var extent = Mock.Of<Extent>(e => e.SetTile(Vector.Zero, oldTile));

                Assert.That(changeSet.Undo(extent), Is.EqualTo(2));
                Mock.Get(extent).Verify(e => e.SetTile(Vector.Zero, oldTile), Times.Exactly(2));
            }
        }

        [Test]
        public void Undo_NullExtent_ThrowsArgumentNullException()
        {
            using (var changeSet = new ChangeSet())
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => changeSet.Undo(null), Throws.ArgumentNullException);
            }
        }
    }
}
