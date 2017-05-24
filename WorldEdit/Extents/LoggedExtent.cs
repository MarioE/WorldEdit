using System;
using JetBrains.Annotations;
using WorldEdit.History;
using WorldEdit.TileEntities;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Represents an extent that logs changes to a change set.
    /// </summary>
    public sealed class LoggedExtent : WrappedExtent
    {
        private readonly ChangeSet _changeSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedExtent" /> class with the specified extent and change set.
        /// </summary>
        /// <param name="extent">The extent to wrap, which must not be <c>null</c>.</param>
        /// <param name="changeSet">The change set to log to, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="changeSet" /> is <c>null</c>.</exception>
        public LoggedExtent([NotNull] Extent extent, [NotNull] ChangeSet changeSet) : base(extent)
        {
            _changeSet = changeSet ?? throw new ArgumentNullException(nameof(changeSet));
        }

        /// <inheritdoc />
        public override bool AddTileEntity(ITileEntity tileEntity)
        {
            if (Extent.AddTileEntity(tileEntity))
            {
                _changeSet.Add(new TileEntityAddition(tileEntity));
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public override bool RemoveTileEntity(ITileEntity tileEntity)
        {
            if (Extent.RemoveTileEntity(tileEntity))
            {
                _changeSet.Add(new TileEntityRemoval(tileEntity));
                return true;
            }
            return false;
        }

        /// <inheritdoc />
        public override bool SetTile(Vector position, Tile tile)
        {
            var oldTile = Extent.GetTile(position);
            if (Extent.SetTile(position, tile))
            {
                _changeSet.Add(new TileUpdate(position, oldTile, tile));
                return true;
            }
            return false;
        }
    }
}
