using NUnit.Framework;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Tests.Templates.Parsers
{
    [TestFixture]
    public class BlockShapeParserTests
    {
        private static int GetBlockShape(Tile tile) => tile.IsHalfBlock ? -1 : tile.Slope;

        [TestCase("half", -1)]
        [TestCase("NORMAL", 0)]
        public void Parse(string s, int expectedBlockShape)
        {
            var parser = new BlockShapeParser();
            var tile = new Tile();

            var shape = parser.Parse(s);

            Assert.That(shape, Is.Not.Null);
            tile = shape.Apply(tile);
            Assert.That(GetBlockShape(tile), Is.EqualTo(expectedBlockShape));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidBlockShape_ReturnsNull(string s)
        {
            var parser = new BlockShapeParser();

            Assert.That(parser.Parse(s), Is.Null);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            var parser = new BlockShapeParser();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
        }
    }
}
