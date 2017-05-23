using System;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a block shape.
    /// </summary>
    public sealed class Shape : ITemplate
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
        /// Tries to parse the specified string into a <see cref="Shape" /> instance. A return value of <c>null</c> indicates
        /// failure.
        /// </summary>
        /// <param name="s">The string to parse, which must not be <c>null</c>.</param>
        /// <returns>The shape, or <c>null</c> if parsing failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        [CanBeNull]
        [Pure]
        public static Shape TryParse([NotNull] string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(Shape).GetField(s.ToPascalCase());
            return (Shape)field?.GetValue(null);
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
