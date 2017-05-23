using System;
using JetBrains.Annotations;

namespace WorldEdit
{
    /// <summary>
    /// Represents a tile.
    /// </summary>
    /// <remarks>
    /// This is a mutable struct for the following reasons:
    /// <para />
    /// - Value semantics are important, and if this were a class instead, then many copies would have to be made, leading to
    /// far too much GC pressure.
    /// <para />
    /// - Tiles can be thought of as a collection of variables, and each variable should be indepdendently modifiable. As long
    /// as this is understood, then mutability should be okay.
    /// </remarks>
    public struct Tile : IEquatable<Tile>
    {
        /// <summary>
        /// Gets or sets the byte tile header of this <see cref="Tile" /> instance.
        /// </summary>
        public byte BTileHeader { get; set; }

        /// <summary>
        /// Gets or sets the color of this <see cref="Tile" /> instance.
        /// </summary>
        public byte Color
        {
            get => (byte)(STileHeader & 0x1f);
            set => STileHeader = (short)((STileHeader & ~0x1f) | value);
        }

        /// <summary>
        /// Gets or sets the X frame of this <see cref="Tile" /> instance.
        /// </summary>
        public short FrameX { get; set; }

        /// <summary>
        /// Gets or sets the Y frame of this <see cref="Tile" /> instance.
        /// </summary>
        public short FrameY { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance has an actuator.
        /// </summary>
        public bool HasActuator
        {
            get => (STileHeader & 0x800) != 0;
            set => STileHeader = (short)((STileHeader & ~0x800) | (value ? 0x800 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance has blue wire.
        /// </summary>
        public bool HasBlueWire
        {
            get => (STileHeader & 0x100) != 0;
            set => STileHeader = (short)((STileHeader & ~0x100) | (value ? 0x100 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance has green wire.
        /// </summary>
        public bool HasGreenWire
        {
            get => (STileHeader & 0x200) != 0;
            set => STileHeader = (short)((STileHeader & ~0x200) | (value ? 0x200 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance has red wire.
        /// </summary>
        public bool HasRedWire
        {
            get => (STileHeader & 0x80) != 0;
            set => STileHeader = (short)((STileHeader & ~0x80) | (value ? 0x80 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance has yellow wire.
        /// </summary>
        public bool HasYellowWire
        {
            get => (BTileHeader & 0x80) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x80) | (value ? 0x80 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance is active.
        /// </summary>
        public bool IsActive
        {
            get => (STileHeader & 0x20) != 0;
            set => STileHeader = (short)((STileHeader & ~0x20) | (value ? 0x20 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance is actuated.
        /// </summary>
        public bool IsActuated
        {
            get => (STileHeader & 0x40) != 0;
            set => STileHeader = (short)((STileHeader & ~0x40) | (value ? 0x40 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance is a half block.
        /// </summary>
        public bool IsHalfBlock
        {
            get => (STileHeader & 0x400) != 0;
            set => STileHeader = (short)((STileHeader & ~0x400) | (value ? 0x400 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance is honey.
        /// </summary>
        public bool IsHoney
        {
            get => (BTileHeader & 0x40) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x40) | (value ? 0x40 : 0));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Tile" /> instance is lava.
        /// </summary>
        public bool IsLava
        {
            get => (BTileHeader & 0x20) != 0;
            set => BTileHeader = (byte)((BTileHeader & ~0x20) | (value ? 0x20 : 0));
        }

        /// <summary>
        /// Gets or sets the liquid amount of this <see cref="Tile" /> instance.
        /// </summary>
        public byte Liquid { get; set; }

        /// <summary>
        /// Gets or sets the liquid type of this <see cref="Tile" /> instance.
        /// </summary>
        public int LiquidType
        {
            get => (BTileHeader & 0x60) >> 5;
            set => BTileHeader = (byte)((BTileHeader & ~0x60) | ((value & 3) << 5));
        }

        /// <summary>
        /// Gets or sets the slope of this <see cref="Tile" /> instance.
        /// </summary>
        public int Slope
        {
            get => (STileHeader & 0x7000) >> 12;
            set => STileHeader = (short)((STileHeader & ~0x7000) | ((value & 7) << 12));
        }

        /// <summary>
        /// Gets the short tile header of this <see cref="Tile" /> instance.
        /// </summary>
        public short STileHeader { get; set; }

        /// <summary>
        /// Gets or sets the block type of this <see cref="Tile" /> instance.
        /// </summary>
        public ushort Type { get; set; }

        /// <summary>
        /// Gets or sets the wall type of this <see cref="Tile" /> instance.
        /// </summary>
        public byte Wall { get; set; }

        /// <summary>
        /// Gets or sets the wall color of this <see cref="Tile" /> instance.
        /// </summary>
        public byte WallColor
        {
            get => (byte)(BTileHeader & 0x1f);
            set => BTileHeader = (byte)((BTileHeader & ~0x1f) | value);
        }

        /// <summary>
        /// Tests the two specified tiles for equality.
        /// </summary>
        /// <param name="tile">The first tile.</param>
        /// <param name="tile2">The second tile.</param>
        /// <returns><c>true</c> if the two tiles are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Tile tile, Tile tile2) => tile.Equals(tile2);

        /// <summary>
        /// Tests the two specified tiles for inequality.
        /// </summary>
        /// <param name="tile">The first tile.</param>
        /// <param name="tile2">The second tile.</param>
        /// <returns><c>true</c> if the two tiles are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Tile tile, Tile tile2) => !tile.Equals(tile2);

        /// <summary>
        /// Determines whether this <see cref="Tile" /> instance equals the specified tile.
        /// </summary>
        /// <param name="other">The other tile.</param>
        /// <returns><c>true</c> if the two are equal; otherwise <c>false</c>.</returns>
        [Pure]
        public bool Equals(Tile other) =>
            BTileHeader == other.BTileHeader && FrameX == other.FrameX && FrameY == other.FrameY &&
            Liquid == other.Liquid && STileHeader == other.STileHeader && Type == other.Type && Wall == other.Wall;

        /// <summary>
        /// Determines whether this <see cref="Tile" /> instance equals the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the two are equal; otherwise, <c>false</c>.</returns>
        [Pure]
        public override bool Equals(object obj) => obj is Tile tile && Equals(tile);

        /// <summary>
        /// Returns the hash code for this <see cref="Tile" /> instance.
        /// </summary>
        /// <returns>The hash code.</returns>
        [Pure]
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = BTileHeader.GetHashCode();
                hashCode = (hashCode * 397) ^ FrameX.GetHashCode();
                hashCode = (hashCode * 397) ^ FrameY.GetHashCode();
                hashCode = (hashCode * 397) ^ Liquid.GetHashCode();
                hashCode = (hashCode * 397) ^ STileHeader.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                hashCode = (hashCode * 397) ^ Wall.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// Returns the string representation of this <see cref="Tile" /> instance.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"Block: {Type} ({FrameX}, {FrameY}), Wall: {Wall}";
    }
}
