using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Regions;
using WorldEdit.Core.Templates;
using WorldEdit.Core.Tools;

namespace WorldEdit.Tests.Tools
{
    [TestFixture]
    public class BrushToolTests
    {
        [TestCase(5, 5, 2)]
        public void Apply(int x, int y, int radius)
        {
            var pattern = new Pattern<Block>(new List<PatternEntry<Block>> {new PatternEntry<Block>(Block.Water, 1)});
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var tool = new BrushTool<Block>(radius, pattern);

                tool.Apply(world, new Vector(x, y));

                var region = new EllipticRegion(new Vector(x, y), radius * Vector.One);
                foreach (var position in region.Where(world.IsInBounds))
                {
                    Assert.IsTrue(pattern.Matches(world.GetTile(position)));
                }
            }
        }

        [Test]
        public void Ctor_NullPattern_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new BrushTool<Block>(1, null));
        }

        [Test]
        public void Ctor_RadiusNegative_ThrowsArgumentOutOfRangeException()
        {
            var pattern = new Pattern<Block>(new PatternEntry<Block>[0]);

            Assert.Throws<ArgumentOutOfRangeException>(() => new BrushTool<Block>(-1, pattern));
        }
    }
}
