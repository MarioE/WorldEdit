using System;
using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    ///     Represents a block shape.
    /// </summary>
    [NoReorder]
    public sealed class BlockShape : ITemplate
    {
#pragma warning disable 1591
        public static readonly BlockShape Half = new BlockShape(-1);
        public static readonly BlockShape Normal = new BlockShape(0);
        public static readonly BlockShape TopRight = new BlockShape(1);
        public static readonly BlockShape TopLeft = new BlockShape(2);
        public static readonly BlockShape BottomRight = new BlockShape(3);
        public static readonly BlockShape BottomLeft = new BlockShape(4);
#pragma warning restore 1591

        private readonly int _type;

        private BlockShape(int type)
        {
            _type = type;
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
