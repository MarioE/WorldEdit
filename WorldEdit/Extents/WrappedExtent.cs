using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using WorldEdit.TileEntities;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Represents a wrapped extent.
    /// </summary>
    public class WrappedExtent : Extent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedExtent" /> class wrapping the specified extent.
        /// </summary>
        /// <param name="extent">The extent to wrap, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public WrappedExtent([NotNull] Extent extent)
        {
            Extent = extent ?? throw new ArgumentNullException(nameof(extent));
        }

        /// <inheritdoc />
        public override Vector Dimensions => Extent.Dimensions;

        /// <summary>
        /// Gets the wrapped extent of this <see cref="WrappedExtent" /> instance.
        /// </summary>
        protected Extent Extent { get; }

        /// <inheritdoc />
        public override bool AddTileEntity(ITileEntity tileEntity) => Extent.AddTileEntity(tileEntity);

        /// <inheritdoc />
        public override Tile GetTile(Vector position) => Extent.GetTile(position);

        /// <inheritdoc />
        public override IEnumerable<ITileEntity> GetTileEntities() => Extent.GetTileEntities();

        /// <inheritdoc />
        public override bool RemoveTileEntity(ITileEntity tileEntity) => Extent.RemoveTileEntity(tileEntity);

        /// <inheritdoc />
        public override bool SetTile(Vector position, Tile tile) => Extent.SetTile(position, tile);
    }
}
