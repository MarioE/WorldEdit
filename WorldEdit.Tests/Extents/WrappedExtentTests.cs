using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.Extents
{
    [TestFixture]
    public class WrappedExtentTests
    {
        [Test]
        public void AddTileEntity()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var extent = Mock.Of<Extent>(e => e.AddTileEntity(tileEntity));
            var wrappedExtent = new WrappedExtent(extent);

            Assert.That(wrappedExtent.AddTileEntity(tileEntity));
            Mock.Get(extent).Verify(e => e.AddTileEntity(tileEntity), Times.Once);
        }

        [Test]
        public void Dimensions()
        {
            var extent = Mock.Of<Extent>(e => e.Dimensions == new Vector(10, 10));
            var wrappedExtent = new WrappedExtent(extent);

            Assert.That(wrappedExtent.Dimensions, Is.EqualTo(extent.Dimensions));
            Mock.Get(extent).Verify(e => e.Dimensions);
        }

        [TestCase(3, 4)]
        public void GetTile(int x, int y)
        {
            var position = new Vector(x, y);
            var extent = Mock.Of<Extent>(e => e.GetTile(position) == new Tile {Wall = 1});
            var wrappedExtent = new WrappedExtent(extent);

            Assert.That(wrappedExtent.GetTile(position), Is.EqualTo(extent.GetTile(position)));
            Mock.Get(extent).Verify(e => e.GetTile(position));
        }

        [Test]
        public void GetTileEntities()
        {
            var extent = Mock.Of<Extent>();
            Mock.Get(extent).Setup(e => e.GetTileEntities()).Returns(new List<ITileEntity>());
            var wrappedExtent = new WrappedExtent(extent);

            Assert.That(wrappedExtent.GetTileEntities(), Is.Empty);
            Mock.Get(extent).Verify(e => e.GetTileEntities(), Times.Once);
        }

        [Test]
        public void RemoveTileEntity()
        {
            var tileEntity = Mock.Of<ITileEntity>();
            var extent = Mock.Of<Extent>(e => e.RemoveTileEntity(tileEntity));
            var wrappedExtent = new WrappedExtent(extent);

            Assert.That(wrappedExtent.RemoveTileEntity(tileEntity));
            Mock.Get(extent).Verify(e => e.RemoveTileEntity(tileEntity), Times.Once);
        }

        [TestCase(3, 4)]
        public void SetTile(int x, int y)
        {
            var position = new Vector(x, y);
            var tile = new Tile {Wall = 1};
            var extent = Mock.Of<Extent>(e => e.SetTile(position, tile));
            var wrappedExtent = new WrappedExtent(extent);

            Assert.That(wrappedExtent.SetTile(position, tile));
            Mock.Get(extent).Verify(e => e.SetTile(position, tile), Times.Once);
        }
    }
}
