using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Regions;
using WorldEdit.Core.Templates;

namespace WorldEdit.Tests.Extents
{
    [TestFixture]
    public class ExtentTests
    {
        [TestCase(0, 0, 10, 10)]
        public void ClearTiles(int x, int y, int x2, int y2)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            for (var x3 = 0; x3 < 20; ++x3)
            {
                for (var y3 = 0; y3 < 10; ++y3)
                {
                    extent.SetTile(x3, y3, new Tile {Wall = (byte)(x3 * y3 % 4)});
                }
            }
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            extent.ClearTiles(region);

            foreach (var position in region.Where(extent.IsInBounds))
            {
                Assert.AreEqual(new Tile(), extent.GetTile(position));
            }
        }

        [Test]
        public void ClearTiles_NullRegion_ThrowsArgumentNullException()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.Throws<ArgumentNullException>(() => extent.ClearTiles(null));
        }

        [TestCase(20, 10)]
        public void Dimensions(int width, int height)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};

            Assert.AreEqual(new Vector(width, height), extent.Dimensions);
        }

        [TestCase(0, 0)]
        public void GetTileVector(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.AreEqual(extent.GetTile(x, y), extent.GetTile(new Vector(x, y)));
        }

        [TestCase(20, 10, -1, 1, false)]
        [TestCase(20, 10, 21, 1, false)]
        [TestCase(20, 10, 5, 7, true)]
        public void IsInBoundsIntInt(int width, int height, int x, int y, bool expected)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};

            Assert.AreEqual(expected, extent.IsInBounds(x, y));
        }

        [TestCase(20, 10, -1, 1, false)]
        [TestCase(20, 10, 21, 1, false)]
        [TestCase(20, 10, 5, 7, true)]
        public void IsInBoundsVector(int width, int height, int x, int y, bool expected)
        {
            var extent = new MockExtent {Tiles = new ITile[width, height]};

            Assert.AreEqual(expected, extent.IsInBounds(new Vector(x, y)));
        }


        [TestCase(0, 0, 10, 10)]
        public void ReplaceTemplates(int x, int y, int x2, int y2)
        {
            var fromTemplate = Wall.Air;
            var toTemplate = Wall.Stone;
            var usedToMatch = new Dictionary<Vector, bool>();
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            for (var x3 = 0; x3 < 20; ++x3)
            {
                for (var y3 = 0; y3 < 10; ++y3)
                {
                    extent.SetTile(x3, y3, new Tile {Wall = (byte)(x3 * y3 % 4)});
                    usedToMatch[new Vector(x3, y3)] = fromTemplate.Matches(extent.GetTile(x3, y3));
                }
            }
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

            extent.ReplaceTemplates(region, fromTemplate, toTemplate);

            foreach (var position in region.Where(extent.IsInBounds))
            {
                Assert.IsFalse(fromTemplate.Matches(extent.GetTile(position)));
                if (usedToMatch[position])
                {
                    Assert.IsTrue(toTemplate.Matches(extent.GetTile(position)));
                }
            }
        }

        [Test]
        public void ReplaceTemplates_NullFromTemplate_ThrowsArgumentNullException()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.Throws<ArgumentNullException>(() => extent.ReplaceTemplates(new NullRegion(), null, Block.Lava));
        }

        [Test]
        public void ReplaceTemplates_NullRegion_ThrowsArgumentNullException()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.Throws<ArgumentNullException>(() => extent.ReplaceTemplates(null, Block.Water, Block.Lava));
        }

        [Test]
        public void ReplaceTemplates_NullToTemplate_ThrowsArgumentNullException()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.Throws<ArgumentNullException>(() => extent.ReplaceTemplates(new NullRegion(), Block.Water, null));
        }


        [TestCase(0, 0, 10, 10)]
        public void SetTemplates(int x, int y, int x2, int y2)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));
            var template = Block.Water;

            extent.SetTemplates(region, template);

            foreach (var position in region.Where(extent.IsInBounds))
            {
                Assert.IsTrue(template.Matches(extent.GetTile(position)));
            }
        }

        [Test]
        public void SetTemplates_NullRegion_ThrowsArgumentNullException()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.Throws<ArgumentNullException>(() => extent.SetTemplates(null, Block.Water));
        }

        [Test]
        public void SetTemplates_NullTemplate_ThrowsArgumentNullException()
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.Throws<ArgumentNullException>(() => extent.SetTemplates(new NullRegion(), null));
        }

        [TestCase(0, 0)]
        public void SetTileVector(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[20, 10]};

            Assert.IsTrue(extent.SetTile(new Vector(x, y), new Tile {Wall = 1}));
            Assert.AreEqual(1, extent.GetTile(x, y).Wall);
        }
    }
}
