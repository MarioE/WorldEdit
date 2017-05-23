using System;
using JetBrains.Annotations;

#pragma warning disable 1591

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a wall color type.
    /// </summary>
    public sealed class WallColor : ITemplate
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

        private readonly byte _type;

        private WallColor(byte type)
        {
            _type = type;
        }

        /// <summary>
        /// Tries to parse the specified string into a <see cref="WallColor" /> instance. A return value of <c>null</c> indicates
        /// failure.
        /// </summary>
        /// <param name="s">The string to parse, which must not be <c>null</c>.</param>
        /// <returns>The wall color, or <c>null</c> if parsing failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        [CanBeNull]
        [Pure]
        public static WallColor TryParse([NotNull] string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var field = typeof(WallColor).GetField(s.ToPascalCase());
            return (WallColor)field?.GetValue(null);
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.WallColor = _type;
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile) => tile.WallColor == _type;
    }
}
