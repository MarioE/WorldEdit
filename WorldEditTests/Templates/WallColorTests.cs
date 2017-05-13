using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class WallColorTests
    {
        [TestCase(1)]
        public void Apply(byte type)
        {
            var tile = new Tile();
            var wallColor = new WallColor(type);

            tile = wallColor.Apply(tile);

            Assert.AreEqual(type, tile.WallColor);
        }

        [TestCase(1)]
        public void GetType(byte type)
        {
            var wallColor = new WallColor(type);

            Assert.AreEqual(type, wallColor.Type);
        }

        [TestCase(1, 2, false)]
        [TestCase(1, 1, true)]
        public void Matches(byte type, byte actualType, bool expected)
        {
            var tile = new Tile();
            tile.WallColor = actualType;
            var wallColor = new WallColor(type);

            Assert.AreEqual(expected, wallColor.Matches(tile));
        }

        [TestCase("blank", 0)]
        [TestCase("red", 1)]
        public void Parse(string s, int expectedType)
        {
            var result = WallColor.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(expectedType, result.Value.Type);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidWallColor_ThrowsFormatException(string s)
        {
            var result = WallColor.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => WallColor.Parse(null));
        }
    }
}
