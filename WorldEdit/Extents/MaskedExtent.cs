using System;
using WorldEdit.Masks;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Represents a composable extent that masks the tiles that can be set.
    /// </summary>
    public class MaskedExtent : Extent
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
        public override Vector LowerBound => _extent.LowerBound;

        /// <inheritdoc />
        public override Vector UpperBound => _extent.UpperBound;

        /// <inheritdoc />
        public override Tile this[int x, int y]
        {
            get => _extent[x, y];
            set
            {
                if (_mask.Test(_extent, new Vector(x, y)))
                {
                    _extent[x, y] = value;
                }
            }
        }
    }
}
