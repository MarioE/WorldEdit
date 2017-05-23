using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
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

            Assert.That(tile.IsActive);
            Assert.That(tile.Type, Is.EqualTo(expectedType));
            Assert.That(tile.FrameX, Is.EqualTo(expectedFrameX));
            Assert.That(tile.FrameY, Is.EqualTo(expectedFrameY));
        }

        [Test]
        public void Apply_Air()
        {
            var tile = new Tile();

            tile = Block.Air.Apply(tile);

            Assert.That(!tile.IsActive);
        }

        [Test]
        public void Apply_Honey()
        {
            var tile = new Tile();

            tile = Block.Honey.Apply(tile);

            Assert.That(!tile.IsActive);
            Assert.That(tile.Liquid, Is.EqualTo(255));
            Assert.That(tile.LiquidType, Is.EqualTo(2));
        }

        [Test]
        public void Apply_Lava()
        {
            var tile = new Tile();

            tile = Block.Lava.Apply(tile);

            Assert.That(!tile.IsActive);
            Assert.That(tile.Liquid, Is.EqualTo(255));
            Assert.That(tile.LiquidType, Is.EqualTo(1));
        }

        [Test]
        public void Apply_Water()
        {
            var tile = new Tile();

            tile = Block.Water.Apply(tile);

            Assert.That(!tile.IsActive);
            Assert.That(tile.Liquid, Is.EqualTo(255));
            Assert.That(tile.LiquidType, Is.Zero);
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

            Assert.That(block.Matches(tile), Is.EqualTo(expected));
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Matches_Air(bool active, bool expected)
        {
            var tile = new Tile {IsActive = active};

            Assert.That(Block.Air.Matches(tile), Is.EqualTo(expected));
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

            Assert.That(Block.Honey.Matches(tile), Is.EqualTo(expected));
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

            Assert.That(Block.Lava.Matches(tile), Is.EqualTo(expected));
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

            Assert.That(Block.Water.Matches(tile), Is.EqualTo(expected));
        }

        [TestCase("STONE", 1, -1, -1)]
        [TestCase("spooky platform", 19, -1, 288)]
        [TestCase("67", 67, -1, -1)]
        [TestCase("67:7:14", 67, 7, 14)]
        [TestCase("wood platform:-1:18", 19, -1, 18)]
        public void TryParse(string s, int expectedType, short expectedFrameX, short expectedFrameY)
        {
            var tile = new Tile();

            var block = Block.TryParse(s);

            Assert.That(block, Is.Not.Null);
            tile = block.Apply(tile);
            Assert.That(tile.Type, Is.EqualTo(expectedType));
            Assert.That(tile.FrameX, Is.EqualTo(expectedFrameX));
            Assert.That(tile.FrameY, Is.EqualTo(expectedFrameY));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void TryParse_InvalidBlock_ReturnsNull(string s)
        {
            Assert.That(Block.TryParse(s), Is.Null);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => Block.TryParse(null), Throws.ArgumentNullException);
        }
    }
}
