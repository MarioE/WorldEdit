using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class WallTests
    {
        private static readonly object[] ApplyTestCases =
        {
            new object[] {Wall.AdamantiteBeam, (byte)32}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {Wall.AdamantiteBeam, (byte)32, true},
            new object[] {Wall.AdamantiteBeam, (byte)31, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(Wall wall, byte expectedWall)
        {
            var tile = new Tile();

            tile = wall.Apply(tile);

            Assert.AreEqual(expectedWall, tile.Wall);
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(Wall wall, byte actualWall, bool expected)
        {
            var tile = new Tile {Wall = actualWall};

            Assert.AreEqual(expected, wall.Matches(tile));
        }

        [TestCase("AIR", 0)]
        [TestCase("stone", 1)]
        [TestCase("4", 4)]
        public void Parse(string s, byte expectedWall)
        {
            var tile = new Tile();

            var result = Wall.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            tile = result.Value.Apply(tile);
            Assert.AreEqual(expectedWall, tile.Wall);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidWall_ThrowsFormatException(string s)
        {
            var result = Wall.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Wall.Parse(null));
        }
    }
}
