using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class ShapeTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(4)]
        public void Apply(int type)
        {
            var tile = new Tile();
            var shape = new Shape(type);

            tile = shape.Apply(tile);

            Assert.AreEqual(type < 0, tile.HalfBlock);
            Assert.AreEqual(Math.Max(0, type), tile.Slope);
        }

        [TestCase(1)]
        public void GetType(int type)
        {
            var shape = new Shape(type);

            Assert.AreEqual(type, shape.Type);
        }

        [TestCase(-1, 0, false)]
        [TestCase(0, 3, false)]
        [TestCase(-1, -1, true)]
        [TestCase(0, 0, true)]
        [TestCase(3, 3, true)]
        public void Matches(int type, int actualType, bool expected)
        {
            var tile = new Tile
            {
                HalfBlock = actualType < 0,
                Slope = Math.Max(0, actualType)
            };
            var shape = new Shape(type);

            Assert.AreEqual(expected, shape.Matches(tile));
        }

        [TestCase("", false, null)]
        [TestCase("ston", false, null)]
        [TestCase("half", true, -1)]
        [TestCase("NORMAL", true, 0)]
        public void TryParse(string s, bool expected, int? expectedType)
        {
            Assert.AreEqual(expected, Shape.TryParse(s, out var shape));
            Assert.AreEqual(expectedType, shape?.Type);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Shape.TryParse(null, out var _));
        }
    }
}
