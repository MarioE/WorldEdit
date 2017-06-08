using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class WallTypeTests
    {
        private static readonly object[] ApplyTestCases =
        {
            new object[] {WallType.AdamantiteBeam, (byte)32}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {WallType.AdamantiteBeam, (byte)32, true},
            new object[] {WallType.AdamantiteBeam, (byte)31, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(WallType wall, byte expectedWallType)
        {
            var tile = new Tile();

            tile = wall.Apply(tile);

            Assert.That(tile.WallId, Is.EqualTo(expectedWallType));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(WallType wall, byte actualWallType, bool expected)
        {
            var tile = new Tile {WallId = actualWallType};

            Assert.That(wall.Matches(tile), Is.EqualTo(expected));
        }
    }
}
