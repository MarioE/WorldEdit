using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class ColorTests
    {
        private static readonly object[] ApplyTestCases =
        {
            new object[] {Color.Black, (byte)25}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {Color.Black, (byte)25, true},
            new object[] {Color.Black, (byte)24, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(Color wallColor, byte expectedColor)
        {
            var tile = new Tile();

            tile = wallColor.Apply(tile);

            Assert.That(tile.Color, Is.EqualTo(expectedColor));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(Color wallColor, byte actualColor, bool expected)
        {
            var tile = new Tile {Color = actualColor};

            Assert.That(Color.Black.Matches(tile), Is.EqualTo(expected));
        }

        [TestCase("blank", (byte)0)]
        [TestCase("red", (byte)1)]
        public void TryParse(string s, byte expectedColor)
        {
            var tile = new Tile();

            var color = Color.TryParse(s);

            Assert.That(color, Is.Not.Null);
            tile = color.Apply(tile);
            Assert.That(tile.Color, Is.EqualTo(expectedColor));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void TryParse_InvalidColor_ReturnsNull(string s)
        {
            Assert.That(Color.TryParse(s), Is.Null);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => Color.TryParse(null), Throws.ArgumentNullException);
        }
    }
}
