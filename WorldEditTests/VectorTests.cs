using System;
using NUnit.Framework;
using WorldEdit;

namespace WorldEditTests
{
    [TestFixture]
    public class VectorTests
    {
        [TestCase(42, -67, 3, 6, false)]
        [TestCase(42, -67, 42, -67, true)]
        public void Equals_False(int x, int y, int x2, int y2, bool expected)
        {
            var vector1 = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.AreEqual(expected, vector1.Equals(vector2));
        }

        [TestCase(4, -6)]
        public void Equals_Null_False(int x, int y)
        {
            var vector = new Vector(x, y);

            Assert.IsFalse(vector.Equals(null));
        }

        [TestCase(4, -5)]
        public void Equals_String_False(int x, int y)
        {
            var vector = new Vector(x, y);

            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.IsFalse(vector.Equals(""));
        }

        [TestCase(-50, 25)]
        public void GetHashCode_IsConsistent(int x, int y)
        {
            var vector1 = new Vector(x, y);
            var vector2 = new Vector(x, y);

            Assert.AreEqual(vector1.GetHashCode(), vector2.GetHashCode());
        }

        [TestCase(5)]
        public void GetX(int x)
        {
            var vector = new Vector(x, 0);

            Assert.AreEqual(x, vector.X);
        }

        [TestCase(10)]
        public void GetY(int y)
        {
            var vector = new Vector(0, y);

            Assert.AreEqual(y, vector.Y);
        }

        [TestCase(40, -6, 3, 20, 43, 14)]
        public void OpAddition(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.AreEqual(new Vector(expectedX, expectedY), vector + vector2);
        }

        [TestCase(42, -67, 3, 6, false)]
        [TestCase(42, -67, 42, -67, true)]
        public void OpEquality(int x, int y, int x2, int y2, bool expected)
        {
            var vector1 = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.AreEqual(expected, vector1 == vector2);
        }

        [TestCase(-5, 10, 27, -16, true)]
        [TestCase(-5, 10, -5, 10, false)]
        public void OpInequality(int x, int y, int x2, int y2, bool expected)
        {
            var vector1 = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.AreEqual(expected, vector1 != vector2);
        }

        [TestCase(-5, 20, 40, -100, -200)]
        [TestCase(4, -5, 16, -20, 64)]
        public void OpMultiplication(int scalar, int x, int y, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);

            Assert.AreEqual(new Vector(expectedX, expectedY), scalar * vector);
        }

        [TestCase(5, 10, -5, -10)]
        public void OpNegation(int x, int y, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);

            Assert.AreEqual(new Vector(expectedX, expectedY), -vector);
        }

        [TestCase(5, 10, -16, 28, 21, -18)]
        public void OpSubtraction(int x, int y, int x2, int y2, int expectedX, int expectedY)
        {
            var vector = new Vector(x, y);
            var vector2 = new Vector(x2, y2);

            Assert.AreEqual(new Vector(expectedX, expectedY), vector - vector2);
        }
    }
}
