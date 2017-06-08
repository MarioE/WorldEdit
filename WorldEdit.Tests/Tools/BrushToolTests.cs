using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Regions;
using WorldEdit.Templates;
using WorldEdit.Tools;

namespace WorldEdit.Tests.Tools
{
    [TestFixture]
    public class BrushToolTests
    {
        [TestCase(5, 5, 2)]
        public void Apply(int x, int y, int radius)
        {
            var tile = new Tile {WallId = 1};
            var template = Mock.Of<ITemplate>(t => t.Apply(It.IsAny<Tile>()) == tile);
            var tool = new BrushTool(radius, template);
            var extent = Mock.Of<Extent>(e => e.Dimensions == new Vector(10, 10) &&
                                              e.SetTile(It.IsAny<Vector>(), It.IsAny<Tile>()));

            tool.Apply(extent, new Vector(x, y));

            var region = new EllipticRegion(new Vector(x, y), radius * Vector.One);
            foreach (var position in region.Where(extent.IsInBounds))
            {
                Mock.Get(extent).Verify(e => e.SetTile(position, tile), Times.Once);
            }
        }

        [Test]
        public void Ctor_NullTemplate_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new BrushTool(1, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_RadiusNegative_ThrowsArgumentOutOfRangeException()
        {
            var template = Mock.Of<ITemplate>();

            Assert.That(() => new BrushTool(-1, template), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}
