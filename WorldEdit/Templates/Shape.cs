using System;
using System.Reflection;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a block shape.
    /// </summary>
    public class Shape : ITemplate
    {
        public static readonly Shape BottomLeftSlope = new Shape(4);
        public static readonly Shape BottomRightSlope = new Shape(3);
        public static readonly Shape Half = new Shape(-1);
        public static readonly Shape Normal = new Shape(0);
        public static readonly Shape TopLeftSlope = new Shape(2);
        public static readonly Shape TopRightSlope = new Shape(1);

        /// <summary>
        /// Initializes a new instance of the <see cref="Shape" /> class with the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        public Shape(int type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public int Type { get; }

        /// <summary>
        /// Parses the specified string into a shape.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<Shape> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(Shape).GetField(s.Replace(" ", ""),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            if (field == null)
            {
                return ParsingResult<Shape>.Error($"Invalid shape '{s}'.");
            }
            return (Shape)field.GetValue(null);
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.HalfBlock = Type < 0;
            tile.Slope = Math.Max(0, Type);
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile)
        {
            return Type < 0 ? tile.HalfBlock : Type == tile.Slope;
        }
    }
}
