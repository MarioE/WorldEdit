using System;
using JetBrains.Annotations;
using WorldEdit.Extents;
using WorldEdit.TileEntities;

namespace WorldEdit.History
{
    /// <summary>
    /// Represents a tile entity addition.
    /// </summary>
    public sealed class TileEntityAddition : IChange
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileEntityAddition" /> class with the specified tile entity.
        /// </summary>
        /// <param name="tileEntity">The tile entity, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tileEntity" /> is <c>null</c>.</exception>
        public TileEntityAddition([NotNull] ITileEntity tileEntity)
        {
            TileEntity = tileEntity ?? throw new ArgumentNullException(nameof(tileEntity));
        }

        /// <summary>
        /// Gets the tile entity of this <see cref="TileEntityAddition" /> instance.
        /// </summary>
        [NotNull]
        public ITileEntity TileEntity { get; }

        /// <inheritdoc />
        public bool Redo(Extent extent) => extent.AddTileEntity(TileEntity);

        /// <inheritdoc />
        public bool Undo(Extent extent) => extent.RemoveTileEntity(TileEntity);
    }
}
