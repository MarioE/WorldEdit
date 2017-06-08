using NUnit.Framework;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Tests.Templates.Parsers
{
    [TestFixture]
    public class BlockColorParserTests
    {
        [TestCase("blank", (byte)0)]
        [TestCase("red", (byte)1)]
        public void Parse(string s, byte expectedBlockColor)
        {
            var parser = new BlockColorParser();
            var tile = new Tile();

            var color = parser.Parse(s);

            Assert.That(color, Is.Not.Null);
            tile = color.Apply(tile);
            Assert.That(tile.BlockColor, Is.EqualTo(expectedBlockColor));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidBlockColor_ReturnsNull(string s)
        {
            var parser = new BlockColorParser();

            Assert.That(parser.Parse(s), Is.Null);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            var parser = new BlockColorParser();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
        }
    }
}
