using System;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a tile state.
    /// </summary>
    public sealed class State : ITemplate
    {
        public static readonly State Actuated = new State(6, true);
        public static readonly State Actuator = new State(5, true);
        public static readonly State BlueWire = new State(2, true);
        public static readonly State GreenWire = new State(3, true);
        public static readonly State NotActuated = new State(6, false);
        public static readonly State NotActuator = new State(5, false);
        public static readonly State NotBlueWire = new State(2, false);
        public static readonly State NotGreenWire = new State(3, false);
        public static readonly State NotRedWire = new State(1, false);
        public static readonly State NotYellowWire = new State(4, false);
        public static readonly State RedWire = new State(1, true);
        public static readonly State YellowWire = new State(4, true);

        private readonly byte _type;
        private readonly bool _value;

        private State(byte type, bool value)
        {
            _type = type;
            _value = value;
        }

        /// <summary>
        /// Tries to parse the specified string into a <see cref="State" /> instance. A return value of <c>null</c> indicates
        /// failure.
        /// </summary>
        /// <param name="s">The string to parse, which must not be <c>null</c>.</param>
        /// <returns>The state, or <c>null</c> if parsing failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        /// <remarks>
        /// Strings beginning with "!" will be treated as if they began with "Not " instead.
        /// </remarks>
        [CanBeNull]
        [Pure]
        public static State TryParse([NotNull] string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var s2 = s;
            if (s2.StartsWith("!", StringComparison.OrdinalIgnoreCase))
            {
                s2 = "Not " + s2.Substring(1);
            }
            var field = typeof(State).GetField(s2.ToPascalCase());
            return (State)field?.GetValue(null);
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
