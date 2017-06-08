using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    ///     Represents a block color.
    /// </summary>
    [NoReorder]
    public sealed class BlockColor : ITemplate
    {
#pragma warning disable 1591
        public static readonly BlockColor Blank = new BlockColor(0);
        public static readonly BlockColor Red = new BlockColor(1);
        public static readonly BlockColor Orange = new BlockColor(2);
        public static readonly BlockColor Yellow = new BlockColor(3);
        public static readonly BlockColor Lime = new BlockColor(4);
        public static readonly BlockColor Green = new BlockColor(5);
        public static readonly BlockColor Teal = new BlockColor(6);
        public static readonly BlockColor Cyan = new BlockColor(7);
        public static readonly BlockColor SkyBlue = new BlockColor(8);
        public static readonly BlockColor Blue = new BlockColor(9);
        public static readonly BlockColor Purple = new BlockColor(10);
        public static readonly BlockColor Violet = new BlockColor(11);
        public static readonly BlockColor Pink = new BlockColor(12);
        public static readonly BlockColor DeepRed = new BlockColor(13);
        public static readonly BlockColor DeepOrange = new BlockColor(14);
        public static readonly BlockColor DeepYellow = new BlockColor(15);
        public static readonly BlockColor DeepLime = new BlockColor(16);
        public static readonly BlockColor DeepGreen = new BlockColor(17);
        public static readonly BlockColor DeepTeal = new BlockColor(18);
        public static readonly BlockColor DeepCyan = new BlockColor(19);
        public static readonly BlockColor DeepSkyBlue = new BlockColor(20);
        public static readonly BlockColor DeepBlue = new BlockColor(21);
        public static readonly BlockColor DeepPurple = new BlockColor(22);
        public static readonly BlockColor DeepViolet = new BlockColor(23);
        public static readonly BlockColor DeepPink = new BlockColor(24);
        public static readonly BlockColor Black = new BlockColor(25);
        public static readonly BlockColor White = new BlockColor(26);
        public static readonly BlockColor Gray = new BlockColor(27);
        public static readonly BlockColor Brown = new BlockColor(28);
        public static readonly BlockColor Shadow = new BlockColor(29);
        public static readonly BlockColor Negative = new BlockColor(30);
#pragma warning restore 1591

        private readonly byte _type;

        private BlockColor(byte type)
        {
            _type = type;
        }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            tile.BlockColor = _type;
            return tile;
        }

        /// <inheritdoc />
        public bool Matches(Tile tile) => tile.BlockColor == _type;
    }
}
