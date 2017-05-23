using System;
using NUnit.Framework;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class VectorTests
    {
        [TestCase(-1, 5)]
        [TestCase(67, -5)]
        public void Abs(int x, int y)
        {
            var vector = Vector.Abs(new Vector(x, y));

            Assert.That(vector, Is.EqualTo(new Vector(Math.Abs(x), Math.Abs(y))));
        }

        [TestCase(42, -67, 3, 6, false)]
        [TestCase(42, -67, 42, -67, true)]
        public void Equals(int x, int y, int x2, int y2, bool expected)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.That(vector.Equals(vector2), Is.EqualTo(expected));
        }

        [Test]
        public void Equals_Null_False()
        {
            var vector = new Vector();

            Assert.That(!vector.Equals(null));
        }

        [Test]
        public void Equals_String_False()
        {
            var vector = new Vector();

            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.That(!vector.Equals(""));
        }

        [TestCase(-50, 25)]
        public void GetHashCode_IsConsistent(int x, int y)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x, y);

            Assert.That(vector.GetHashCode(), Is.EqualTo(vector2.GetHashCode()));
        }

        [TestCase(5)]
        public void GetX(int x)
        {
            var vector = new Vector(x, 0);

            Assert.That(vector.X, Is.EqualTo(x));
        }

        [TestCase(10)]
        public void GetY(int y)
        {
            var vector = new Vector(0, y);

            Assert.That(vector.Y, Is.EqualTo(y));
        }

        [TestCase(40, -6, 3, 20, 43, 14)]
        public void OpAddition(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.That(vector + vector2, Is.EqualTo(new Vector(expectedX, expectedY)));
        }

        [TestCase(42, -67, 3, 6, false)]
        [TestCase(42, -67, 42, -67, true)]
        public void OpEquality(int x, int y, int x2, int y2, bool expected)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.That(vector == vector2, Is.EqualTo(expected));
        }

        [TestCase(-5, 10, 27, -16, true)]
        [TestCase(-5, 10, -5, 10, false)]
        public void OpInequality(int x, int y, int x2, int y2, bool expected)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.That(vector != vector2, Is.EqualTo(expected));
        }

        [TestCase(-5, 20, 40, -100, -200)]
        [TestCase(4, -5, 16, -20, 64)]
        public void OpMultiplication(int scalar, int x, int y, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);

            Assert.That(scalar * vector, Is.EqualTo(new Vector(expectedX, expectedY)));
        }

        [TestCase(5, 10, -5, -10)]
        public void OpNegation(int x, int y, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);

            Assert.That(-vector, Is.EqualTo(new Vector(expectedX, expectedY)));
        }

        [TestCase(5, 10, -16, 28, 21, -18)]
        public void OpSubtraction(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.That(vector - vector2, Is.EqualTo(new Vector(expectedX, expectedY)));
        }
    }
}
