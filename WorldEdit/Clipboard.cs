using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using WorldEdit.Extents;
using WorldEdit.Regions;
using WorldEdit.TileEntities;

namespace WorldEdit
{
    /// <summary>
    /// Represents a clipboard that can be copied and pasted.
    /// </summary>
    public sealed class Clipboard : Extent
    {
        private readonly List<ITileEntity> _tileEntities = new List<ITileEntity>();
        private readonly Tile?[,] _tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="Clipboard" /> class with the specified tiles.
        /// </summary>
        /// <param name="tiles">The tiles, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tiles" /> is <c>null</c>.</exception>
        public Clipboard([NotNull] Tile?[,] tiles)
        {
            _tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }

        /// <inheritdoc />
        public override Vector Dimensions => new Vector(_tiles.GetLength(0), _tiles.GetLength(1));

        /// <summary>
        /// Creates a <see cref="Clipboard" /> instance copied from the specified extent and region.
        /// </summary>
        /// <param name="extent">The extent to copy from, which must not be <c>null</c>.</param>
        /// <param name="region">The region,  which must not be <c>null</c>.</param>
        /// <returns>The clipboard.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="extent" /> or <paramref name="region" /> is <c>null</c>.
        /// </exception>
        [NotNull]
        [Pure]
        public static Clipboard CopyFrom([NotNull] Extent extent, [NotNull] Region region)
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
            var clipboard = new Clipboard(new Tile?[dimensions.X, dimensions.Y]);
            foreach (var position in region.Where(extent.IsInBounds))
            {
                var offsetPosition = position - region.LowerBound;
                clipboard.SetTile(offsetPosition, extent.GetTile(position));
            }
            foreach (var entity in extent.GetTileEntities().Where(e => region.Contains(e.Position)))
            {
                var offsetPosition = entity.Position - region.LowerBound;
                clipboard.AddTileEntity(entity.WithPosition(offsetPosition));
            }
            return clipboard;
        }

        /// <inheritdoc />
        public override bool AddTileEntity(ITileEntity tileEntity)
        {
            if (!IsInBounds(tileEntity.Position))
            {
                return false;
            }

            _tileEntities.Add(tileEntity);
            return true;
        }

        /// <inheritdoc />
        public override Tile GetTile(Vector position) => _tiles[position.X, position.Y] ?? new Tile();

        /// <inheritdoc />
        public override IEnumerable<ITileEntity> GetTileEntities() => _tileEntities;

        /// <summary>
        /// Pastes the clipboard to the specified extent and position.
        /// </summary>
        /// <param name="extent">The extent to paste to, which must not be <c>null</c>.</param>
        /// <param name="position">The position.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        /// <remarks>
        /// Null tiles in the array will be ignored when pasting.
        /// </remarks>
        public int PasteTo([NotNull] Extent extent, Vector position)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            var count = 0;
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
                            ++count;
                        }
                    }
                }
            }
            foreach (var tileEntity in _tileEntities)
            {
                var offsetPosition = tileEntity.Position + position;
                if (extent.IsInBounds(offsetPosition))
                {
                    extent.AddTileEntity(tileEntity.WithPosition(offsetPosition));
                }
            }
            return count;
        }

        /// <inheritdoc />
        public override bool RemoveTileEntity(ITileEntity tileEntity) => _tileEntities.Remove(tileEntity);

        /// <inheritdoc />
        public override bool SetTile(Vector position, Tile tile)
        {
            _tiles[position.X, position.Y] = tile;
            return true;
        }
    }
}
