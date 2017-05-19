using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using OTAPI.Tile;
using Terraria;
using WorldEdit.Extents;

namespace WorldEdit
{
    /// <summary>
    /// Represents a world backed with a tile collection. Players will periodically be notified of changes.
    /// </summary>
    public sealed class World : Extent, IDisposable
    {
        private static readonly int SectionHeight = 150;
        private static readonly int SectionWidth = 200;

        private readonly HashSet<Vector> _dirtySections = new HashSet<Vector>();
        private readonly object _lock = new object();
        private readonly ITileCollection _tiles;
        private readonly Timer _timer = new Timer(100);

        /// <summary>
        /// Initializes a new instance of the <see cref="World" /> class with the specified tile collection.
        /// </summary>
        /// <param name="tiles">The tile collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="tiles" /> is <c>null</c>.</exception>
        public World(ITileCollection tiles)
        {
            _tiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
            _timer.Elapsed += NotifyChanges;
            _timer.Start();
        }

        /// <inheritdoc />
        public override Vector Dimensions => new Vector(_tiles.Width, _tiles.Height);

        /// <summary>
        /// Disposes the world.
        /// </summary>
        public void Dispose()
        {
            _timer.Dispose();
        }

        /// <inheritdoc />
        public override Tile GetTile(int x, int y) => new Tile(_tiles[x, y]);

        /// <inheritdoc />
        public override bool SetTile(int x, int y, Tile tile)
        {
            // Don't construct a new instance of ITile, if at all possible. This reduces GC pressure.
            if (_tiles[x, y] == null)
            {
                _tiles[x, y] = tile.ToITile();
            }
            else
            {
                _tiles[x, y].bTileHeader = tile.BTileHeader;
                _tiles[x, y].frameX = tile.FrameX;
                _tiles[x, y].frameY = tile.FrameY;
                _tiles[x, y].liquid = tile.Liquid;
                _tiles[x, y].sTileHeader = tile.STileHeader;
                _tiles[x, y].type = tile.Type;
                _tiles[x, y].wall = tile.Wall;
            }

            lock (_lock)
            {
                _dirtySections.Add(new Vector(x / SectionWidth, y / SectionHeight));
            }
            return true;
        }

        private void NotifyChanges(object sender, ElapsedEventArgs args)
        {
            lock (_lock)
            {
                foreach (var client in Netplay.Clients.Where(c => c.IsActive))
                {
                    foreach (var section in _dirtySections)
                    {
                        client.TileSections[section.X, section.Y] = false;
                    }
                }
                _dirtySections.Clear();
            }
        }
    }
}
