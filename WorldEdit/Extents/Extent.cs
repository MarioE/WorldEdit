using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Templates;
using WorldEdit.TileEntities;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Specifies a tile and tile entity container.
    /// </summary>
    public abstract class Extent
    {
        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        public abstract Vector Dimensions { get; }

        /// <summary>
        /// Adds the specified tile entity.
        /// </summary>
        /// <param name="tileEntity">The tile entity to add, which must not be <c>null</c>.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// The extent is not required to accept the addition if, for example, there are too many tile entities already.
        /// </remarks>
        public abstract bool AddTileEntity([NotNull] ITileEntity tileEntity);

        /// <summary>
        /// Clears everything within the specified region.
        /// </summary>
        /// <param name="region">The region, which must not be <c>null</c>.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="region" /> is <c>null</c>.</exception>
        public int Clear([NotNull] Region region)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                if (SetTile(position, new Tile()))
                {
                    ++count;
                }
            }
            foreach (var tileEntity in GetTileEntities().Where(e => region.Contains(e.Position)))
            {
                RemoveTileEntity(tileEntity);
            }
            return count;
        }

        /// <summary>
        /// Gets the tile located at the specified position.
        /// </summary>
        /// <param name="position">The position, which must be within the bounds.</param>
        /// <returns>The tile located at the position.</returns>
        [Pure]
        public abstract Tile GetTile(Vector position);

        /// <summary>
        /// Gets the tile entities.
        /// </summary>
        /// <returns>The tile entities.</returns>
        [ItemNotNull]
        [NotNull]
        [Pure]
        public abstract IEnumerable<ITileEntity> GetTileEntities();

        /// <summary>
        /// Determines whether the specified position is within the bounds.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns><c>true</c> if the position is within the bounds; otherwise, <c>false</c>.</returns>
        public bool IsInBounds(Vector position) =>
            0 <= position.X && position.X < Dimensions.X && 0 <= position.Y && position.Y < Dimensions.Y;

        /// <summary>
        /// Modifies the tiles within the specified region using the template.
        /// </summary>
        /// <param name="region">The region, which must not be <c>null</c>.</param>
        /// <param name="template">The template, which must not be <c>null</c>.</param>
        /// <returns>The number of modifications.</returns>
        public int ModifyTiles([NotNull] Region region, [NotNull] ITemplate template) =>
            ModifyTiles(region, template, new EmptyMask());

        /// <summary>
        /// Modifies the tiles within the specified region using the template and mask.
        /// </summary>
        /// <param name="region">The region, which must not be <c>null</c>.</param>
        /// <param name="template">The template, which must not be <c>null</c>.</param>
        /// <param name="mask">The mask to test, which must not be <c>null</c>.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="region" />, <paramref name="template" />, or <paramref name="mask" /> is <c>null</c>.
        /// </exception>
        public int ModifyTiles([NotNull] Region region, [NotNull] ITemplate template, [NotNull] Mask mask)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
            if (mask == null)
            {
                throw new ArgumentNullException(nameof(mask));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                if (mask.Test(this, position) && SetTile(position, template.Apply(GetTile(position))))
                {
                    ++count;
                }
            }
            return count;
        }

        /// <summary>
        /// Removes the specified tile entity.
        /// </summary>
        /// <param name="tileEntity">The tile entity to remove, which must not be <c>null</c>.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        public abstract bool RemoveTileEntity([NotNull] ITileEntity tileEntity);

        /// <summary>
        /// Sets the tile located at the specified position.
        /// </summary>
        /// <param name="position">The position, which must be within the bounds.</param>
        /// <param name="tile">The tile.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// The extent is not required to accept the change if, for example, a limit has been reached.
        /// </remarks>
        public abstract bool SetTile(Vector position, Tile tile);
    }
}
