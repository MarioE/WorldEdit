using System;
using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class PatternEntryTests
    {
        [Test]
        public void Ctor_NullTemplate_ThrowsArgumenNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new PatternEntry(null, 1), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_WeightNotPositive_ThrowsArgumentOutOfRangeException(int weight)
        {
            Assert.That(() => new PatternEntry(BlockType.Water, weight),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Template()
        {
            var template = BlockType.Water;
            var patternEntry = new PatternEntry(template, 1);

            Assert.That(patternEntry.Template, Is.EqualTo(template));
        }

        [TestCase(2)]
        public void Weight(int weight)
        {
            var patternEntry = new PatternEntry(BlockType.Water, weight);

            Assert.That(patternEntry.Weight, Is.EqualTo(weight));
        }
    }
}
