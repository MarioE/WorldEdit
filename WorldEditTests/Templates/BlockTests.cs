using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class BlockTests
    {
        [TestCase(1, -1, -1)]
        [TestCase(19, -1, 18)]
        public void Apply(int type, short frameX, short frameY)
        {
            var tile = new Tile();
            var block = new Block(type, frameX, frameY);

            tile = block.Apply(tile);

            Assert.IsTrue(tile.Active);
            Assert.AreEqual(type, tile.Type);
            Assert.AreEqual(frameX, tile.FrameX);
            Assert.AreEqual(frameY, tile.FrameY);
        }

        [Test]
        public void Apply_Air()
        {
            var tile = new Tile();

            tile = Block.Air.Apply(tile);

            Assert.IsFalse(tile.Active);
        }

        [Test]
        public void Apply_Honey()
        {
            var tile = new Tile();

            tile = Block.Honey.Apply(tile);

            Assert.IsFalse(tile.Active);
            Assert.AreEqual(255, tile.Liquid);
            Assert.AreEqual(2, tile.LiquidType);
        }

        [Test]
        public void Apply_Lava()
        {
            var tile = new Tile();

            tile = Block.Lava.Apply(tile);

            Assert.IsFalse(tile.Active);
            Assert.AreEqual(255, tile.Liquid);
            Assert.AreEqual(1, tile.LiquidType);
        }

        [Test]
        public void Apply_Water()
        {
            var tile = new Tile();

            tile = Block.Water.Apply(tile);

            Assert.IsFalse(tile.Active);
            Assert.AreEqual(255, tile.Liquid);
            Assert.AreEqual(0, tile.LiquidType);
        }

        [TestCase(1)]
        public void GetFrameX(short frameX)
        {
            var block = new Block(0, frameX);

            Assert.AreEqual(frameX, block.FrameX);
        }

        [TestCase(1)]
        public void GetFrameY(short frameY)
        {
            var block = new Block(0, -1, frameY);

            Assert.AreEqual(frameY, block.FrameY);
        }

        [TestCase(5)]
        public void GetType(int type)
        {
            var block = new Block(type);

            Assert.AreEqual(type, block.Type);
        }

        [TestCase(1, -1, -1, 0, -1, -1, false)]
        [TestCase(1, 1, 1, 1, 1, -1, false)]
        [TestCase(1, -1, -1, 1, -1, -1, true)]
        [TestCase(1, -1, -1, 1, 16, -1, true)]
        [TestCase(1, 1, -1, 1, 1, -16, true)]
        [TestCase(1, 1, 1, 1, 1, 1, true)]
        public void Matches(int type, short frameX, short frameY, int actualType, short actualFrameX,
            short actualFrameY, bool expected)
        {
            var tile = new Tile
            {
                Type = (ushort)actualType,
                FrameX = actualFrameX,
                FrameY = actualFrameY,
                Active = true
            };
            var block = new Block(type, frameX, frameY);

            Assert.AreEqual(expected, block.Matches(tile));
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Matches_Air(bool active, bool expected)
        {
            var tile = new Tile {Active = active};

            Assert.AreEqual(expected, Block.Air.Matches(tile));
        }

        [TestCase(0, 0, false)]
        [TestCase(255, 0, false)]
        [TestCase(255, 1, false)]
        [TestCase(255, 2, true)]
        public void Matches_Honey(byte liquid, int liquidType, bool expected)
        {
            var tile = new Tile
            {
                Liquid = liquid,
                LiquidType = liquidType
            };

            Assert.AreEqual(expected, Block.Honey.Matches(tile));
        }

        [TestCase(0, 0, false)]
        [TestCase(255, 0, false)]
        [TestCase(255, 1, true)]
        [TestCase(255, 2, false)]
        public void Matches_Lava(byte liquid, int liquidType, bool expected)
        {
            var tile = new Tile
            {
                Liquid = liquid,
                LiquidType = liquidType
            };

            Assert.AreEqual(expected, Block.Lava.Matches(tile));
        }

        [TestCase(0, 0, false)]
        [TestCase(255, 0, true)]
        [TestCase(255, 1, false)]
        [TestCase(255, 2, false)]
        public void Matches_Water(byte liquid, int liquidType, bool expected)
        {
            var tile = new Tile
            {
                Liquid = liquid,
                LiquidType = liquidType
            };

            Assert.AreEqual(expected, Block.Water.Matches(tile));
        }

        [TestCase("STONE", 1, -1, -1)]
        [TestCase("spooky platform", 19, -1, 288)]
        [TestCase("67", 67, -1, -1)]
        [TestCase("67:7:14", 67, 7, 14)]
        public void Parse(string s, int expectedType, short expectedFrameX, short expectedFrameY)
        {
            var result = Block.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(expectedType, result.Value.Type);
            Assert.AreEqual(expectedFrameX, result.Value.FrameX);
            Assert.AreEqual(expectedFrameY, result.Value.FrameY);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidBlock(string s)
        {
            var result = Block.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Block.Parse(null));
        }
    }
}
