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

        [TestCase("blank", (byte)0)]
        [TestCase("red", (byte)1)]
        public void TryParse(string s, byte expectedColor)
        {
            var tile = new Tile();

            var wallColor = WallColor.TryParse(s);

            Assert.That(wallColor, Is.Not.Null);
            tile = wallColor.Apply(tile);
            Assert.That(tile.WallColor, Is.EqualTo(expectedColor));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void TryParse_InvalidWallColor_ReturnsNull(string s)
        {
            Assert.That(WallColor.TryParse(s), Is.Null);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => WallColor.TryParse(null), Throws.ArgumentNullException);
        }
    }
}
