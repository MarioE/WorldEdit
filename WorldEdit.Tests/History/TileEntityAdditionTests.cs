using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.History;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.History
{
    [TestFixture]
    public class TileEntityAdditionTests
    {
        [Test]
        public void Ctor_NullEntity_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new TileEntityAddition(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetTileEntity()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var addition = new TileEntityAddition(tileEntity);

            Assert.That(addition.TileEntity, Is.EqualTo(tileEntity));
        }

        [Test]
        public void Redo()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var addition = new TileEntityAddition(tileEntity);
            var extent = Mock.Of<Extent>(e => e.AddTileEntity(tileEntity));

            Assert.That(addition.Redo(extent));
            Mock.Get(extent).Verify(e => e.AddTileEntity(tileEntity), Times.Once);
        }

        [Test]
        public void Undo()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var addition = new TileEntityAddition(tileEntity);
            var extent = Mock.Of<Extent>(e => e.RemoveTileEntity(tileEntity));

            Assert.That(addition.Undo(extent));
            Mock.Get(extent).Verify(e => e.RemoveTileEntity(tileEntity), Times.Once);
        }
    }
}
