using System;
using NUnit.Framework;
using WorldEdit;

namespace WorldEditTests
{
    [TestFixture]
    public class ParsingResultTests
    {
        [Test]
        public void Ctor_NullErrorMessage_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ParsingResult(null));
        }

        [TestCase(3)]
        public void From(int value)
        {
            var result = ParsingResult.From(value);

            Assert.AreEqual(value, result.Value);
        }

        [TestCase("a")]
        public void FromError(string errorMessage)
        {
            var result = ParsingResult.FromError<int>(errorMessage);

            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void FromError_NullErrorMessage_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ParsingResult.FromError<int>(null));
        }

        [TestCase("a")]
        public void GetErrorMessage(string errorMessage)
        {
            var result = new ParsingResult(errorMessage);

            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GetValue()
        {
            var value = new object();
            var result = new ParsingResult(value);

            Assert.AreEqual(value, result.Value);
        }

        [TestCase(5)]
        public void GetValue_Generic(int value)
        {
            var result = new ParsingResult<int>(value);

            Assert.AreEqual(value, result.Value);
        }

        [Test]
        public void GetWasSuccessful_False()
        {
            var result = new ParsingResult("Fail");

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void GetWasSuccessful_True()
        {
            var result = new ParsingResult(3);

            Assert.IsTrue(result.WasSuccessful);
        }
    }
}
