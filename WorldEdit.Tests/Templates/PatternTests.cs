using System.Collections.Generic;
using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class PatternTests
    {
        [Test]
        public void Apply()
        {
            var pattern = new Pattern(new[] {new PatternEntry(BlockType.Dirt, 1)});
            var tile = new Tile();

            tile = pattern.Apply(tile);

            Assert.That(BlockType.Dirt.Matches(tile));
        }

        [Test]
        public void Ctor_NullEntries_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new Pattern(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_NullEntry_ThrowsArgumentException()
        {
            Assert.That(() => new Pattern(new List<PatternEntry> {null}), Throws.ArgumentException);
        }

        [Test]
        public void Matches()
        {
            var pattern = new Pattern(new[]
                {new PatternEntry(BlockType.Stone, 1), new PatternEntry(BlockType.Dirt, 1)});
            var tile = new Tile();

            tile = BlockType.Dirt.Apply(tile);

            Assert.That(pattern.Matches(tile));
        }
    }
}
