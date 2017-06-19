using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using JetBrains.Annotations;
using OTAPI.Tile;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
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
    ///     Represents a world backed by an OTAPI tile collection. Players will be periodically notified of changes.
    /// </summary>
    /// <remarks>
    ///     Tile accesses are not synchronized, but this is likely not to be an issue, since concurrent modification of the
    ///     same tiles is quite rare and unlikely to matter.
    /// </remarks>
    public sealed class OTAPIWorld : World, IDisposable
    {
        private const int SectionHeight = 150;
        private const int SectionWidth = 200;

        private readonly object _chestLock = new object();
        private readonly HashSet<Vector> _dirtySections = new HashSet<Vector>();
        private readonly object _sectionLock = new object();
        private readonly Timer _sectionTimer = new Timer(100);
        private readonly object _signLock = new object();
        private readonly TerrariaChest[] _terrariaChests;
        private readonly TerrariaSign[] _terrariaSigns;
        private readonly ITileCollection _terrariaTiles;
        private readonly object _tileEntityLock = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="World" /> class wrapping the specified Terraria tiles, chests, and
        ///     signs.
        /// </summary>
        /// <param name="tiles">The tiles, which must not be <c>null</c>.</param>
        /// <param name="chests">The chests, which must not be <c>null</c>.</param>
        /// <param name="signs">The signs, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        ///     Either <paramref name="tiles" />, <paramref name="chests" />, or <paramref name="signs" /> is <c>null</c>.
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

        /// <summary>
        ///     Disposes the world.
        /// </summary>
        public void Dispose()
        {
            _sectionTimer.Dispose();
        }

        private static ITileEntity Adapt(TileEntity terrariaTileEntity)
        {
            var position = new Vector(terrariaTileEntity.Position.X, terrariaTileEntity.Position.Y);
            switch (terrariaTileEntity)
            {
                case TEItemFrame terrariaItemFrame:
                    return new ItemFrame(position, Adapt(terrariaItemFrame.item));
                case TELogicSensor terrariaLogicSensor:
                    return new LogicSensor(position, (LogicSensorType)terrariaLogicSensor.logicCheck,
                        terrariaLogicSensor.On, terrariaLogicSensor.CountedData);
                case TETrainingDummy terrariaTrainingDummy:
                    return new TrainingDummy(position, terrariaTrainingDummy.npc);
                default:
                    return null;
            }
        }

        private static TerrariaItem Adapt(Item item) =>
            new TerrariaItem {netID = item.Type, stack = item.StackSize, prefix = item.Prefix};

        private static Item Adapt(TerrariaItem terrariaItem) =>
            new Item(terrariaItem.type, terrariaItem.stack, terrariaItem.prefix);

        private static TerrariaChest Adapt(Chest chest) =>
            new TerrariaChest
            {
                x = chest.Position.X,
                y = chest.Position.Y,
                name = chest.Name,
                item = chest.Items.Select(Adapt).ToArray()
            };

        private static Chest Adapt(TerrariaChest terrariaChest) =>
            new Chest(new Vector(terrariaChest.x, terrariaChest.y),
                terrariaChest.name,
                terrariaChest.item.Select(Adapt));

        private static TerrariaSign Adapt(Sign sign) =>
            new TerrariaSign {x = sign.Position.X, y = sign.Position.Y, text = sign.Text};

        private static Sign Adapt(TerrariaSign terrariaSign) =>
            new Sign(new Vector(terrariaSign.x, terrariaSign.y), terrariaSign.text);

        /// <inheritdoc />
        public override bool AddTileEntity(ITileEntity tileEntity)
        {
            switch (tileEntity)
            {
                case Chest chest:
                    return AddChest(chest);
                case Sign sign:
                    return AddSign(sign);
                default:
                    var point16 = new Point16(tileEntity.Position.X, tileEntity.Position.Y);
                    if (TileEntity.ByPosition.ContainsKey(point16))
                    {
                        return false;
                    }

                    AddTerrariaTileEntity(tileEntity);
                    return true;
            }
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
                BlockId = terrariaTile.type,
                BTileHeader = terrariaTile.bTileHeader,
                FrameX = terrariaTile.frameX,
                FrameY = terrariaTile.frameY,
                Liquid = terrariaTile.liquid,
                STileHeader = terrariaTile.sTileHeader,
                WallId = terrariaTile.wall
            };
        }

        /// <inheritdoc />
        public override IEnumerable<ITileEntity> GetTileEntities()
        {
            lock (_chestLock)
            lock (_signLock)
            lock (_tileEntityLock)
            {
                return Enumerable.Empty<ITileEntity>()
                    .Concat(_terrariaChests.Where(tc => tc != null).Select(Adapt))
                    .Concat(_terrariaSigns.Where(ts => ts != null).Select(Adapt))
                    .Concat(TileEntity.ByID.Values.Where(te => te != null).Select(Adapt)).ToList();
            }
        }

        /// <inheritdoc />
        public override bool RemoveTileEntity(ITileEntity tileEntity)
        {
            switch (tileEntity)
            {
                case Chest chest:
                    return RemoveChest(chest);
                case Sign sign:
                    return RemoveSign(sign);
                default:
                    lock (_tileEntityLock)
                    {
                        var point16 = new Point16(tileEntity.Position.X, tileEntity.Position.Y);
                        if (!TileEntity.ByPosition.TryGetValue(point16, out var ttileEntity))
                        {
                            return false;
                        }

                        TileEntity.ByID.Remove(ttileEntity.ID);
                        TileEntity.ByPosition.Remove(point16);
                    }
                    return true;
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
            terrariaTile.type = tile.BlockId;
            terrariaTile.bTileHeader = tile.BTileHeader;
            terrariaTile.frameX = tile.FrameX;
            terrariaTile.frameY = tile.FrameY;
            terrariaTile.liquid = tile.Liquid;
            terrariaTile.sTileHeader = tile.STileHeader;
            terrariaTile.wall = tile.WallId;

            lock (_sectionLock)
            {
                _dirtySections.Add(new Vector(x / SectionWidth, y / SectionHeight));
            }
            return true;
        }

        private bool AddChest(Chest chest)
        {
            var index = -1;
            lock (_chestLock)
            {
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
            }
            var x = chest.Position.X;
            var y = chest.Position.Y;
            var tile = _terrariaTiles[x, y];
            var frameX = tile.frameX;
            var type2 = tile.type;
            if (type2 == TileID.Containers)
            {
                NetMessage.SendData(34, -1, -1, null, 0, x, y, (short)(frameX / 36), index);
            }
            else if (type2 == TileID.Dressers)
            {
                NetMessage.SendData(34, -1, -1, null, 2, x, y, (short)((frameX - 18) / 54), index);
            }
            else if (type2 == TileID.Containers2)
            {
                NetMessage.SendData(34, -1, -1, null, 4, x, y, (short)(frameX / 36), index);
            }
            return true;
        }

        private bool AddSign(Sign sign)
        {
            var index = -1;
            lock (_signLock)
            {
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
            }
            return true;
        }

        private void AddTerrariaTileEntity(ITileEntity tileEntity)
        {
            int id;
            var x = tileEntity.Position.X;
            var y = tileEntity.Position.Y;

            lock (_tileEntityLock)
            {
                switch (tileEntity)
                {
                    case ItemFrame itemFrame:
                        var item = itemFrame.Item;
                        id = TEItemFrame.Place(x, y);
                        TEItemFrame.TryPlacing(x, y, item.Type, item.Prefix, item.StackSize);
                        break;
                    case LogicSensor logicSensor:
                        id = TELogicSensor.Place(x, y);
                        var terrariaLogicSensor = (TELogicSensor)TileEntity.ByID[id];
                        terrariaLogicSensor.CountedData = logicSensor.Data;
                        terrariaLogicSensor.logicCheck = (TELogicSensor.LogicCheckType)logicSensor.Type;
                        terrariaLogicSensor.On = logicSensor.IsEnabled;
                        break;
                    case TrainingDummy _:
                        id = TETrainingDummy.Place(x, y);
                        ((TETrainingDummy)TileEntity.ByID[id]).Activate();
                        break;
                    default:
                        return;
                }
            }
            NetMessage.SendData(86, -1, -1, null, id, x, y);
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
            lock (_chestLock)
            {
                for (var i = _terrariaChests.Length - 1; i >= 0; --i)
                {
                    var terrariaChest = _terrariaChests[i];
                    if (terrariaChest != null && terrariaChest.x == chest.Position.X &&
                        terrariaChest.y == chest.Position.Y)
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
            }
            var x = chest.Position.X;
            var y = chest.Position.Y;
            var type = _terrariaTiles[x, y].type;
            if (type == TileID.Containers)
            {
                NetMessage.SendData(34, -1, -1, null, 1, x, y, 0, index);
            }
            else if (type == TileID.Dressers)
            {
                NetMessage.SendData(34, -1, -1, null, 3, x, y, 0, index);
            }
            else if (type == TileID.Containers2)
            {
                NetMessage.SendData(34, -1, -1, null, 5, x, y, 0, index);
            }
            return true;
        }

        private bool RemoveSign(Sign sign)
        {
            lock (_signLock)
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
            }
            return true;
        }
    }
}
