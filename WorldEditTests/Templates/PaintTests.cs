using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class PaintTests
    {
        [TestCase(1, false)]
        [TestCase(1, true)]
        public void Apply(byte type, bool targetWalls)
        {
            var tile = new Tile();
            var paint = new Paint(type, targetWalls);

            tile = paint.Apply(tile);

            Assert.AreEqual(type, targetWalls ? tile.WallColor : tile.Color);
        }

        [TestCase(true)]
        public void GetTargetWalls(bool targetWalls)
        {
            var paint = new Paint(0, targetWalls);

            Assert.AreEqual(targetWalls, paint.TargetWalls);
        }

        [TestCase(1)]
        public void GetType(byte type)
        {
            var paint = new Paint(type, false);

            Assert.AreEqual(type, paint.Type);
        }

        [TestCase(1, true, 1, false, false)]
        [TestCase(1, false, 0, false, false)]
        [TestCase(1, true, 1, true, true)]
        [TestCase(1, false, 1, false, true)]
        public void Matches(byte type, bool targetWalls, byte actualType, bool actualTargetWalls, bool expected)
        {
            var tile = new Tile();
            if (actualTargetWalls)
            {
                tile.WallColor = actualType;
            }
            else
            {
                tile.Color = actualType;
            }
            var paint = new Paint(type, targetWalls);

            Assert.AreEqual(expected, paint.Matches(tile));
        }

        [TestCase("blank", 0)]
        [TestCase("red", 1)]
        public void Parse(string s, int expectedType)
        {
            var result = Paint.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(expectedType, result.Value.Type);
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidPaint_ThrowsFormatException(string s)
        {
            var result = Paint.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Paint.Parse(null));
        }
    }
}
