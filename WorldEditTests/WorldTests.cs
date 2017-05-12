using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using TTile = Terraria.Tile;

namespace WorldEditTests
{
    [TestFixture]
    public class WorldTests
    {
        [Test]
        public void Ctor_NullTiles_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new World(null));
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var world = new World(new MockTileCollection {Tiles = tiles});

            Assert.AreEqual(1, world[x, y].Type);
        }

        [TestCase(20, 10)]
        public void LowerBound(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            var world = new World(extent);

            Assert.AreEqual(Vector.Zero, world.LowerBound);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});

            world[x, y] = new Tile {Wall = 1};

            Assert.AreEqual(1, world[x, y].Wall);
        }

        [TestCase(20, 10)]
        public void UpperBound(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            var world = new World(extent);

            Assert.AreEqual(new Vector(width, height), world.UpperBound);
        }
    }
}
