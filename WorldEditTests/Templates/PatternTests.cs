using System;
using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class PatternTests
    {
        [Test]
        public void Ctor_NullEntries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Pattern<Block>(null));
        }

        [TestCase("")]
        [TestCase("ston")]
        [TestCase("*stone")]
        [TestCase("6*stone,4:")]
        public void Parse_InvalidPattern_ThrowsFormatException(string s)
        {
            var result = Pattern<Block>.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_MultipleEntries()
        {
            var result = Pattern<Block>.Parse("air,stone");

            Assert.IsTrue(result.WasSuccessful);
            var entries = result.Value.Entries;
            Assert.AreEqual(2, entries.Count);
            Assert.AreEqual(-1, entries[0].Template.Type);
            Assert.AreEqual(1, entries[0].Weight);
            Assert.AreEqual(1, entries[1].Template.Type);
            Assert.AreEqual(1, entries[1].Weight);
        }

        [Test]
        public void Parse_MultipleEntriesCustomWeights()
        {
            var result = Pattern<Block>.Parse("7*air,2*stone");

            Assert.IsTrue(result.WasSuccessful);
            var entries = result.Value.Entries;
            Assert.AreEqual(2, entries.Count);
            Assert.AreEqual(-1, entries[0].Template.Type);
            Assert.AreEqual(7, entries[0].Weight);
            Assert.AreEqual(1, entries[1].Template.Type);
            Assert.AreEqual(2, entries[1].Weight);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Pattern<Block>.Parse(null));
        }

        [Test]
        public void Parse_OneEntry()
        {
            var result = Pattern<Block>.Parse("air");

            Assert.IsTrue(result.WasSuccessful);
            var entries = result.Value.Entries;
            Assert.AreEqual(1, entries.Count);
            Assert.AreEqual(-1, entries[0].Template.Type);
            Assert.AreEqual(1, entries[0].Weight);
        }
    }
}
