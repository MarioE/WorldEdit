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
            new object[] {"active", "IsActive", true},
            new object[] {"active", "IsActive", false},
            new object[] {"actuator", "HasActuator", true},
            new object[] {"actuator", "HasActuator", false},
            new object[] {"color", "Color", (byte)1},
            new object[] {"halfBrick", "IsHalfBlock", true},
            new object[] {"halfBrick", "IsHalfBlock", false},
            new object[] {"honey", "IsHoney", true},
            new object[] {"honey", "IsHoney", false},
            new object[] {"inActive", "IsActuated", true},
            new object[] {"inActive", "IsActuated", false},
            new object[] {"lava", "IsLava", true},
            new object[] {"lava", "IsLava", false},
            new object[] {"liquidType", "LiquidType", 1},
            new object[] {"slope", "Slope", (byte)1},
            new object[] {"wallColor", "WallColor", (byte)1},
            new object[] {"wire", "HasRedWire", true},
            new object[] {"wire", "HasRedWire", false},
            new object[] {"wire2", "HasBlueWire", true},
            new object[] {"wire2", "HasBlueWire", false},
            new object[] {"wire3", "HasGreenWire", true},
            new object[] {"wire3", "HasGreenWire", false},
            new object[] {"wire4", "HasYellowWire", true},
            new object[] {"wire4", "HasYellowWire", false}
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
