using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
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

            Assert.That(tile.Wall, Is.EqualTo(expectedWall));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(Wall wall, byte actualWall, bool expected)
        {
            var tile = new Tile {Wall = actualWall};

            Assert.That(wall.Matches(tile), Is.EqualTo(expected));
        }

        [TestCase("AIR", 0)]
        [TestCase("stone", 1)]
        [TestCase("4", 4)]
        public void TryParse(string s, byte expectedWall)
        {
            var tile = new Tile();

            var wall = Wall.TryParse(s);

            Assert.That(wall, Is.Not.Null);
            tile = wall.Apply(tile);
            Assert.That(tile.Wall, Is.EqualTo(expectedWall));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void TryParse_InvalidWall_ReturnsNull(string s)
        {
            Assert.That(Wall.TryParse(s), Is.Null);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => Wall.TryParse(null), Throws.ArgumentNullException);
        }
    }
}
