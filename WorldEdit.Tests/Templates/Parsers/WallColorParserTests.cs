using NUnit.Framework;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Tests.Templates.Parsers
{
    [TestFixture]
    public class WallColorParserTests
    {
        [TestCase("blank", (byte)0)]
        [TestCase("red", (byte)1)]
        public void Parse(string s, byte expectedColor)
        {
            var parser = new WallColorParser();
            var tile = new Tile();

            var wallColor = parser.Parse(s);

            Assert.That(wallColor, Is.Not.Null);
            tile = wallColor.Apply(tile);
            Assert.That(tile.WallColor, Is.EqualTo(expectedColor));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidWallColor_ReturnsNull(string s)
        {
            var parser = new WallColorParser();

            Assert.That(parser.Parse(s), Is.Null);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            var parser = new WallColorParser();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
        }
    }
}
