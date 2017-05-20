using System;
using NUnit.Framework;
using WorldEdit.Core.Templates;

namespace WorldEdit.Core.Tests.Templates
{
    [TestFixture]
    public class BlockTests
    {
        private static readonly object[] ApplyTestCases =
        {
            new object[] {Block.Dirt, (ushort)0, (short)-1, (short)-1},
            new object[] {Block.EbonwoodPlatform, (ushort)19, (short)-1, (short)18}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {Block.Dirt, (ushort)0, (short)-1, (short)-1, true},
            new object[] {Block.Dirt, (ushort)0, (short)111, (short)-1, true},
            new object[] {Block.Dirt, (ushort)0, (short)-1, (short)111, true},
            new object[] {Block.Dirt, (ushort)1, (short)-1, (short)-1, false},
            new object[] {Block.EbonwoodPlatform, (ushort)19, (short)-1, (short)18, true},
            new object[] {Block.EbonwoodPlatform, (ushort)19, (short)-1, (short)-1, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(Block block, ushort expectedType, short expectedFrameX, short expectedFrameY)
        {
            var tile = new Tile();

            tile = block.Apply(tile);

            Assert.IsTrue(tile.IsActive);
            Assert.AreEqual(expectedType, tile.Type);
            Assert.AreEqual(expectedFrameX, tile.FrameX);
            Assert.AreEqual(expectedFrameY, tile.FrameY);
        }

        [Test]
        public void Apply_Air()
        {
            var tile = new Tile();

            tile = Block.Air.Apply(tile);

            Assert.IsFalse(tile.IsActive);
        }

        [Test]
        public void Apply_Honey()
        {
            var tile = new Tile();

            tile = Block.Honey.Apply(tile);

            Assert.IsFalse(tile.IsActive);
            Assert.AreEqual(255, tile.Liquid);
            Assert.AreEqual(2, tile.LiquidType);
        }

        [Test]
        public void Apply_Lava()
        {
            var tile = new Tile();

            tile = Block.Lava.Apply(tile);

            Assert.IsFalse(tile.IsActive);
            Assert.AreEqual(255, tile.Liquid);
            Assert.AreEqual(1, tile.LiquidType);
        }

        [Test]
        public void Apply_Water()
        {
            var tile = new Tile();

            tile = Block.Water.Apply(tile);

            Assert.IsFalse(tile.IsActive);
            Assert.AreEqual(255, tile.Liquid);
            Assert.AreEqual(0, tile.LiquidType);
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(Block block, ushort actualType, short actualFrameX, short actualFrameY, bool expected)
        {
            var tile = new Tile
            {
                Type = actualType,
                FrameX = actualFrameX,
                FrameY = actualFrameY,
                IsActive = true
            };

            Assert.AreEqual(expected, block.Matches(tile));
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Matches_Air(bool active, bool expected)
        {
            var tile = new Tile {IsActive = active};

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
        [TestCase("wood platform:-1:18", 19, -1, 18)]
        public void Parse(string s, int expectedType, short expectedFrameX, short expectedFrameY)
        {
            var tile = new Tile();

            var result = Block.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            tile = result.Value.Apply(tile);
            Assert.AreEqual(expectedType, tile.Type);
            Assert.AreEqual(expectedFrameX, tile.FrameX);
            Assert.AreEqual(expectedFrameY, tile.FrameY);
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
