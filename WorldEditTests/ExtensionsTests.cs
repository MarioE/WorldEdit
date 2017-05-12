using System;
using NUnit.Framework;
using WorldEdit;

namespace WorldEditTests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [TestCase("a", "A")]
        [TestCase("a b c", "A B C")]
        public void ToTitleCase(string s, string expected)
        {
            Assert.AreEqual(expected, s.ToTitleCase());
        }

        [Test]
        public void ToTitleCase_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).ToTitleCase());
        }
    }
}
