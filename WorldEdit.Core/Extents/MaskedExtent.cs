using System;
using WorldEdit.Core.Masks;

namespace WorldEdit.Core.Extents
{
    /// <summary>
    /// Represents a composable extent that masks the tiles that can be set.
    /// </summary>
    public sealed class MaskedExtent : Extent
    {
        private readonly Extent _extent;
        private readonly Mask _mask;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaskedExtent" /> class with the specified extent and mask.
        /// </summary>
        /// <param name="extent">The extent to compose with.</param>
        /// <param name="mask">The mask to test.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="extent" /> or <paramref name="mask" /> is <c>null</c>.
        /// </exception>
        public MaskedExtent(Extent extent, Mask mask)
        {
            _extent = extent ?? throw new ArgumentNullException(nameof(extent));
            _mask = mask ?? throw new ArgumentNullException(nameof(mask));
        }

        /// <inheritdoc />
        public override Vector Dimensions => _extent.Dimensions;

        /// <inheritdoc />
        public override Tile GetTile(int x, int y) => _extent.GetTile(x, y);

        /// <inheritdoc />
        public override bool SetTile(int x, int y, Tile tile) =>
            _mask.Test(_extent, new Vector(x, y)) && _extent.SetTile(x, y, tile);
    }
}
