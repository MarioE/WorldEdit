using System;
using JetBrains.Annotations;
using WorldEdit.Masks;

namespace WorldEdit.Extents
{
    /// <summary>
    ///     Represents an extent that masks the tiles that can be set.
    /// </summary>
    public sealed class MaskedExtent : WrappedExtent
    {
        private readonly Mask _mask;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MaskedExtent" /> class with the specified extent and mask.
        /// </summary>
        /// <param name="extent">The extent to wrap, which must not be <c>null</c>.</param>
        /// <param name="mask">The mask to use, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mask" /> is <c>null</c>.</exception>
        public MaskedExtent([NotNull] Extent extent, [NotNull] Mask mask) : base(extent)
        {
            _mask = mask ?? throw new ArgumentNullException(nameof(mask));
        }

        /// <inheritdoc />
        public override bool SetTile(Vector position, Tile tile) =>
            _mask.Test(Extent, position) && Extent.SetTile(position, tile);
    }
}
