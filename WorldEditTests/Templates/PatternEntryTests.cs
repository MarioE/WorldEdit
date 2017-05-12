using System;
using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class PatternEntryTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_NonPositiveWeight_ThrowsArgumentOutOfRangeException(int weight)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new PatternEntry<Block>(new Block(1), weight));
        }

        [Test]
        public void Ctor_NullTemplate_ThrowsArgumenNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PatternEntry<Block>(null, 1));
        }

        [Test]
        public void GetTemplate()
        {
            var template = new Block(1);
            var patternEntry = new PatternEntry<Block>(template, 1);

            Assert.AreEqual(template, patternEntry.Template);
        }

        [TestCase(2)]
        public void GetWeight(int weight)
        {
            var patternEntry = new PatternEntry<Block>(new Block(1), weight);

            Assert.AreEqual(weight, patternEntry.Weight);
        }
    }
}
