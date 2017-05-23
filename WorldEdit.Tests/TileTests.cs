using NUnit.Framework;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class TileTests
    {
        [Test]
        public void Equals_False()
        {
            var tile = new Tile {FrameX = 1, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};
            var tile2 = new Tile {FrameX = 0, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};

            Assert.That(!tile.Equals(tile2));
        }

        [Test]
        public void Equals_Null_False()
        {
            var tile = new Tile();

            Assert.That(!tile.Equals(null));
        }

        [Test]
        public void Equals_String_False()
        {
            var tile = new Tile();

            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.That(!tile.Equals(""));
        }

        [Test]
        public void Equals_True()
        {
            var tile = new Tile {FrameX = 1, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};
            var tile2 = tile;

            Assert.That(tile.Equals(tile2));
        }

        [TestCase(nameof(Tile.Color), (byte)1)]
        [TestCase(nameof(Tile.FrameX), (short)1)]
        [TestCase(nameof(Tile.FrameY), (short)1)]
        [TestCase(nameof(Tile.HasActuator), true)]
        [TestCase(nameof(Tile.HasActuator), false)]
        [TestCase(nameof(Tile.HasBlueWire), true)]
        [TestCase(nameof(Tile.HasBlueWire), false)]
        [TestCase(nameof(Tile.HasGreenWire), true)]
        [TestCase(nameof(Tile.HasGreenWire), false)]
        [TestCase(nameof(Tile.HasRedWire), true)]
        [TestCase(nameof(Tile.HasRedWire), false)]
        [TestCase(nameof(Tile.HasYellowWire), true)]
        [TestCase(nameof(Tile.HasYellowWire), false)]
        [TestCase(nameof(Tile.IsActive), true)]
        [TestCase(nameof(Tile.IsActive), false)]
        [TestCase(nameof(Tile.IsActuated), true)]
        [TestCase(nameof(Tile.IsActuated), false)]
        [TestCase(nameof(Tile.IsHalfBlock), true)]
        [TestCase(nameof(Tile.IsHalfBlock), false)]
        [TestCase(nameof(Tile.IsHoney), true)]
        [TestCase(nameof(Tile.IsHoney), false)]
        [TestCase(nameof(Tile.IsLava), true)]
        [TestCase(nameof(Tile.IsLava), false)]
        [TestCase(nameof(Tile.Liquid), (byte)1)]
        [TestCase(nameof(Tile.LiquidType), 1)]
        [TestCase(nameof(Tile.Slope), (byte)1)]
        [TestCase(nameof(Tile.Type), (ushort)1)]
        [TestCase(nameof(Tile.Wall), (byte)1)]
        [TestCase(nameof(Tile.WallColor), (byte)1)]
        public void GetSetProperty(string propertyName, object value)
        {
            object tile = new Tile();
            var property = typeof(Tile).GetProperty(propertyName);

            Assert.That(property, Is.Not.Null);
            property.SetValue(tile, value);

            Assert.That(property.GetValue(tile), Is.EqualTo(value));
        }

        [Test]
        public void OpEquality_False()
        {
            var tile = new Tile {FrameX = 1, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};
            var tile2 = new Tile {FrameX = 0, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};

            Assert.That(!(tile == tile2));
        }

        [Test]
        public void OpEquality_True()
        {
            var tile = new Tile {FrameX = 1, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};
            var tile2 = tile;

            Assert.That(tile == tile2);
        }

        [Test]
        public void OpInequality_False()
        {
            var tile = new Tile {FrameX = 1, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};
            var tile2 = tile;

            Assert.That(!(tile != tile2));
        }

        [Test]
        public void OpInequality_True()
        {
            var tile = new Tile {FrameX = 1, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};
            var tile2 = new Tile {FrameX = 0, FrameY = 2, Liquid = 3, Type = 4, Wall = 5};

            Assert.That(tile != tile2);
        }
    }
}
