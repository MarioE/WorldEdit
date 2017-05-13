using System;
using System.Reflection;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a wire type and state.
    /// </summary>
    public class Wire : ITemplate
    {
        public static readonly Wire ActuatorOff = new Wire(-1, false);
        public static readonly Wire ActuatorOn = new Wire(-1, true);
        public static readonly Wire BlueOff = new Wire(2, false);
        public static readonly Wire BlueOn = new Wire(2, true);
        public static readonly Wire GreenOff = new Wire(3, false);
        public static readonly Wire GreenOn = new Wire(3, true);
        public static readonly Wire RedOff = new Wire(1, false);
        public static readonly Wire RedOn = new Wire(1, true);
        public static readonly Wire YellowOff = new Wire(4, false);
        public static readonly Wire YellowOn = new Wire(4, true);

        /// <summary>
        /// Initializes a new instance of the <see cref="Wire" /> class with the specified type and state.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="state">The state.</param>
        public Wire(int type, bool state)
        {
            Type = type;
            State = state;
        }

        /// <summary>
        /// Gets a value indicating the wire state.
        /// </summary>
        public bool State { get; }

        /// <summary>
        /// Gets the wire type.
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// Parses the specified string into a wire.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<Wire> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(Wire).GetField(s.Replace(" ", ""),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (field == null)
            {
                return ParsingResult.FromError<Wire>($"Invalid wire '{s}'.");
            }
            return ParsingResult.From((Wire)field.GetValue(null));
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            switch (Type)
            {
                case -1:
                    tile.Actuator = State;
                    break;
                case 1:
                    tile.Wire = State;
                    break;
                case 2:
                    tile.Wire2 = State;
                    break;
                case 3:
                    tile.Wire3 = State;
                    break;
                case 4:
                    tile.Wire4 = State;
                    break;
            }
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            switch (Type)
            {
                case -1:
                    return tile.Actuator == State;
                case 1:
                    return tile.Wire == State;
                case 2:
                    return tile.Wire2 == State;
                case 3:
                    return tile.Wire3 == State;
                case 4:
                    return tile.Wire4 == State;
                default:
                    return false;
            }
        }
    }
}
