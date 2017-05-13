using System;
using System.Reflection;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a wall color type.
    /// </summary>
    public class WallColor : ITemplate
    {
        public static readonly WallColor Black = new WallColor(25);
        public static readonly WallColor Blank = new WallColor(0);
        public static readonly WallColor Blue = new WallColor(9);
        public static readonly WallColor Brown = new WallColor(28);
        public static readonly WallColor Cyan = new WallColor(7);
        public static readonly WallColor DeepBlue = new WallColor(21);
        public static readonly WallColor DeepCyan = new WallColor(19);
        public static readonly WallColor DeepGreen = new WallColor(17);
        public static readonly WallColor DeepLime = new WallColor(16);
        public static readonly WallColor DeepOrange = new WallColor(14);
        public static readonly WallColor DeepPink = new WallColor(24);
        public static readonly WallColor DeepPurple = new WallColor(22);
        public static readonly WallColor DeepRed = new WallColor(13);
        public static readonly WallColor DeepSkyBlue = new WallColor(20);
        public static readonly WallColor DeepTeal = new WallColor(18);
        public static readonly WallColor DeepViolet = new WallColor(23);
        public static readonly WallColor DeepYellow = new WallColor(15);
        public static readonly WallColor Gray = new WallColor(27);
        public static readonly WallColor Green = new WallColor(5);
        public static readonly WallColor Lime = new WallColor(4);
        public static readonly WallColor Negative = new WallColor(30);
        public static readonly WallColor Orange = new WallColor(2);
        public static readonly WallColor Pink = new WallColor(12);
        public static readonly WallColor Purple = new WallColor(10);
        public static readonly WallColor Red = new WallColor(1);
        public static readonly WallColor Shadow = new WallColor(29);
        public static readonly WallColor SkyBlue = new WallColor(8);
        public static readonly WallColor Teal = new WallColor(6);
        public static readonly WallColor Violet = new WallColor(11);
        public static readonly WallColor White = new WallColor(26);
        public static readonly WallColor Yellow = new WallColor(3);

        /// <summary>
        /// Initializes a new instance of the <see cref="WallColor" /> class with the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public WallColor(byte type)
        {
            Type = type;
        }
        
        /// <summary>
        /// Gets the type.
        /// </summary>
        public byte Type { get; }

        /// <summary>
        /// Parses the specified string into a wall color.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<WallColor> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(WallColor).GetField(s.Replace(" ", ""),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (field == null)
            {
                return ParsingResult.FromError<WallColor>($"Invalid wall color '{s}'.");
            }
            return ParsingResult.From((WallColor)field.GetValue(null));
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.WallColor = Type;
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            return tile.WallColor == Type;
        }
    }
}
