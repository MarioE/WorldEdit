using System;
using System.Reflection;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a color type.
    /// </summary>
    public class Color : ITemplate
    {
        public static readonly Color Black = new Color(25);
        public static readonly Color Blank = new Color(0);
        public static readonly Color Blue = new Color(9);
        public static readonly Color Brown = new Color(28);
        public static readonly Color Cyan = new Color(7);
        public static readonly Color DeepBlue = new Color(21);
        public static readonly Color DeepCyan = new Color(19);
        public static readonly Color DeepGreen = new Color(17);
        public static readonly Color DeepLime = new Color(16);
        public static readonly Color DeepOrange = new Color(14);
        public static readonly Color DeepPink = new Color(24);
        public static readonly Color DeepPurple = new Color(22);
        public static readonly Color DeepRed = new Color(13);
        public static readonly Color DeepSkyBlue = new Color(20);
        public static readonly Color DeepTeal = new Color(18);
        public static readonly Color DeepViolet = new Color(23);
        public static readonly Color DeepYellow = new Color(15);
        public static readonly Color Gray = new Color(27);
        public static readonly Color Green = new Color(5);
        public static readonly Color Lime = new Color(4);
        public static readonly Color Negative = new Color(30);
        public static readonly Color Orange = new Color(2);
        public static readonly Color Pink = new Color(12);
        public static readonly Color Purple = new Color(10);
        public static readonly Color Red = new Color(1);
        public static readonly Color Shadow = new Color(29);
        public static readonly Color SkyBlue = new Color(8);
        public static readonly Color Teal = new Color(6);
        public static readonly Color Violet = new Color(11);
        public static readonly Color White = new Color(26);
        public static readonly Color Yellow = new Color(3);

        /// <summary>
        /// Initializes a new instance of the <see cref="Color" /> class with the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public Color(byte type)
        {
            Type = type;
        }
        
        /// <summary>
        /// Gets the type.
        /// </summary>
        public byte Type { get; }

        /// <summary>
        /// Parses the specified string into a color.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<Color> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(Color).GetField(s.Replace(" ", ""),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (field == null)
            {
                return ParsingResult.FromError<Color>($"Invalid color '{s}'.");
            }
            return ParsingResult.From((Color)field.GetValue(null));
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.Color = Type;
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            return tile.Color == Type;
        }
    }
}
