using System;
using NUnit.Framework;
using WorldEdit.Core;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void Ctor_NullErrorMessage_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Result(null));
        }

        [TestCase(3)]
        public void From(int value)
        {
            var result = Result.From(value);

            Assert.AreEqual(value, result.Value);
        }

        [TestCase("a")]
        public void FromError(string errorMessage)
        {
            var result = Result.FromError<int>(errorMessage);

            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void FromError_NullErrorMessage_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Result.FromError<int>(null));
        }

        [TestCase("a")]
        public void GetErrorMessage(string errorMessage)
        {
            var result = new Result(errorMessage);

            Assert.AreEqual(errorMessage, result.ErrorMessage);
        }

        [Test]
        public void GetValue()
        {
            var value = new object();
            var result = new Result(value);

            Assert.AreEqual(value, result.Value);
        }

        [TestCase(5)]
        public void GetValue_Generic(int value)
        {
            var result = new Result<int>(value);

            Assert.AreEqual(value, result.Value);
        }

        [Test]
        public void GetWasSuccessful_False()
        {
            var result = new Result("Fail");

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void GetWasSuccessful_True()
        {
            var result = new Result(3);

            Assert.IsTrue(result.WasSuccessful);
        }
    }
}
