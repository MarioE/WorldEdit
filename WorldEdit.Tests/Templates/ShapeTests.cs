using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
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

            Assert.That(GetShape(tile), Is.EqualTo(expectedShape));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(Shape shape, int actualShape, bool expected)
        {
            var tile = new Tile();
            tile = SetShape(tile, actualShape);

            Assert.That(shape.Matches(tile), Is.EqualTo(expected));
        }

        [TestCase("half", -1)]
        [TestCase("NORMAL", 0)]
        public void TryParse(string s, int expectedShape)
        {
            var tile = new Tile();

            var shape = Shape.TryParse(s);

            Assert.That(shape, Is.Not.Null);
            tile = shape.Apply(tile);
            Assert.That(GetShape(tile), Is.EqualTo(expectedShape));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void TryParse_InvalidShape_ReturnsNull(string s)
        {
            Assert.That(Shape.TryParse(s), Is.Null);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => Shape.TryParse(null), Throws.ArgumentNullException);
        }
    }
}
