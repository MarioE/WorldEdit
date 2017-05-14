using System;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a tile state.
    /// </summary>
    public class State : ITemplate
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

        /// <summary>
        /// Initializes a new instance of the <see cref="State" /> with the specified type and value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        public State(int type, bool value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public bool Value { get; }

        /// <summary>
        /// Parses the specified string into a state.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<State> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var s2 = s.Trim();
            if (s2.StartsWith("!"))
            {
                s2 = "Not " + s2.Substring(1);
            }

            var field = typeof(State).GetField(s2.ToPascalCase());
            if (field == null)
            {
                return ParsingResult.FromError<State>($"Invalid state '{s}'.");
            }
            return ParsingResult.From((State)field.GetValue(null));
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            switch (Type)
            {
                case 1:
                    tile.Wire = Value;
                    return tile;
                case 2:
                    tile.Wire2 = Value;
                    return tile;
                case 3:
                    tile.Wire3 = Value;
                    return tile;
                case 4:
                    tile.Wire4 = Value;
                    return tile;
                case 5:
                    tile.Actuator = Value;
                    return tile;
                case 6:
                    tile.Actuated = Value;
                    return tile;
                default:
                    return tile;
            }
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            switch (Type)
            {
                case 1:
                    return tile.Wire == Value;
                case 2:
                    return tile.Wire2 == Value;
                case 3:
                    return tile.Wire3 == Value;
                case 4:
                    return tile.Wire4 == Value;
                case 5:
                    return tile.Actuator == Value;
                case 6:
                    return tile.Actuated == Value;
                default:
                    return false;
            }
        }
    }
}
