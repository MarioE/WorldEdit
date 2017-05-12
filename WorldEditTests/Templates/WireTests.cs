using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Templates;

namespace WorldEditTests.Templates
{
    [TestFixture]
    public class WireTests
    {
        private bool GetWireState(Tile tile, int type)
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
            }
            return false;
        }

        private Tile SetWireState(Tile tile, int type, bool state)
        {
            switch (type)
            {
                case 1:
                    tile.Wire = state;
                    break;
                case 2:
                    tile.Wire2 = state;
                    break;
                case 3:
                    tile.Wire3 = state;
                    break;
                case 4:
                    tile.Wire4 = state;
                    break;
            }
            return tile;
        }

        [TestCase(1, true)]
        [TestCase(1, false)]
        [TestCase(3, false)]
        public void Apply(int type, bool state)
        {
            var tile = new Tile();
            var wire = new Wire(type, state);

            tile = wire.Apply(tile);

            Assert.AreEqual(state, GetWireState(tile, type));
        }

        [TestCase(true)]
        public void GetState(bool state)
        {
            var wire = new Wire(0, state);

            Assert.AreEqual(state, wire.State);
        }

        [TestCase(1)]
        public void GetType(int type)
        {
            var wire = new Wire(type, false);

            Assert.AreEqual(type, wire.Type);
        }

        [TestCase(1, true, 1, false, false)]
        [TestCase(3, true, 2, true, false)]
        [TestCase(1, true, 1, true, true)]
        [TestCase(3, false, 3, false, true)]
        public void Matches(int type, bool state, int actualType, bool actualState, bool expected)
        {
            var tile = new Tile();
            tile = SetWireState(tile, actualType, actualState);
            var wire = new Wire(type, state);

            Assert.AreEqual(expected, wire.Matches(tile));
        }

        [TestCase("", false, null, null)]
        [TestCase("ston", false, null, null)]
        public void TryParse(string s, bool expected, int? expectedType, bool? expectedShape)
        {
            Assert.IsFalse(Wire.TryParse(s, out var wire));
            Assert.AreEqual(expectedType, wire?.Type);
            Assert.AreEqual(expectedShape, wire?.State);
        }

        [Test]
        public void TryParse_NullS_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Wire.TryParse(null, out var _));
        }
    }
}
