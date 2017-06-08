using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class BlockShapeTests
    {
        private static int GetBlockShape(Tile tile) => tile.IsHalfBlock ? -1 : tile.Slope;

        private static Tile SetBlockShape(Tile tile, int shape)
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
            new object[] {BlockShape.Half, -1},
            new object[] {BlockShape.Normal, 0},
            new object[] {BlockShape.TopLeft, 2}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {BlockShape.Half, 1, false},
            new object[] {BlockShape.Half, -1, true},
            new object[] {BlockShape.TopLeft, 2, true},
            new object[] {BlockShape.TopLeft, 3, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(BlockShape blockShape, int expectedBlockShape)
        {
            var tile = new Tile();

            tile = blockShape.Apply(tile);

            Assert.That(GetBlockShape(tile), Is.EqualTo(expectedBlockShape));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(BlockShape blockShape, int actualBlockShape, bool expected)
        {
            var tile = new Tile();
            tile = SetBlockShape(tile, actualBlockShape);

            Assert.That(blockShape.Matches(tile), Is.EqualTo(expected));
        }
    }
}
