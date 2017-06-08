using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class BlockColorTests
    {
        private static readonly object[] ApplyTestCases =
        {
            new object[] {BlockColor.Black, (byte)25}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {BlockColor.Black, (byte)25, true},
            new object[] {BlockColor.Black, (byte)24, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(BlockColor blockColor, byte expectedBlockColor)
        {
            var tile = new Tile();

            tile = blockColor.Apply(tile);

            Assert.That(tile.BlockColor, Is.EqualTo(expectedBlockColor));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(BlockColor blockColor, byte actualBlockColor, bool expected)
        {
            var tile = new Tile {BlockColor = actualBlockColor};

            Assert.That(BlockColor.Black.Matches(tile), Is.EqualTo(expected));
        }
    }
}
