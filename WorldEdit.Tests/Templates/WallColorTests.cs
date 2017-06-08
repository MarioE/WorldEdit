using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class WallColorTests
    {
        private static readonly object[] ApplyTestCases =
        {
            new object[] {WallColor.Black, (byte)25}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {WallColor.Black, (byte)25, true},
            new object[] {WallColor.Black, (byte)24, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(WallColor wallColor, byte expectedColor)
        {
            var tile = new Tile();

            tile = wallColor.Apply(tile);

            Assert.That(tile.WallColor, Is.EqualTo(expectedColor));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(WallColor wallColor, byte actualColor, bool expected)
        {
            var tile = new Tile
            {
                WallColor = actualColor
            };
            Assert.That(WallColor.Black.Matches(tile), Is.EqualTo(expected));
        }
    }
}
