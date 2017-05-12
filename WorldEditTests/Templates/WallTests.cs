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

        [TestCase("", false, null)]
        [TestCase("ston", false, null)]
        [TestCase("AIR", true, 0)]
        [TestCase("stone", true, 1)]
        public void TryParse(string s, bool expected, byte? expectedType)
        {
            Assert.AreEqual(expected, Wall.TryParse(s, out var wall));
            Assert.AreEqual(expectedType, wall?.Type);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Wall.TryParse(null, out var _));
        }
    }
}
