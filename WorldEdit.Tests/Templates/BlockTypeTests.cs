using System;
using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class BlockTypeTypeTests
    {
        private static readonly object[] ApplyTestCases =
        {
            new object[] {BlockType.Dirt, (ushort)0, (short)-1, (short)-1},
            new object[] {BlockType.EbonwoodPlatform, (ushort)19, (short)-1, (short)18}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {BlockType.Dirt, (ushort)0, (short)-1, (short)-1, true},
            new object[] {BlockType.Dirt, (ushort)0, (short)111, (short)-1, true},
            new object[] {BlockType.Dirt, (ushort)0, (short)-1, (short)111, true},
            new object[] {BlockType.Dirt, (ushort)1, (short)-1, (short)-1, false},
            new object[] {BlockType.EbonwoodPlatform, (ushort)19, (short)-1, (short)18, true},
            new object[] {BlockType.EbonwoodPlatform, (ushort)19, (short)-1, (short)-1, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(BlockType blockType, ushort expectedBlockId, short expectedFrameX, short expectedFrameY)
        {
            var tile = new Tile();

            tile = blockType.Apply(tile);

            Assert.That(tile.IsActive);
            Assert.That(tile.BlockId, Is.EqualTo(expectedBlockId));
            Assert.That(tile.FrameX, Is.EqualTo(expectedFrameX));
            Assert.That(tile.FrameY, Is.EqualTo(expectedFrameY));
        }

        [Test]
        public void Apply_Air()
        {
            var tile = new Tile();

            tile = BlockType.Air.Apply(tile);

            Assert.That(!tile.IsActive);
        }

        [Test]
        public void Apply_Honey()
        {
            var tile = new Tile();

            tile = BlockType.Honey.Apply(tile);

            Assert.That(!tile.IsActive);
            Assert.That(tile.Liquid, Is.EqualTo(255));
            Assert.That(tile.LiquidType, Is.EqualTo(2));
        }

        [Test]
        public void Apply_Lava()
        {
            var tile = new Tile();

            tile = BlockType.Lava.Apply(tile);

            Assert.That(!tile.IsActive);
            Assert.That(tile.Liquid, Is.EqualTo(255));
            Assert.That(tile.LiquidType, Is.EqualTo(1));
        }

        [Test]
        public void Apply_Water()
        {
            var tile = new Tile();

            tile = BlockType.Water.Apply(tile);

            Assert.That(!tile.IsActive);
            Assert.That(tile.Liquid, Is.EqualTo(255));
            Assert.That(tile.LiquidType, Is.Zero);
        }

        [TestCase(2)]
        public void FromId(int id)
        {
            var blockType = BlockType.FromId((ushort)id);
            var tile = new Tile();

            tile = blockType.Apply(tile);

            Assert.That(tile.BlockId, Is.EqualTo(id));
        }

        [TestCase(10000)]
        public void FromId_IdTooLarge_ThrowsOutOfRangeException(int id)
        {
            Assert.That(() => BlockType.FromId((ushort)id), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(BlockType blockType, ushort actualBlockId, short actualFrameX, short actualFrameY,
            bool expected)
        {
            var tile = new Tile
            {
                BlockId = actualBlockId,
                FrameX = actualFrameX,
                FrameY = actualFrameY,
                IsActive = true
            };

            Assert.That(blockType.Matches(tile), Is.EqualTo(expected));
        }

        [TestCase(true, false)]
        [TestCase(false, true)]
        public void Matches_Air(bool active, bool expected)
        {
            var tile = new Tile {IsActive = active};

            Assert.That(BlockType.Air.Matches(tile), Is.EqualTo(expected));
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

            Assert.That(BlockType.Honey.Matches(tile), Is.EqualTo(expected));
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

            Assert.That(BlockType.Lava.Matches(tile), Is.EqualTo(expected));
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

            Assert.That(BlockType.Water.Matches(tile), Is.EqualTo(expected));
        }

        [TestCase(4, 6)]
        public void WithFrames(short frameX, short frameY)
        {
            var tile = new Tile();

            var block2 = BlockType.Dirt.WithFrames(frameX, frameY);

            tile = block2.Apply(tile);
            Assert.That(tile.FrameX, Is.EqualTo(frameX));
            Assert.That(tile.FrameY, Is.EqualTo(frameY));
        }
    }
}
