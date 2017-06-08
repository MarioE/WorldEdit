using System;
using System.Diagnostics.CodeAnalysis;

namespace WorldEdit
{
    /// <summary>
    ///     Represents a tile.
    /// </summary>
    /// <remarks>
    ///     This is a mutable struct for the following reasons:
    ///     <para>
    ///         - Value semantics are important, and if this were a class instead, then many copies would have to be made,
    ///         leading to
    ///         far too much GC pressure.
    ///     </para>
    ///     <para>
    ///         - Tiles can be thought of as a collection of variables, and each variable should be indepdendently modifiable.
    ///         As long
    ///         as this is understood, then mutability should be okay.
    ///     </para>
    /// </remarks>
    public struct Tile : IEquatable<Tile>
    {
        /// <summary>
        ///     Gets or sets the block color.
        /// </summary>
        public byte BlockColor
        {
            get => (byte)(STileHeader & 0x1f);
            set => STileHeader = (short)((STileHeader & ~0x1f) | value);
        }

        /// <summary>
        ///     Gets or sets the block ID.
        /// </summary>
        public ushort BlockId { get; set; }

        /// <summary>
        ///     Gets or sets the byte tile header.
        /// </summary>
        public byte BTileHeader { get; set; }

        /// <summary>
        ///     Gets or sets the X frame.
        /// </summary>
        public short FrameX { get; set; }

        /// <summary>
        ///     Gets or sets the Y frame.
        /// </summary>
        public short FrameY { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile has an actuator.
        /// </summary>
        public bool HasActuator
        {
            get => (STileHeader & 0x800) != 0;
            set => STileHeader = (short)((STileHeader & ~0x800) | (value ? 0x800 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile has blue wire.
        /// </summary>
        public bool HasBlueWire
        {
            get => (STileHeader & 0x100) != 0;
            set => STileHeader = (short)((STileHeader & ~0x100) | (value ? 0x100 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile has green wire.
        /// </summary>
        public bool HasGreenWire
        {
            get => (STileHeader & 0x200) != 0;
            set => STileHeader = (short)((STileHeader & ~0x200) | (value ? 0x200 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile has red wire.
        /// </summary>
        public bool HasRedWire
        {
            get => (STileHeader & 0x80) != 0;
            set => STileHeader = (short)((STileHeader & ~0x80) | (value ? 0x80 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile has yellow wire.
        /// </summary>
        public bool HasYellowWire
        {
            get => (BTileHeader & 0x80) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x80) | (value ? 0x80 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile is active.
        /// </summary>
        public bool IsActive
        {
            get => (STileHeader & 0x20) != 0;
            set => STileHeader = (short)((STileHeader & ~0x20) | (value ? 0x20 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile is actuated.
        /// </summary>
        public bool IsActuated
        {
            get => (STileHeader & 0x40) != 0;
            set => STileHeader = (short)((STileHeader & ~0x40) | (value ? 0x40 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile is a half block.
        /// </summary>
        public bool IsHalfBlock
        {
            get => (STileHeader & 0x400) != 0;
            set => STileHeader = (short)((STileHeader & ~0x400) | (value ? 0x400 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile is honey.
        /// </summary>
        public bool IsHoney
        {
            get => (BTileHeader & 0x40) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x40) | (value ? 0x40 : 0));
        }

        /// <summary>
        ///     Gets or sets a value indicating whether the tile is lava.
        /// </summary>
        public bool IsLava
        {
            get => (BTileHeader & 0x20) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x20) | (value ? 0x20 : 0));
        }

        /// <summary>
        ///     Gets or sets the liquid amount.
        /// </summary>
        public byte Liquid { get; set; }

        /// <summary>
        ///     Gets or sets the liquid type.
        /// </summary>
        public int LiquidType
        {
            get => (BTileHeader & 0x60) >> 5;
            set => BTileHeader = (byte)((BTileHeader & ~0x60) | ((value & 3) << 5));
        }

        /// <summary>
        ///     Gets or sets the slope.
        /// </summary>
        public int Slope
        {
            get => (STileHeader & 0x7000) >> 12;
            set => STileHeader = (short)((STileHeader & ~0x7000) | ((value & 7) << 12));
        }

        /// <summary>
        ///     Gets the short tile header.
        /// </summary>
        public short STileHeader { get; set; }

        /// <summary>
        ///     Gets or sets the wall color.
        /// </summary>
        public byte WallColor
        {
            get => (byte)(BTileHeader & 0x1f);
            set => BTileHeader = (byte)((BTileHeader & ~0x1f) | value);
        }

        /// <summary>
        ///     Gets or sets the wall ID.
        /// </summary>
        public byte WallId { get; set; }

        /// <summary>
        ///     Tests the two specified tiles for equality.
        /// </summary>
        /// <param name="tile">The first tile.</param>
        /// <param name="tile2">The second tile.</param>
        /// <returns><c>true</c> if the two tiles are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Tile tile, Tile tile2) => tile.Equals(tile2);

        /// <summary>
        ///     Tests the two specified tiles for inequality.
        /// </summary>
        /// <param name="tile">The first tile.</param>
        /// <param name="tile2">The second tile.</param>
        /// <returns><c>true</c> if the two tiles are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Tile tile, Tile tile2) => !tile.Equals(tile2);

        /// <summary>
        ///     Determines whether the tile equals the specified tile.
        /// </summary>
        /// <param name="other">The other tile.</param>
        /// <returns><c>true</c> if the two are equal; otherwise <c>false</c>.</returns>
        public bool Equals(Tile other) =>
            BTileHeader == other.BTileHeader && FrameX == other.FrameX && FrameY == other.FrameY &&
            Liquid == other.Liquid && STileHeader == other.STileHeader && BlockId == other.BlockId &&
            WallId == other.WallId;

        /// <summary>
        ///     Determines whether the tile equals the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the two are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => obj is Tile tile && Equals(tile);

        /// <summary>
        ///     Returns the hash code for the tile.
        /// </summary>
        /// <returns>The hash code.</returns>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode", Justification =
            "Tiles will not be used as keys for collections.")]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = BTileHeader.GetHashCode();
                hashCode = (hashCode * 397) ^ FrameX.GetHashCode();
                hashCode = (hashCode * 397) ^ FrameY.GetHashCode();
                hashCode = (hashCode * 397) ^ Liquid.GetHashCode();
                hashCode = (hashCode * 397) ^ STileHeader.GetHashCode();
                hashCode = (hashCode * 397) ^ BlockId.GetHashCode();
                hashCode = (hashCode * 397) ^ WallId.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        ///     Returns the string representation of the tile.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"Block: {BlockId} ({FrameX}, {FrameY}), Wall: {WallId}";
    }
}
