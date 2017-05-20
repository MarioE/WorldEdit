using System;
using NUnit.Framework;
using WorldEdit.Core.Templates;

namespace WorldEdit.Core.Tests.Templates
{
    [TestFixture]
    public class StateTests
    {
        private static bool GetValue(Tile tile, byte type)
        {
            switch (type)
            {
                case 1:
                    return tile.HasRedWire;
                case 2:
                    return tile.HasBlueWire;
                case 3:
                    return tile.HasGreenWire;
                case 4:
                    return tile.HasYellowWire;
                case 5:
                    return tile.HasActuator;
                case 6:
                    return tile.IsActuated;
                default:
                    return false;
            }
        }

        private static Tile SetValue(Tile tile, byte type, bool value)
        {
            switch (type)
            {
                case 1:
                    tile.HasRedWire = value;
                    return tile;
                case 2:
                    tile.HasBlueWire = value;
                    return tile;
                case 3:
                    tile.HasGreenWire = value;
                    return tile;
                case 4:
                    tile.HasYellowWire = value;
                    return tile;
                case 5:
                    tile.HasActuator = value;
                    return tile;
                case 6:
                    tile.IsActuated = value;
                    return tile;
                default:
                    return tile;
            }
        }

        private static readonly object[] ApplyTestCases =
        {
            new object[] {State.RedWire, (byte)1, true},
            new object[] {State.NotRedWire, (byte)1, false}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {State.RedWire, (byte)1, true, true},
            new object[] {State.BlueWire, (byte)2, false, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(State state, byte expectedType, bool expectedValue)
        {
            var tile = new Tile();

            tile = state.Apply(tile);

            Assert.AreEqual(expectedValue, GetValue(tile, expectedType));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(State state, byte actualType, bool actualValue, bool expected)
        {
            var tile = new Tile();
            tile = SetValue(tile, actualType, actualValue);

            Assert.AreEqual(expected, state.Matches(tile));
        }

        [TestCase("red wire", 1, true)]
        [TestCase("!red wire", 1, false)]
        public void Parse(string s, byte expectedType, bool expectedValue)
        {
            var tile = new Tile();

            var result = State.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            tile = result.Value.Apply(tile);
            Assert.AreEqual(expectedValue, GetValue(tile, expectedType));
        }

        [TestCase("")]
        [TestCase("ston")]
        public void Parse_InvalidState_ThrowsFormatException(string s)
        {
            var result = State.Parse(s);

            Assert.IsFalse(result.WasSuccessful);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => State.Parse(null));
        }
    }
}
