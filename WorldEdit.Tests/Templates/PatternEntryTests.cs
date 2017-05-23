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
            Assert.That(() => new PatternEntry<Block>(null, 1), Throws.ArgumentNullException);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_WeightNotPositive_ThrowsArgumentOutOfRangeException(int weight)
        {
            Assert.That(() => new PatternEntry<Block>(Block.Water, weight),
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GetTemplate()
        {
            var template = Block.Water;
            var patternEntry = new PatternEntry<Block>(template, 1);

            Assert.That(patternEntry.Template, Is.EqualTo(template));
        }

        [TestCase(2)]
        public void GetWeight(int weight)
        {
            var patternEntry = new PatternEntry<Block>(Block.Water, weight);

            Assert.That(patternEntry.Weight, Is.EqualTo(weight));
        }
    }
}
