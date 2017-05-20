using System;
using NUnit.Framework;
using WorldEdit.Core.Templates;

namespace WorldEdit.Core.Tests.Templates
{
    [TestFixture]
    public class PatternTests
    {
        [Test]
        public void Ctor_NullEntries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Pattern<Block>(null));
        }

        [TestCase("air")]
        [TestCase("air,stone")]
        [TestCase("7*air,2*stone")]
        public void Parse(string s)
        {
            var result = Pattern<Block>.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
        }

        [TestCase("")]
        [TestCase("ston")]
        [TestCase("*stone")]
        [TestCase("6*stone,4:")]
        public void Parse_InvalidPattern(string s)
        {
            var result = Pattern<Block>.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Pattern<Block>.Parse(null));
        }
    }
}
