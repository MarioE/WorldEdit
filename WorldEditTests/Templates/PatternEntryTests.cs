using System;
using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class PatternEntryTests
    {
        [Test]
        public void Ctor_NullTemplate_ThrowsArgumenNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PatternEntry<Block>(null, 1));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_WeightNotPositive_ThrowsArgumentOutOfRangeException(int weight)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PatternEntry<Block>(Block.Water, weight));
        }

        [Test]
        public void GetTemplate()
        {
            var template = Block.Water;
            var patternEntry = new PatternEntry<Block>(template, 1);

            Assert.AreEqual(template, patternEntry.Template);
        }

        [TestCase(2)]
        public void GetWeight(int weight)
        {
            var patternEntry = new PatternEntry<Block>(Block.Water, weight);

            Assert.AreEqual(weight, patternEntry.Weight);
        }
    }
}
