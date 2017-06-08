using NUnit.Framework;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Templates
{
    [TestFixture]
    public class TileStateTests
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
            new object[] {TileState.RedWire, (byte)1, true},
            new object[] {TileState.NotRedWire, (byte)1, false},
            new object[] {TileState.BlueWire, (byte)2, true},
            new object[] {TileState.NotBlueWire, (byte)2, false},
            new object[] {TileState.GreenWire, (byte)3, true},
            new object[] {TileState.NotGreenWire, (byte)3, false},
            new object[] {TileState.YellowWire, (byte)4, true},
            new object[] {TileState.NotYellowWire, (byte)4, false},
            new object[] {TileState.Actuator, (byte)5, true},
            new object[] {TileState.NotActuator, (byte)5, false},
            new object[] {TileState.Actuated, (byte)6, true},
            new object[] {TileState.NotActuated, (byte)6, false}
        };

        private static readonly object[] MatchesTestCases =
        {
            new object[] {TileState.RedWire, (byte)1, true, true},
            new object[] {TileState.BlueWire, (byte)2, false, false},
            new object[] {TileState.GreenWire, (byte)2, true, false},
            new object[] {TileState.NotYellowWire, (byte)4, true, false},
            new object[] {TileState.Actuator, (byte)5, false, false},
            new object[] {TileState.NotActuated, (byte)6, true, false}
        };

        [TestCaseSource(nameof(ApplyTestCases))]
        public void Apply(TileState tileState, byte expectedType, bool expectedValue)
        {
            var tile = new Tile();

            tile = tileState.Apply(tile);

            Assert.That(GetValue(tile, expectedType), Is.EqualTo(expectedValue));
        }

        [TestCaseSource(nameof(MatchesTestCases))]
        public void Matches(TileState tileState, byte actualType, bool actualValue, bool expected)
        {
            var tile = new Tile();
            tile = SetValue(tile, actualType, actualValue);

            Assert.That(tileState.Matches(tile), Is.EqualTo(expected));
        }
    }
}
