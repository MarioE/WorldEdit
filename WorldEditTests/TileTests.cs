using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using TTile = Terraria.Tile;

namespace WorldEditTests
{
    [TestFixture]
    public class TileTests
    {
        private static readonly object[] PackedPropertyTestCases =
        {
            new object[] {"active", "Active", true},
            new object[] {"active", "Active", false},
            new object[] {"actuator", "Actuator", true},
            new object[] {"actuator", "Actuator", false},
            new object[] {"color", "Color", (byte)1},
            new object[] {"halfBrick", "HalfBlock", true},
            new object[] {"halfBrick", "HalfBlock", false},
            new object[] {"honey", "Honey", true},
            new object[] {"honey", "Honey", false},
            new object[] {"inActive", "Actuated", true},
            new object[] {"inActive", "Actuated", false},
            new object[] {"lava", "Lava", true},
            new object[] {"lava", "Lava", false},
            new object[] {"liquidType", "LiquidType", 1},
            new object[] {"slope", "Slope", (byte)1},
            new object[] {"wallColor", "WallColor", (byte)1},
            new object[] {"wire", "Wire", true},
            new object[] {"wire", "Wire", false},
            new object[] {"wire2", "Wire2", true},
            new object[] {"wire2", "Wire2", false},
            new object[] {"wire3", "Wire3", true},
            new object[] {"wire3", "Wire3", false},
            new object[] {"wire4", "Wire4", true},
            new object[] {"wire4", "Wire4", false}
        };

        private static readonly object[] PropertyTestCases =
        {
            new object[] {"frameX", "FrameX", (short)1},
            new object[] {"frameY", "FrameY", (short)1},
            new object[] {"liquid", "Liquid", (byte)1},
            new object[] {"type", "Type", (ushort)1},
            new object[] {"wall", "Wall", (byte)1}
        };

        [TestCase(1, 2, 3, (ushort)4, 5)]
        public void Ctor(short frameX, short frameY, byte liquid, ushort type, byte wall)
        {
            var itile = new TTile
            {
                frameX = frameX,
                frameY = frameY,
                liquid = liquid,
                type = type,
                wall = wall
            };

            var tile = new Tile(itile);

            Assert.AreEqual(frameX, tile.FrameX);
            Assert.AreEqual(frameY, tile.FrameY);
            Assert.AreEqual(liquid, tile.Liquid);
            Assert.AreEqual(type, tile.Type);
            Assert.AreEqual(wall, tile.Wall);
        }

        [TestCaseSource(nameof(PackedPropertyTestCases))]
        public void GetPackedProperty(string methodName, string propertyName, object value)
        {
            var itile = new TTile();
            typeof(ITile).GetMethod(methodName, new[] {value.GetType()}).Invoke(itile, new[] {value});
            var tile = new Tile(itile);

            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(value, typeof(Tile).GetProperty(propertyName).GetValue(tile));
        }

        [TestCaseSource(nameof(PropertyTestCases))]
        public void GetProperty(string itilePropertyName, string propertyName, object value)
        {
            var itile = new TTile();
            // ReSharper disable once PossibleNullReferenceException
            typeof(ITile).GetProperty(itilePropertyName).SetValue(itile, value);
            var tile = new Tile(itile);

            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(value, typeof(Tile).GetProperty(propertyName).GetValue(tile));
        }

        [TestCaseSource(nameof(PackedPropertyTestCases))]
        public void SetPackedProperty(string methodName, string propertyName, object value)
        {
            object tile = new Tile();

            // ReSharper disable once PossibleNullReferenceException
            typeof(Tile).GetProperty(propertyName).SetValue(tile, value);

            var itile = ((Tile)tile).ToITile();
            Assert.AreEqual(value, typeof(ITile).GetMethod(methodName, new Type[0]).Invoke(itile, new object[0]));
        }

        [TestCaseSource(nameof(PropertyTestCases))]
        public void SetProperty(string itilePropertyName, string propertyName, object value)
        {
            object tile = new Tile();

            // ReSharper disable once PossibleNullReferenceException
            typeof(Tile).GetProperty(propertyName).SetValue(tile, value);

            var itile = ((Tile)tile).ToITile();
            // ReSharper disable once PossibleNullReferenceException
            Assert.AreEqual(value, typeof(ITile).GetProperty(itilePropertyName).GetValue(itile));
        }

        [TestCase(1, 2, 3, (ushort)4, 5)]
        public void ToITile(short frameX, short frameY, byte liquid, ushort type, byte wall)
        {
            var tile = new Tile
            {
                FrameX = frameX,
                FrameY = frameY,
                Liquid = liquid,
                Type = type,
                Wall = wall
            };

            var itile = tile.ToITile();

            Assert.AreEqual(frameX, itile.frameX);
            Assert.AreEqual(frameY, itile.frameY);
            Assert.AreEqual(liquid, itile.liquid);
            Assert.AreEqual(type, itile.type);
            Assert.AreEqual(wall, itile.wall);
        }
    }
}
