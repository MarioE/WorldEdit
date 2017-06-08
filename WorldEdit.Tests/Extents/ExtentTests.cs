using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Templates;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.Extents
{
    [TestFixture]
    public class ExtentTests
    {
        [Test]
        public void Clear()
        {
            var tileEntity = Mock.Of<ITileEntity>(e => e.Position == new Vector(0, 0));
            var extent = Mock.Of<Extent>(e => e.Dimensions == new Vector(10, 10) &&
                                              e.SetTile(It.IsAny<Vector>(), new Tile()));
            Mock.Get(extent).Setup(e => e.GetTileEntities()).Returns(new List<ITileEntity> {tileEntity});
            var region = Mock.Of<Region>(r => r.LowerBound == Vector.Zero && r.UpperBound == new Vector(5, 5));
            Mock.Get(region).Setup(r => r.Contains(It.IsAny<Vector>())).Returns((Vector v) => true);
            Mock.Get(region)
                .As<IEnumerable<Vector>>()
                .Setup(r => r.GetEnumerator())
                .Returns(() => region.GetEnumerator());

            Assert.That(extent.Clear(region), Is.EqualTo(25));
            for (var x = 0; x < 5; ++x)
            {
                for (var y = 0; y < 5; ++y)
                {
                    var position = new Vector(x, y);
                    Mock.Get(extent).Verify(e => e.SetTile(position, new Tile()), Times.Once);
                }
            }
            Mock.Get(extent).Verify(e => e.RemoveTileEntity(tileEntity), Times.Once);
        }

        [Test]
        public void Clear_NullRegion_ThrowsArgumentNullException()
        {
            var extent = Mock.Of<Extent>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => extent.Clear(null), Throws.ArgumentNullException);
        }

        [TestCase(-1, -1, false)]
        [TestCase(4, 5, true)]
        [TestCase(11, 11, false)]
        public void IsInBounds(int x, int y, bool expected)
        {
            var region = Mock.Of<Extent>(e => e.Dimensions == new Vector(10, 10));

            Assert.That(region.IsInBounds(new Vector(x, y)), Is.EqualTo(expected));
        }

        [Test]
        public void ModifyTilesRegionTemplateMask()
        {
            var tile = new Tile {WallId = 1};
            var region = Mock.Of<Region>(r => r.LowerBound == Vector.Zero && r.UpperBound == new Vector(5, 5));
            Mock.Get(region).Setup(r => r.Contains(It.IsAny<Vector>())).Returns((Vector v) => true);
            Mock.Get(region)
                .As<IEnumerable<Vector>>()
                .Setup(r => r.GetEnumerator())
                .Returns(() => region.GetEnumerator());
            var template = Mock.Of<ITemplate>(t => t.Apply(new Tile()) == tile);
            var extent = Mock.Of<Extent>(e => e.Dimensions == new Vector(10, 10) &&
                                              e.GetTile(It.IsAny<Vector>()) == new Tile() &&
                                              e.SetTile(It.IsAny<Vector>(), It.IsAny<Tile>()));
            var mask = Mock.Of<Mask>(m => m.Test(extent, Match.Create<Vector>(v => v.X == 0)));

            Assert.That(extent.ModifyTiles(region, template, mask), Is.EqualTo(5));
            for (var y = 0; y < 5; ++y)
            {
                var position = new Vector(0, y);
                Mock.Get(extent).Verify(e => e.SetTile(position, tile));
            }
        }

        [Test]
        public void ModifyTilesRegionTemplateMask_NullMask_ThrowsArgumentNullException()
        {
            var extent = Mock.Of<Extent>();
            var region = Mock.Of<Region>();
            var template = Mock.Of<ITemplate>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => extent.ModifyTiles(region, template, null), Throws.ArgumentNullException);
        }

        [Test]
        public void ModifyTilesRegionTemplateMask_NullRegion_ThrowsArgumentNullException()
        {
            var extent = Mock.Of<Extent>();
            var template = Mock.Of<ITemplate>();
            var mask = Mock.Of<Mask>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => extent.ModifyTiles(null, template, mask), Throws.ArgumentNullException);
        }

        [Test]
        public void ModifyTilesRegionTemplateMask_NullTemplate_ThrowsArgumentNullException()
        {
            var extent = Mock.Of<Extent>();
            var region = Mock.Of<Region>();
            var mask = Mock.Of<Mask>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => extent.ModifyTiles(region, null, mask), Throws.ArgumentNullException);
        }
    }
}
