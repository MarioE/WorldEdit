using System.Linq;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.History;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.Extents
{
    [TestFixture]
    public class LoggedExtentTests
    {
        [TestCase(3, 4)]
        public void AddTileEntity_Failed_WasNotLogged(int x, int y)
        {
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(x, y));
            var extent = Mock.Of<Extent>(e => !e.AddTileEntity(tileEntity));
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(extent, changeSet);

            Assert.That(!loggedExtent.AddTileEntity(tileEntity));
            Assert.That(changeSet, Is.Empty);
        }

        [TestCase(3, 4)]
        public void AddTileEntity_Succeeded_WasLogged(int x, int y)
        {
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(x, y));
            var extent = Mock.Of<Extent>(e => e.AddTileEntity(tileEntity));
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(extent, changeSet);

            Assert.That(loggedExtent.AddTileEntity(tileEntity));
            var changes = changeSet.ToList();
            Assert.That(changes, Has.Count.EqualTo(1));
            Assert.That(((TileEntityAddition)changes[0]).TileEntity, Is.EqualTo(tileEntity));
        }

        [Test]
        public void Ctor_NullChangeSet_ThrowsArgumentNullException()
        {
            var extent = Mock.Of<Extent>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new LoggedExtent(extent, null), Throws.ArgumentNullException);
        }
        
        [TestCase(3, 4)]
        public void RemoveTileEntity_Failed_WasNotLogged(int x, int y)
        {
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(x, y));
            var extent = Mock.Of<Extent>(e => !e.RemoveTileEntity(tileEntity));
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(extent, changeSet);

            Assert.That(!loggedExtent.RemoveTileEntity(tileEntity));
            Assert.That(changeSet, Is.Empty);
        }

        [TestCase(3, 4)]
        public void RemoveTileEntity_Succeeded_WasLogged(int x, int y)
        {
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(x, y));
            var extent = Mock.Of<Extent>(e => e.RemoveTileEntity(tileEntity));
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(extent, changeSet);

            Assert.That(loggedExtent.RemoveTileEntity(tileEntity));
            var changes = changeSet.ToList();
            Assert.That(changes, Has.Count.EqualTo(1));
            Assert.That(((TileEntityRemoval)changes[0]).TileEntity, Is.EqualTo(tileEntity));
        }

        [TestCase(3, 4)]
        public void SetTile_Failed_WasNotLogged(int x, int y)
        {
            var position = new Vector(x, y);
            var tile = new Tile {Wall = 1};
            var extent = Mock.Of<Extent>(e => !e.SetTile(position, tile));
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(extent, changeSet);

            Assert.That(!loggedExtent.SetTile(position, tile));
            Assert.That(changeSet, Is.Empty);
        }

        [TestCase(3, 4)]
        public void SetTile_Succeeded_WasLogged(int x, int y)
        {
            var position = new Vector(x, y);
            var tile = new Tile {Wall = 1};
            var extent = Mock.Of<Extent>(e => e.SetTile(position, tile));
            var changeSet = new ChangeSet();
            var loggedExtent = new LoggedExtent(extent, changeSet);

            Assert.That(loggedExtent.SetTile(position, tile));
            var changes = changeSet.ToList();
            Assert.That(changes, Has.Count.EqualTo(1));
            Assert.That(((TileUpdate)changes[0]).NewTile, Is.EqualTo(tile));
        }
    }
}
