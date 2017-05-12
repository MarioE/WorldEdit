using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class WallTests
    {
        [TestCase(1)]
        public void Apply(byte type)
        {
            var tile = new Tile();
            var wall = new Wall(type);

            tile = wall.Apply(tile);

            Assert.AreEqual(type, tile.Wall);
        }

        [TestCase(1)]
        public void GetType(byte type)
        {
            var wall = new Wall(type);

            Assert.AreEqual(type, wall.Type);
        }

        [TestCase(1, 0, false)]
        [TestCase(1, 1, true)]
        public void Matches(byte type, byte actualType, bool expected)
        {
            var tile = new Tile {Wall = actualType};
            var wall = new Wall(type);

            Assert.AreEqual(expected, wall.Matches(tile));
        }

        [TestCase("AIR", 0)]
        [TestCase("stone", 1)]
        public void Parse(string s, byte expectedType)
        {
            var result = Wall.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(expectedType, result.Value.Type);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidWall_ThrowsFormatException(string s)
        {
            var result = Wall.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Wall.Parse(null));
        }
    }
}
