using System;
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
        [Test]
        public void Ctor_NullMask_ThrowsArgumentNullException()
        {
            var world = new World(new MockTileCollection());

            Assert.Throws<ArgumentNullException>(() => new EditSession(world, null, -1));
        }

        [Test]
        public void Ctor_NullWorld_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new EditSession(null, new NullMask(), -1));
        }

        [TestCase(0, 0)]
        public void GetTileIntInt(int x, int y)
        {
            var tiles = new ITile[20, 10];
            tiles[x, y] = new TTile {type = 1};
            var world = new World(new MockTileCollection {Tiles = tiles});
            var editSession = new EditSession(world, new NullMask(), -1);

            Assert.AreEqual(1, editSession[x, y].Type);
        }

        [TestCase(20, 10)]
        public void LowerBound(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            var world = new World(extent);
            var editSession = new EditSession(world, new NullMask(), -1);

            Assert.AreEqual(Vector.Zero, editSession.LowerBound);
        }

        [TestCase(1, 2)]
        public void Redo(byte oldWall, byte newWall)
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            world[0, 0] = new Tile {Wall = oldWall};
            var editSession = new EditSession(world, new NullMask(), -1);
            editSession[0, 0] = new Tile {Wall = newWall};
            editSession.Undo();

            Assert.AreEqual(1, editSession.Redo());
            Assert.AreEqual(newWall, editSession[0, 0].Wall);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt(int x, int y)
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var editSession = new EditSession(world, new NullMask(), -1);

            world[x, y] = new Tile {Wall = 1};

            Assert.AreEqual(1, editSession[x, y].Wall);
        }

        [TestCase(1, 0, 0)]
        public void SetTileIntInt_LimitObeyed(int limit, int x, int y)
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var editSession = new EditSession(world, new NullMask(), limit);
            for (var i = 0; i < limit; ++i)
            {
                editSession[x, y] = new Tile();
            }

            editSession[x, y] = new Tile {Wall = 1};

            Assert.AreNotEqual(1, editSession[x, y].Wall);
        }

        [TestCase(0, 0)]
        public void SetTileIntInt_MaskObeyed(int x, int y)
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var editSession = new EditSession(world, new TemplateMask(new Wall(1)), -1);

            editSession[x, y] = new Tile {Wall = 1};

            Assert.AreNotEqual(1, editSession[x, y].Wall);
        }

        [TestCase(0, 0, 10, 10)]
        public void SetTiles(int x, int y, int x2, int y2)
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var editSession = new EditSession(world, new NullMask(), -1);
            var region = new RectangularRegion(new Vector(x, y), new Vector(x2, y2));
            var template = new Block(1);

            editSession.SetTiles(region, template);

            foreach (var position in region.Where(editSession.IsInBounds))
            {
                Assert.IsTrue(template.Matches(editSession[position]));
            }
        }

        [Test]
        public void SetTiles_NullRegion_ThrowsArgumentNullException()
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var editSession = new EditSession(world, new NullMask(), -1);

            Assert.Throws<ArgumentNullException>(() => editSession.SetTiles(null, new Block(1)));
        }

        [Test]
        public void SetTiles_NullTemplate_ThrowsArgumentNullException()
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var editSession = new EditSession(world, new NullMask(), -1);

            Assert.Throws<ArgumentNullException>(() => editSession.SetTiles(new NullRegion(), null));
        }

        [TestCase(1, 2)]
        public void Undo(byte oldWall, byte newWall)
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            world[0, 0] = new Tile {Wall = oldWall};
            var editSession = new EditSession(world, new NullMask(), -1);
            editSession[0, 0] = new Tile {Wall = newWall};

            Assert.AreEqual(1, editSession.Undo());
            Assert.AreEqual(oldWall, editSession[0, 0].Wall);
        }

        [TestCase(20, 10)]
        public void UpperBound(int width, int height)
        {
            var extent = new MockTileCollection {Tiles = new ITile[width, height]};
            var world = new World(extent);
            var editSession = new EditSession(world, new NullMask(), -1);

            Assert.AreEqual(new Vector(width, height), editSession.UpperBound);
        }
    }
}
