using System;
using System.Linq;
using WorldEdit.Regions;
using WorldEdit.Templates;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Specifies a tile container.
    /// </summary>
    public abstract class Extent
    {
        /// <summary>
        /// Gets the dimensions of the extent.
        /// </summary>
        public abstract Vector Dimensions { get; }

        /// <summary>
        /// Clears the tiles in the specified region.
        /// </summary>
        /// <param name="region">The region to modify.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="region" /> is <c>null</c>.</exception>
        public int ClearTiles(Region region)
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
            return count;
        }

        /// <summary>
        /// Returns the tile located at the specified coordinates. For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The tile.</returns>
        public abstract Tile GetTile(int x, int y);

        /// <summary>
        /// Returns the tile located at the specified position. For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The tile.</returns>
        public Tile GetTile(Vector position) => GetTile(position.X, position.Y);

        /// <summary>
        /// Determines whether the specified coordinates are in the bounds of the extent.
        /// </summary>
        /// <param name="x">The X coordinate to check.</param>
        /// <param name="y">The Y coordinate to check.</param>
        /// <returns><c>true</c> if the coordinates are in the bounds; otherwise, <c>false</c>.</returns>
        public bool IsInBounds(int x, int y) => 0 <= x && x < Dimensions.X && 0 <= y && y < Dimensions.Y;

        /// <summary>
        /// Determines whether the specified position is in in the bounds of the extent.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><c>true</c> if the position is in the bounds; otherwise, <c>false</c>.</returns>
        public bool IsInBounds(Vector position) => IsInBounds(position.X, position.Y);

        /// <summary>
        /// Replaces the tiles in the specified region using the specified templates.
        /// </summary>
        /// <param name="region">The region to modify.</param>
        /// <param name="fromTemplate">The template to match with.</param>
        /// <param name="toTemplate">The template to apply.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="region" />, <paramref name="fromTemplate" />, or <paramref name="toTemplate" /> is <c>null</c>.
        /// </exception>
        public int ReplaceTiles(Region region, ITemplate fromTemplate, ITemplate toTemplate)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (fromTemplate == null)
            {
                throw new ArgumentNullException(nameof(fromTemplate));
            }
            if (toTemplate == null)
            {
                throw new ArgumentNullException(nameof(toTemplate));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                var tile = GetTile(position);
                if (fromTemplate.Matches(tile) && SetTile(position, toTemplate.Apply(tile)))
                {
                    ++count;
                }
            }
            return count;
        }

        /// <summary>
        /// Sets the tile located at the specified coordinates. For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="tile">The tile.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        public abstract bool SetTile(int x, int y, Tile tile);

        /// <summary>
        /// Sets the tile located at the specified position. For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="tile">The tile.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        public bool SetTile(Vector position, Tile tile) => SetTile(position.X, position.Y, tile);

        /// <summary>
        /// Sets the tiles in the specified region using the specified template.
        /// </summary>
        /// <param name="region">The region to modify.</param>
        /// <param name="template">The template to apply.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="region" /> or <paramref name="template" /> is <c>null</c>.
        /// </exception>
        public int SetTiles(Region region, ITemplate template)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                if (SetTile(position, template.Apply(GetTile(position))))
                {
                    ++count;
                }
            }
            return count;
        }
    }
}
