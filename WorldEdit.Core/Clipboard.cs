﻿using System;
using System.Linq;
using WorldEdit.Core.Extents;
using WorldEdit.Core.Regions;

namespace WorldEdit.Core
{
    /// <summary>
    /// Represents a clipboard of tiles that can be copied, pasted, and saved.
    /// </summary>
    public sealed class Clipboard : Extent
    {
        private readonly Tile?[,] _tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="Clipboard" /> class with the specified tiles.
        /// </summary>
        /// <param name="tiles">The tiles.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tiles" /> is <c>null</c>.</exception>
        public Clipboard(Tile?[,] tiles)
        {
            _tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }

        /// <inheritdoc />
        public override Vector Dimensions => new Vector(_tiles.GetLength(0), _tiles.GetLength(1));

        /// <summary>
        /// Copies a clipboard from the specified extent and region. The region bounds will be used when copying.
        /// </summary>
        /// <param name="extent">The extent to copy from.</param>
        /// <param name="region">The region to use.</param>
        /// <returns>The clipboard.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="extent" /> or <paramref name="region" /> is <c>null</c>.
        /// </exception>
        public static Clipboard CopyFrom(Extent extent, Region region)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            var dimensions = region.Dimensions;
            var tiles = new Tile?[dimensions.X, dimensions.Y];
            foreach (var position in region.Where(extent.IsInBounds))
            {
                var offsetPosition = position - region.LowerBound;
                tiles[offsetPosition.X, offsetPosition.Y] = extent.GetTile(position);
            }
            return new Clipboard(tiles);
        }

        /// <inheritdoc />
        public override Tile GetTile(int x, int y) => _tiles[x, y] ?? new Tile();

        /// <summary>
        /// Pastes the clipboard to the specified extent and position. Tiles that are <c>null</c> will be ignored.
        /// </summary>
        /// <param name="extent">The extent to paste to.</param>
        /// <param name="position">The position.</param>
        public void PasteTo(Extent extent, Vector position)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            for (var x = 0; x < Dimensions.X; ++x)
            {
                for (var y = 0; y < Dimensions.Y; ++y)
                {
                    var tile = _tiles[x, y];
                    if (tile != null)
                    {
                        var offsetPosition = new Vector(x, y) + position;
                        if (extent.IsInBounds(offsetPosition))
                        {
                            extent.SetTile(offsetPosition, tile.Value);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public override bool SetTile(int x, int y, Tile tile)
        {
            _tiles[x, y] = tile;
            return true;
        }
    }
}