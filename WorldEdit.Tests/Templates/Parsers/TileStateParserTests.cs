using NUnit.Framework;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Tests.Templates.Parsers
{
    [TestFixture]
    public class TileStateParserTests
    {
        private static bool GetValue(Tile tile, byte type)
        {
            switch (type)
            {
                case 1:
                    return tile.HasRedWire;
                case 2:
                    return tile.HasBlueWire;
                case 3:
                    return tile.HasGreenWire;
                case 4:
                    return tile.HasYellowWire;
                case 5:
                    return tile.HasActuator;
                case 6:
                    return tile.IsActuated;
                default:
                    return false;
            }
        }

        [TestCase("red wire", 1, true)]
        [TestCase("!red wire", 1, false)]
        public void Parse(string s, byte expectedType, bool expectedValue)
        {
            var parser = new TileStateParser();
            var tile = new Tile();

            var state = parser.Parse(s);

            Assert.That(state, Is.Not.Null);
            tile = state.Apply(tile);
            Assert.That(GetValue(tile, expectedType), Is.EqualTo(expectedValue));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidTileState_ReturnsNull(string s)
        {
            var parser = new TileStateParser();

            Assert.That(parser.Parse(s), Is.Null);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            var parser = new TileStateParser();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => parser.Parse(null), Throws.ArgumentNullException);
        }
    }
}
