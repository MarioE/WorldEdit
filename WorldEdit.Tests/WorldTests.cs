using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using TTile = Terraria.Tile;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class WorldTests
    {
        [Test]
        public void Ctor_NullTiles_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new World(null));
        }

        [TestCase(20, 10)]
        public void Dimensions(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            using (var world = new World(extent))
            {
                Assert.AreEqual(new Vector(width, height), world.Dimensions);
            }
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            using (var world = new World(new MockTileCollection {Tiles = tiles}))
            {
                Assert.AreEqual(1, world.GetTile(x, y).Type);
            }
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                Assert.IsTrue(world.SetTile(x, y, new Tile {Wall = 1}));
                Assert.AreEqual(1, world.GetTile(x, y).Wall);
            }
        }
    }
}
