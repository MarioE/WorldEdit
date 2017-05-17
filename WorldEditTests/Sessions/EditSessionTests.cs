using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Sessions;
using WorldEdit.Templates;
using TTile = Terraria.Tile;

namespace WorldEditTests.Sessions
{
    [TestFixture]
    public class EditSessionTests
    {
        [TestCase(0, 0, 10, 10)]
        public void ClearTiles(int x, int y, int x2, int y2)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                for (var x3 = 0; x3 < 20; ++x3)
                {
                    for (var y3 = 0; y3 < 10; ++y3)
                    {
                        world.SetTile(x3, y3, new Tile {Wall = (byte)(x3 * y3 % 4)});
                    }
                }
                var editSession = new EditSession(world, -1, new NullMask());
                var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

                editSession.ClearTiles(region);

                foreach (var position in region.Where(editSession.IsInBounds))
                {
                    Assert.AreEqual(new Tile(), editSession.GetTile(position));
                }
            }
        }

        [Test]
        public void ClearTiles_NullRegion_ThrowsArgumentNullException()
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.Throws<ArgumentNullException>(() => editSession.ClearTiles(null));
            }
        }

        [Test]
        public void Ctor_NullMask_ThrowsArgumentNullException()
        {
            var world = new World(new MockTileCollection());

            Assert.Throws<ArgumentNullException>(() => new EditSession(world, -1, null));
        }

        [Test]
        public void Ctor_NullWorld_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditSession(null, -1, new NullMask()));
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            using (var world = new World(new MockTileCollection {Tiles = tiles}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.AreEqual(1, editSession.GetTile(x, y).Type);
            }
        }

        [TestCase(20, 10)]
        public void LowerBound(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            using (var world = new World(extent))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.AreEqual(Vector.Zero, editSession.LowerBound);
            }
        }

        [TestCase(1, 2)]
        public void Redo(byte oldWall, byte newWall)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                world.SetTile(0, 0, new Tile {Wall = oldWall});
                var editSession = new EditSession(world, -1, new NullMask());
                editSession.SetTile(0, 0, new Tile {Wall = newWall});
                editSession.Undo();

                Assert.AreEqual(1, editSession.Redo());
                Assert.AreEqual(newWall, editSession.GetTile(0, 0).Wall);
            }
        }

        [TestCase(0, 0, 10, 10)]
        public void ReplaceTiles(int x, int y, int x2, int y2)
        {
            var fromTemplate = Wall.Air;
            var toTemplate = Wall.Stone;
            var usedToMatch = new Dictionary<Vector, bool>();
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                for (var x3 = 0; x3 < 20; ++x3)
                {
                    for (var y3 = 0; y3 < 10; ++y3)
                    {
                        world.SetTile(x3, y3, new Tile {Wall = (byte)(x3 * y3 % 4)});
                        usedToMatch[new Vector(x3, y3)] = fromTemplate.Matches(world.GetTile(x3, y3));
                    }
                }
                var editSession = new EditSession(world, -1, new NullMask());
                var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));

                editSession.ReplaceTiles(region, fromTemplate, toTemplate);

                foreach (var position in region.Where(editSession.IsInBounds))
                {
                    Assert.IsFalse(fromTemplate.Matches(editSession.GetTile(position)));
                    if (usedToMatch[position])
                    {
                        Assert.IsTrue(toTemplate.Matches(editSession.GetTile(position)));
                    }
                }
            }
        }

        [Test]
        public void ReplaceTiles_NullFromTemplate_ThrowsArgumentNullException()
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.Throws<ArgumentNullException>(
                    () => editSession.ReplaceTiles(new NullRegion(), null, Block.Lava));
            }
        }

        [Test]
        public void ReplaceTiles_NullRegion_ThrowsArgumentNullException()
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.Throws<ArgumentNullException>(() => editSession.ReplaceTiles(null, Block.Water, Block.Lava));
            }
        }

        [Test]
        public void ReplaceTiles_NullToTemplate_ThrowsArgumentNullException()
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.Throws<ArgumentNullException>(
                    () => editSession.ReplaceTiles(new NullRegion(), Block.Water, null));
            }
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.IsTrue(editSession.SetTile(x, y, new Tile {Wall = 1}));
                Assert.AreEqual(1, editSession.GetTile(x, y).Wall);
            }
        }

        [TestCase(1, 0, 0)]
        public void SetTileIntInt_LimitObeyed(int limit, int x, int y)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, limit, new NullMask());
                for (var i = 0; i < limit; ++i)
                {
                    editSession.SetTile(x, y, new Tile());
                }

                Assert.IsFalse(editSession.SetTile(x, y, new Tile {Wall = 1}));
                Assert.AreNotEqual(1, editSession.GetTile(x, y).Wall);
            }
        }

        [TestCase(0, 0)]
        public void SetTileIntInt_MaskObeyed(int x, int y)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new TemplateMask(Wall.AdamantiteBeam));

                Assert.IsFalse(editSession.SetTile(x, y, new Tile {Wall = 32}));
                Assert.AreNotEqual(32, editSession.GetTile(x, y).Wall);
            }
        }

        [TestCase(0, 0, 10, 10)]
        public void SetTiles(int x, int y, int x2, int y2)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());
                var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));
                var template = Block.Water;

                editSession.SetTiles(region, template);

                foreach (var position in region.Where(editSession.IsInBounds))
                {
                    Assert.IsTrue(template.Matches(editSession.GetTile(position)));
                }
            }
        }

        [Test]
        public void SetTiles_NullRegion_ThrowsArgumentNullException()
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.Throws<ArgumentNullException>(() => editSession.SetTiles(null, Block.Water));
            }
        }

        [Test]
        public void SetTiles_NullTemplate_ThrowsArgumentNullException()
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.Throws<ArgumentNullException>(() => editSession.SetTiles(new NullRegion(), null));
            }
        }

        [TestCase(1, 2)]
        public void Undo(byte oldWall, byte newWall)
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                world.SetTile(0, 0, new Tile {Wall = oldWall});
                var editSession = new EditSession(world, -1, new NullMask());
                editSession.SetTile(0, 0, new Tile {Wall = newWall});

                Assert.AreEqual(1, editSession.Undo());
                Assert.AreEqual(oldWall, editSession.GetTile(0, 0).Wall);
            }
        }

        [TestCase(20, 10)]
        public void UpperBound(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            using (var world = new World(extent))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.AreEqual(new Vector(width, height), editSession.UpperBound);
            }
        }
    }
}
