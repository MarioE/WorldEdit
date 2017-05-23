using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.History;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.History
{
    [TestFixture]
    public class TileEntityRemovalTests
    {
        [Test]
        public void Ctor_NullEntity_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new TileEntityRemoval(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetTileEntity()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var removal = new TileEntityRemoval(tileEntity);

            Assert.That(removal.TileEntity, Is.EqualTo(tileEntity));
        }

        [Test]
        public void Redo()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var removal = new TileEntityRemoval(tileEntity);
            var extent = Mock.Of<Extent>(e => e.RemoveTileEntity(tileEntity));

            Assert.That(removal.Redo(extent));
            Mock.Get(extent).Verify(e => e.RemoveTileEntity(tileEntity), Times.Once);
        }

        [Test]
        public void Undo()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var removal = new TileEntityRemoval(tileEntity);
            var extent = Mock.Of<Extent>(e => e.AddTileEntity(tileEntity));

            Assert.That(removal.Undo(extent));
            Mock.Get(extent).Verify(e => e.AddTileEntity(tileEntity), Times.Once);
        }
    }
}
