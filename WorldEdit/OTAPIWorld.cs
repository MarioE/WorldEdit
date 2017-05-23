using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using JetBrains.Annotations;
using OTAPI.Tile;
using Terraria;
using Terraria.ID;
using WorldEdit.TileEntities;
using Chest = WorldEdit.TileEntities.Chest;
using Sign = WorldEdit.TileEntities.Sign;
using TerrariaChest = Terraria.Chest;
using TerrariaItem = Terraria.Item;
using TerrariaSign = Terraria.Sign;
using TerrariaTile = Terraria.Tile;

namespace WorldEdit
{
    /// <summary>
    /// Represents a world backed by an OTAPI tile collection. Players will be periodically notified of changes.
    /// </summary>
    public sealed class OTAPIWorld : World, IDisposable
    {
        private const int SectionHeight = 150;
        private const int SectionWidth = 200;

        private readonly HashSet<Vector> _dirtySections = new HashSet<Vector>();
        private readonly object _sectionLock = new object();
        private readonly Timer _sectionTimer = new Timer(100);
        private readonly TerrariaChest[] _terrariaChests;
        private readonly TerrariaSign[] _terrariaSigns;
        private readonly ITileCollection _terrariaTiles;

        /// <summary>
        /// Initializes a new instance of the <see cref="World" /> class wrapping the specified Terraria tiles, chests, and signs.
        /// </summary>
        /// <param name="tiles">The tiles, which must not be <c>null</c>.</param>
        /// <param name="chests">The chests, which must not be <c>null</c>.</param>
        /// <param name="signs">The signs, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="tiles" />, <paramref name="chests" />, or <paramref name="signs" /> is <c>null</c>.
        /// </exception>
        public OTAPIWorld([NotNull] ITileCollection tiles, [NotNull] TerrariaChest[] chests,
            [NotNull] TerrariaSign[] signs)
        {
            _terrariaTiles = tiles ?? throw new ArgumentNullException(nameof(tiles));
            _terrariaChests = chests ?? throw new ArgumentNullException(nameof(chests));
            _terrariaSigns = signs ?? throw new ArgumentNullException(nameof(signs));

            _sectionTimer.Elapsed += NotifySections;
            _sectionTimer.Start();
        }

        /// <inheritdoc />
        public override Vector Dimensions => new Vector(_terrariaTiles.Width, _terrariaTiles.Height);

        /// <inheritdoc />
        // TODO: Handle tile entities: item frame, training dummy, and logic sensor
        public override bool AddTileEntity(ITileEntity tileEntity)
        {
            switch (tileEntity)
            {
                case Chest chest:
                    return AddChest(chest);
                case Sign sign:
                    return AddSign(sign);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Disposes this <see cref="World" /> instance.
        /// </summary>
        public void Dispose()
        {
            _sectionTimer.Dispose();
        }

        /// <inheritdoc />
        public override Tile GetTile(Vector position)
        {
            var terrariaTile = _terrariaTiles[position.X, position.Y];
            if (terrariaTile == null)
            {
                return new Tile();
            }

            return new Tile
            {
                BTileHeader = terrariaTile.bTileHeader,
                FrameX = terrariaTile.frameX,
                FrameY = terrariaTile.frameY,
                Liquid = terrariaTile.liquid,
                STileHeader = terrariaTile.sTileHeader,
                Type = terrariaTile.type,
                Wall = terrariaTile.wall
            };
        }

        /// <inheritdoc />
        public override IEnumerable<ITileEntity> GetTileEntities()
        {
            return Enumerable.Empty<ITileEntity>()
                .Concat(_terrariaChests.Where(tc => tc != null).Select(Adapt))
                .Concat(_terrariaSigns.Where(ts => ts != null).Select(Adapt));
        }

        /// <inheritdoc />
        // TODO: Handle tile entities: item frame, training dummy, and logic sensor
        public override bool RemoveTileEntity(ITileEntity tileEntity)
        {
            switch (tileEntity)
            {
                case Chest chest:
                    return RemoveChest(chest);
                case Sign sign:
                    return RemoveSign(sign);
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public override bool SetTile(Vector position, Tile tile)
        {
            var x = position.X;
            var y = position.Y;
            var terrariaTile = _terrariaTiles[x, y];
            if (terrariaTile == null)
            {
                terrariaTile = new TerrariaTile();
                _terrariaTiles[x, y] = terrariaTile;
            }

            // Avoid constructing a TerrariaTile whenever possible. This reduces GC pressure.
            terrariaTile.bTileHeader = tile.BTileHeader;
            terrariaTile.frameX = tile.FrameX;
            terrariaTile.frameY = tile.FrameY;
            terrariaTile.liquid = tile.Liquid;
            terrariaTile.sTileHeader = tile.STileHeader;
            terrariaTile.type = tile.Type;
            terrariaTile.wall = tile.Wall;

            lock (_sectionLock)
            {
                _dirtySections.Add(new Vector(x / SectionWidth, y / SectionHeight));
            }
            return true;
        }

        private TerrariaItem Adapt(Item item) =>
            new TerrariaItem {netID = item.Type, stack = item.StackSize, prefix = item.Prefix};

        private Item Adapt(TerrariaItem terrariaItem) =>
            new Item(terrariaItem.type, terrariaItem.stack, terrariaItem.prefix);

        private TerrariaChest Adapt(Chest chest) =>
            new TerrariaChest
            {
                x = chest.Position.X,
                y = chest.Position.Y,
                name = chest.Name,
                item = chest.Items.Select(Adapt).ToArray()
            };

        private Chest Adapt(TerrariaChest terrariaChest)
        {
            var x = terrariaChest.x;
            var y = terrariaChest.y;
            return new Chest(new Vector(x, y),
                terrariaChest.name,
                terrariaChest.item.Select(Adapt),
                _terrariaTiles[x, y].type == TileID.Dressers);
        }

        private TerrariaSign Adapt(Sign sign) =>
            new TerrariaSign {x = sign.Position.X, y = sign.Position.Y, text = sign.Text};

        private Sign Adapt(TerrariaSign terrariaSign) =>
            new Sign(new Vector(terrariaSign.x, terrariaSign.y), terrariaSign.text);

        private bool AddChest(Chest chest)
        {
            var index = -1;
            for (var i = 0; i < _terrariaChests.Length; ++i)
            {
                var terrariaChest = _terrariaChests[i];
                if (terrariaChest == null)
                {
                    index = i;
                    break;
                }

                var type = _terrariaTiles[terrariaChest.x, terrariaChest.y].type;
                if (type != TileID.Containers && type != TileID.Dressers && type != TileID.Containers2)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                return false;
            }

            _terrariaChests[index] = Adapt(chest);
            var x = chest.Position.X;
            var y = chest.Position.Y;
            var frameX = _terrariaTiles[x, y].frameX;
            var style = chest.IsDresser ? (frameX - 18) / 54 : frameX / 36;
            NetMessage.SendData(34, -1, -1, null, chest.IsDresser ? 2 : 0, x, y, style);
            NetMessage.SendData(33, -1, -1, null, index);
            return false;
        }

        private bool AddSign(Sign sign)
        {
            var index = -1;
            for (var i = 0; i < _terrariaSigns.Length; ++i)
            {
                var terrariaSign = _terrariaSigns[i];
                if (terrariaSign == null)
                {
                    index = i;
                    break;
                }

                var type = _terrariaTiles[terrariaSign.x, terrariaSign.y].type;
                if (type != TileID.Signs && type != TileID.Tombstones && type != TileID.AnnouncementBox)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                return false;
            }

            _terrariaSigns[index] = Adapt(sign);
            return true;
        }

        private void NotifySections(object sender, ElapsedEventArgs args)
        {
            lock (_sectionLock)
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

        private bool RemoveChest(Chest chest)
        {
            var index = -1;
            for (var i = _terrariaChests.Length - 1; i >= 0; --i)
            {
                var terrariaChest = _terrariaChests[i];
                if (terrariaChest == null)
                {
                    continue;
                }

                var x = terrariaChest.x;
                var y = terrariaChest.y;
                var type = _terrariaTiles[x, y].type;
                if (x == chest.Position.X && y == chest.Position.Y && type == TileID.Dressers == chest.IsDresser)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                return false;
            }

            _terrariaChests[index] = null;
            var x2 = chest.Position.X;
            var y2 = chest.Position.Y;
            NetMessage.SendData(34, -1, -1, null, chest.IsDresser ? 4 : 1, x2, y2);
            return true;
        }

        private bool RemoveSign(Sign sign)
        {
            var index = -1;
            for (var i = _terrariaSigns.Length - 1; i >= 0; --i)
            {
                var terrariaSign = _terrariaSigns[i];
                if (terrariaSign != null && terrariaSign.x == sign.Position.X && terrariaSign.y == sign.Position.Y)
                {
                    index = i;
                    break;
                }
            }
            if (index < 0)
            {
                return false;
            }

            _terrariaSigns[index] = null;
            return true;
        }
    }
}
