using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class StateTests
    {
        private bool GetValue(Tile tile, int type)
        {
            switch (type)
            {
                case 1:
                    return tile.Wire;
                case 2:
                    return tile.Wire2;
                case 3:
                    return tile.Wire3;
                case 4:
                    return tile.Wire4;
                case 5:
                    return tile.Actuator;
                case 6:
                    return tile.Actuated;
                default:
                    return false;
            }
        }

        private Tile SetValue(Tile tile, int type, bool value)
        {
            switch (type)
            {
                case 1:
                    tile.Wire = value;
                    return tile;
                case 2:
                    tile.Wire2 = value;
                    return tile;
                case 3:
                    tile.Wire3 = value;
                    return tile;
                case 4:
                    tile.Wire4 = value;
                    return tile;
                case 5:
                    tile.Actuator = value;
                    return tile;
                case 6:
                    tile.Actuated = value;
                    return tile;
                default:
                    return tile;
            }
        }

        [TestCase(1, true)]
        [TestCase(1, false)]
        [TestCase(3, false)]
        public void Apply(int type, bool value)
        {
            var tile = new Tile();
            var state = new State(type, value);

            tile = state.Apply(tile);

            Assert.AreEqual(value, GetValue(tile, type));
        }

        [TestCase(1)]
        public void GetType(int type)
        {
            var state = new State(type, false);

            Assert.AreEqual(type, state.Type);
        }

        [TestCase(true)]
        public void GetValue(bool value)
        {
            var state = new State(0, value);

            Assert.AreEqual(value, state.Value);
        }

        [TestCase(1, true, 1, false, false)]
        [TestCase(3, true, 2, true, false)]
        [TestCase(1, true, 1, true, true)]
        [TestCase(3, false, 3, false, true)]
        public void Matches(int type, bool value, int actualType, bool actualValue, bool expected)
        {
            var tile = new Tile();
            tile = SetValue(tile, actualType, actualValue);
            var state = new State(type, value);

            Assert.AreEqual(expected, state.Matches(tile));
        }

        [TestCase("red wire", 1, true)]
        [TestCase("!red wire", 1, false)]
        public void Parse(string s, int expectedType, bool expectedValue)
        {
            var result = State.Parse(s);

            Assert.IsTrue(result.WasSuccessful);
            Assert.AreEqual(expectedType, result.Value.Type);
            Assert.AreEqual(expectedValue, result.Value.Value);
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
