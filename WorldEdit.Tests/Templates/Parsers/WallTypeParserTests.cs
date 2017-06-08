using NUnit.Framework;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Tests.Templates.Parsers
{
    [TestFixture]
    public class WallTypeParserTests
    {
        [TestCase("AIR", 0)]
        [TestCase("stone", 1)]
        [TestCase("4", 4)]
        public void Parse(string s, byte expectedWallType)
        {
            var parser = new WallTypeParser();
            var tile = new Tile();

            var wall = parser.Parse(s);

            Assert.That(wall, Is.Not.Null);
            tile = wall.Apply(tile);
            Assert.That(tile.WallId, Is.EqualTo(expectedWallType));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidWallType_ReturnsNull(string s)
        {
            var parser = new WallTypeParser();

            Assert.That(parser.Parse(s), Is.Null);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            var parser = new WallTypeParser();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
        }
    }
}
