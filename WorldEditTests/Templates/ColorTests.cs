using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
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

            Assert.AreEqual(expectedColor, tile.Color);
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(Color wallColor, byte actualColor, bool expected)
        {
            var tile = new Tile();
            tile.Color = actualColor;

            Assert.AreEqual(expected, Color.Black.Matches(tile));
        }

        [TestCase("blank", (byte)0)]
        [TestCase("red", (byte)1)]
        public void Parse(string s, byte expectedColor)
        {
            var tile = new Tile();

            var result = Color.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            tile = result.Value.Apply(tile);
            Assert.AreEqual(expectedColor, tile.Color);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidColor_ThrowsFormatException(string s)
        {
            var result = Color.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Color.Parse(null));
        }
    }
}
