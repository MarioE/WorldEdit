using System;
using OTAPI.Tile;

namespace WorldEdit
{
    /// <summary>
    /// Represents a tile.
    /// <para />
    /// This class is a mutable struct to relieve GC pressure and add ease of use.
    /// </summary>
    [Serializable]
    public struct Tile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tile" /> structure copying the specified <see cref="ITile" /> instance.
        /// </summary>
        /// <param name="copy">The <see cref="ITile" /> instance.</param>
        public Tile(ITile copy)
        {
            if (copy == null)
            {
                BTileHeader = Liquid = Wall = 0;
                FrameX = FrameY = STileHeader = 0;
                Type = 0;
                return;
            }

            BTileHeader = copy.bTileHeader;
            FrameX = copy.frameX;
            FrameY = copy.frameY;
            Liquid = copy.liquid;
            STileHeader = copy.sTileHeader;
            Type = copy.type;
            Wall = copy.wall;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is active.
        /// </summary>
        public bool Active
        {
            get => (STileHeader & 0x20) != 0;
            set => STileHeader = (short)((STileHeader & ~0x20) | (value ? 0x20 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is actuated.
        /// </summary>
        public bool Actuated
        {
            get => (STileHeader & 0x40) != 0;
            set => STileHeader = (short)((STileHeader & ~0x40) | (value ? 0x40 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile has an actuator.
        /// </summary>
        public bool Actuator
        {
            get => (STileHeader & 0x800) != 0;
            set => STileHeader = (short)((STileHeader & ~0x800) | (value ? 0x800 : 0));
        }

        /// <summary>
        /// Gets the byte tile header.
        /// </summary>
        public byte BTileHeader { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        public byte Color
        {
            get => (byte)(STileHeader & 0x1f);
            set => STileHeader = (short)((STileHeader & ~0x1f) | value);
        }

        /// <summary>
        /// Gets or sets the X frame.
        /// </summary>
        public short FrameX { get; set; }

        /// <summary>
        /// Gets or sets the Y frame.
        /// </summary>
        public short FrameY { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is a half block.
        /// </summary>
        public bool HalfBlock
        {
            get => (STileHeader & 0x400) != 0;
            set => STileHeader = (short)((STileHeader & ~0x400) | (value ? 0x400 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is honey.
        /// </summary>
        public bool Honey
        {
            get => (BTileHeader & 0x40) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x40) | (value ? 0x40 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile is lava.
        /// </summary>
        public bool Lava
        {
            get => (BTileHeader & 0x20) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x20) | (value ? 0x20 : 0));
        }

        /// <summary>
        /// Gets or sets the liquid amount.
        /// </summary>
        public byte Liquid { get; set; }

        /// <summary>
        /// Gets or sets the liquid type.
        /// </summary>
        public int LiquidType
        {
            get => (BTileHeader & 0x60) >> 5;
            set => BTileHeader = (byte)((BTileHeader & ~0x60) | ((value & 3) << 5));
        }

        /// <summary>
        /// Gets or sets the slope.
        /// </summary>
        public int Slope
        {
            get => (STileHeader & 0x7000) >> 12;
            set => STileHeader = (short)((STileHeader & ~0x7000) | ((value & 7) << 12));
        }

        /// <summary>
        /// Gets the short tile header.
        /// </summary>
        public short STileHeader { get; set; }

        /// <summary>
        /// Gets or sets the block type.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// Gets or sets the wall type.
        /// </summary>
        public byte Wall { get; set; }

        /// <summary>
        /// Gets or sets the wall color.
        /// </summary>
        public byte WallColor
        {
            get => (byte)(BTileHeader & 0x1f);
            set => BTileHeader = (byte)((BTileHeader & ~0x1f) | value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile has red wire.
        /// </summary>
        public bool Wire
        {
            get => (STileHeader & 0x80) != 0;
            set => STileHeader = (short)((STileHeader & ~0x80) | (value ? 0x80 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile has blue wire.
        /// </summary>
        public bool Wire2
        {
            get => (STileHeader & 0x100) != 0;
            set => STileHeader = (short)((STileHeader & ~0x100) | (value ? 0x100 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile has green wire.
        /// </summary>
        public bool Wire3
        {
            get => (STileHeader & 0x200) != 0;
            set => STileHeader = (short)((STileHeader & ~0x200) | (value ? 0x200 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tile has yellow wire.
        /// </summary>
        public bool Wire4
        {
            get => (BTileHeader & 0x80) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x80) | (value ? 0x80 : 0));
        }

        /// <summary>
        /// Converts the tile to the equivalent <see cref="ITile" /> instance.
        /// </summary>
        /// <returns>The <see cref="ITile" /> instance.</returns>
        public ITile ToITile()
        {
            return new Terraria.Tile
            {
                bTileHeader = BTileHeader,
                frameX = FrameX,
                frameY = FrameY,
                liquid = Liquid,
                sTileHeader = STileHeader,
                type = Type,
                wall = Wall
            };
        }
    }
}
