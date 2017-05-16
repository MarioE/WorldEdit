using System;
using System.Reflection;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a block shape.
    /// </summary>
    public class Shape : ITemplate
    {
        public static readonly Shape BottomLeft = new Shape(4);
        public static readonly Shape BottomRight = new Shape(3);
        public static readonly Shape Half = new Shape(-1);
        public static readonly Shape Normal = new Shape(0);
        public static readonly Shape TopLeft = new Shape(2);
        public static readonly Shape TopRight = new Shape(1);

        private readonly int _type;

        private Shape(int type)
        {
            _type = type;
        }

        /// <summary>
        /// Parses the specified string into a shape.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static Result<Shape> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(Shape).GetField(s.RemoveWhitespace(),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.IgnoreCase);
            return field != null
                ? Result.From((Shape)field.GetValue(null))
                : Result.FromError<Shape>($"Invalid shape '{s}'.");
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.IsHalfBlock = _type < 0;
            tile.Slope = Math.Max(0, _type);
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile) => _type < 0 ? tile.IsHalfBlock : _type == tile.Slope;
    }
}
