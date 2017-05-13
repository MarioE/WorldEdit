using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class ColorTests
    {
        [TestCase(1)]
        public void Apply(byte type)
        {
            var tile = new Tile();
            var color = new Color(type);

            tile = color.Apply(tile);

            Assert.AreEqual(type, tile.Color);
        }
        
        [TestCase(1)]
        public void GetType(byte type)
        {
            var color = new Color(type);

            Assert.AreEqual(type, color.Type);
        }

        [TestCase(1, 2, false)]
        [TestCase(1, 1, true)]
        public void Matches(byte type, byte actualType, bool expected)
        {
            var tile = new Tile();
            tile.Color = actualType;
            var color = new Color(type);

            Assert.AreEqual(expected, color.Matches(tile));
        }

        [TestCase("blank", 0)]
        [TestCase("red", 1)]
        public void Parse(string s, int expectedType)
        {
            var result = Color.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(expectedType, result.Value.Type);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidColor_ThrowsFormatException(string s)
        {
            var result = Color.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Color.Parse(null));
        }
    }
}
