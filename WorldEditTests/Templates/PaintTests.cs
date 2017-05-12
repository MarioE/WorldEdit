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

        [TestCase("", false, null)]
        [TestCase("ston", false, null)]
        [TestCase("blank", true, 0)]
        [TestCase("red", true, 1)]
        public void TryParse(string s, bool expected, int? expectedType)
        {
            Assert.AreEqual(expected, Paint.TryParse(s, out var paint));
            Assert.AreEqual(expectedType, paint?.Type);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Paint.TryParse(null, out var _));
        }
    }
}
