using NUnit.Framework;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Tests.Templates.Parsers
{
    [TestFixture]
    public class BlockTypeParserTests
    {
        [TestCase("STONE", 1, -1, -1)]
        [TestCase("spooky platform", 19, -1, 288)]
        [TestCase("67", 67, -1, -1)]
        [TestCase("67:7:14", 67, 7, 14)]
        [TestCase("wood platform:-1:18", 19, -1, 18)]
        public void Parse(string s, int expectedBlockId, short expectedFrameX, short expectedFrameY)
        {
            var parser = new BlockTypeParser();
            var tile = new Tile();

            var block = parser.Parse(s);

            Assert.That(block, Is.Not.Null);
            tile = block.Apply(tile);
            Assert.That(tile.BlockId, Is.EqualTo(expectedBlockId));
            Assert.That(tile.FrameX, Is.EqualTo(expectedFrameX));
            Assert.That(tile.FrameY, Is.EqualTo(expectedFrameY));
        }

        [TestCase("")]
        [TestCase("ston")]
        [TestCase("stone:a:-1")]
        public void Parse_InvalidBlockType_ReturnsNull(string s)
        {
            var parser = new BlockTypeParser();

            Assert.That(parser.Parse(s), Is.Null);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            var parser = new BlockTypeParser();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
        }
    }
}
