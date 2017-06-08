using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    ///     Represents a tile state.
    /// </summary>
    [NoReorder]
    public sealed class TileState : ITemplate
    {
#pragma warning disable 1591
        public static readonly TileState RedWire = new TileState(1, true);
        public static readonly TileState NotRedWire = new TileState(1, false);
        public static readonly TileState BlueWire = new TileState(2, true);
        public static readonly TileState NotBlueWire = new TileState(2, false);
        public static readonly TileState GreenWire = new TileState(3, true);
        public static readonly TileState NotGreenWire = new TileState(3, false);
        public static readonly TileState YellowWire = new TileState(4, true);
        public static readonly TileState NotYellowWire = new TileState(4, false);
        public static readonly TileState Actuator = new TileState(5, true);
        public static readonly TileState NotActuator = new TileState(5, false);
        public static readonly TileState Actuated = new TileState(6, true);
        public static readonly TileState NotActuated = new TileState(6, false);
#pragma warning restore 1591

        private readonly byte _type;
        private readonly bool _value;

        private TileState(byte type, bool value)
        {
            _type = type;
            _value = value;
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            switch (_type)
            {
                case 1:
                    tile.HasRedWire = _value;
                    return tile;
                case 2:
                    tile.HasBlueWire = _value;
                    return tile;
                case 3:
                    tile.HasGreenWire = _value;
                    return tile;
                case 4:
                    tile.HasYellowWire = _value;
                    return tile;
                case 5:
                    tile.HasActuator = _value;
                    return tile;
                case 6:
                    tile.IsActuated = _value;
                    return tile;
                default:
                    return tile;
            }
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            switch (_type)
            {
                case 1:
                    return tile.HasRedWire == _value;
                case 2:
                    return tile.HasBlueWire == _value;
                case 3:
                    return tile.HasGreenWire == _value;
                case 4:
                    return tile.HasYellowWire == _value;
                case 5:
                    return tile.HasActuator == _value;
                case 6:
                    return tile.IsActuated == _value;
                default:
                    return false;
            }
        }
    }
}
