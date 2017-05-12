using System;
using OTAPI.Tile;
using WorldEdit.Extents;

namespace WorldEdit
{
    /// <summary>
    /// Represents a world backed with a tile collection.
    /// </summary>
    public class World : Extent
    {
        private readonly ITileCollection _tiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="World" /> class with the specified tile collection.
        /// </summary>
        /// <param name="tiles">The tile collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tiles" /> is <c>null</c>.</exception>
        public World(ITileCollection tiles)
        {
            _tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
        }

        /// <inheritdoc />
        public override Vector LowerBound => Vector.Zero;

        /// <inheritdoc />
        public override Vector UpperBound => new Vector(_tiles.Width, _tiles.Height);

        /// <inheritdoc />
        public override Tile this[int x, int y]
        {
            get => new Tile(_tiles[x, y]);
            set => _tiles[x, y] = value.ToITile();
        }
    }
}
