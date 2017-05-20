using System;
using NUnit.Framework;
using WorldEdit.Core.Templates;

namespace WorldEdit.Core.Tests.Templates
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

            Assert.AreEqual(expectedColor, tile.WallColor);
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(WallColor wallColor, byte actualColor, bool expected)
        {
            var tile = new Tile();
            tile.WallColor = actualColor;

            Assert.AreEqual(expected, WallColor.Black.Matches(tile));
        }

        [TestCase("blank", (byte)0)]
        [TestCase("red", (byte)1)]
        public void Parse(string s, byte expectedColor)
        {
            var tile = new Tile();

            var result = WallColor.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            tile = result.Value.Apply(tile);
            Assert.AreEqual(expectedColor, tile.WallColor);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidWallColor_ThrowsFormatException(string s)
        {
            var result = WallColor.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => WallColor.Parse(null));
        }
    }
}
