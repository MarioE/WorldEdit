using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class PatternTests
    {
        [Test]
        public void Ctor_EntriesWithNullEntry_ThrowsArgumentException()
        {
            Assert.That(() => new Pattern<Block>(new PatternEntry<Block>[] {null}), Throws.ArgumentException);
        }

        [Test]
        public void Ctor_NullEntries_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new Pattern<Block>(null), Throws.ArgumentNullException);
        }

        [TestCase("air")]
        [TestCase("air,stone")]
        [TestCase("7*air,2*stone")]
        public void TryParse(string s)
        {
            var result = Pattern<Block>.TryParse(s);

            Assert.That(result, Is.Not.Null);
        }

        [TestCase("")]
        [TestCase("ston")]
        [TestCase("*stone")]
        [TestCase("6*stone,4:")]
        public void TryParse_InvalidPattern_ReturnsNull(string s)
        {
            Assert.That(Pattern<Block>.TryParse(s), Is.Null);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => Pattern<Block>.TryParse(null), Throws.ArgumentNullException);
        }
    }
}
