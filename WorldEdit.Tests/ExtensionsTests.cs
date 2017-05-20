using System;
using NUnit.Framework;
using WorldEdit.Core;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [TestCase("       a   ab", "aab")]
        [TestCase("     \n\n\n\t a  \t\r\n ab", "aab")]
        public void RemoveWhitespace(string s, string expected)
        {
            Assert.AreEqual(expected, s.RemoveWhitespace());
        }

        [Test]
        public void RemoveWhitespace_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).RemoveWhitespace());
        }

        [TestCase("asdf", "Asdf")]
        [TestCase("as df", "AsDf")]
        public void ToPascalCase(string s, string expected)
        {
            Assert.AreEqual(expected, s.ToPascalCase());
        }

        [Test]
        public void ToPascalCase_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).ToPascalCase());
        }
    }
}
