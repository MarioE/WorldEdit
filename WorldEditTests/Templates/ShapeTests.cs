using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class ShapeTests
    {
        private static int GetShape(Tile tile) => tile.IsHalfBlock ? -1 : tile.Slope;

        private static Tile SetShape(Tile tile, int shape)
        {
            if (shape < 0)
            {
                tile.IsHalfBlock = true;
            }
            else
            {
                tile.Slope = shape;
            }
            return tile;
        }

        private static readonly object[] ApplyTestCases =
        {
            new object[] {Shape.Half, -1},
            new object[] {Shape.Normal, 0},
            new object[] {Shape.TopLeft, 2}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {Shape.Half, 1, false},
            new object[] {Shape.Half, -1, true},
            new object[] {Shape.TopLeft, 2, true},
            new object[] {Shape.TopLeft, 3, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(Shape shape, int expectedShape)
        {
            var tile = new Tile();

            tile = shape.Apply(tile);

            Assert.AreEqual(expectedShape, GetShape(tile));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(Shape shape, int actualShape, bool expected)
        {
            var tile = new Tile();
            tile = SetShape(tile, actualShape);

            Assert.AreEqual(expected, shape.Matches(tile));
        }

        [TestCase("half", -1)]
        [TestCase("NORMAL", 0)]
        public void Parse(string s, int expectedShape)
        {
            var tile = new Tile();

            var result = Shape.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            tile = result.Value.Apply(tile);
            Assert.AreEqual(expectedShape, GetShape(tile));
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
