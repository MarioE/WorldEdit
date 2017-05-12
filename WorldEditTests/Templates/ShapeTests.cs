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

        [TestCase("half", -1)]
        [TestCase("NORMAL", 0)]
        public void Parse(string s, int expectedType)
        {
            var result = Shape.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(expectedType, result.Value.Type);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidShape_ThrowsFormatException(string s)
        {
            var result = Shape.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Shape.Parse(null));
        }
    }
}
