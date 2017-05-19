using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Masks;
using WorldEdit.Sessions;
using WorldEdit.Templates;
using TTile = Terraria.Tile;

namespace WorldEditTests.Sessions
{
    [TestFixture]
    public class EditSessionTests
    {
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

        [TestCase(20, 10)]
        public void Dimensions(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            using (var world = new World(extent))
            {
                var editSession = new EditSession(world, -1, new NullMask());

                Assert.AreEqual(new Vector(width, height), editSession.Dimensions);
            }
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
    }
}
