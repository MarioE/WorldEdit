using System;
using System.Collections.Generic;
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
            var pattern = new Pattern<Block>(new List<PatternEntry<Block>> {new PatternEntry<Block>(Block.Water, 1)});
            var tool = new BrushTool<Block>(radius, pattern);
            var extent = Mock.Of<Extent>(e => e.Dimensions == new Vector(10, 10) &&
                                              e.SetTile(It.IsAny<Vector>(), It.IsAny<Tile>()));

            tool.Apply(extent, new Vector(x, y));

            var region = new EllipticRegion(new Vector(x, y), radius * Vector.One);
            foreach (var position in region.Where(extent.IsInBounds))
            {
                Mock.Get(extent).Verify(e => e.SetTile(position, Match.Create<Tile>(pattern.Matches)), Times.Once);
            }
        }

        [Test]
        public void Ctor_NullPattern_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new BrushTool<Block>(1, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_RadiusNegative_ThrowsArgumentOutOfRangeException()
        {
            var pattern = new Pattern<Block>(new PatternEntry<Block>[0]);

            Assert.That(() => new BrushTool<Block>(-1, pattern), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
    }
}
