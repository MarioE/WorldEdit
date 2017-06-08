using System;
using JetBrains.Annotations;
using Terraria.ID;

#pragma warning disable 1591

namespace WorldEdit
{
    /// <summary>
    ///     Represents an item.
    /// </summary>
    public struct Item : IEquatable<Item>
    {
        public static readonly Item BlueWrench = new Item(ItemID.BlueWrench);
        public static readonly Item GreenWrench = new Item(ItemID.GreenWrench);
        public static readonly Item RedWrench = new Item(ItemID.Wrench);
        public static readonly Item Wire = new Item(ItemID.Wire);

        /// <summary>
        ///     Initializes a new instance of the <see cref="Item" /> structure with the specified type, stack size, and prefix.
        /// </summary>
        /// <param name="type">The item ID.</param>
        /// <param name="stackSize">The stack size.</param>
        /// <param name="prefix">The prefix.</param>
        public Item(int type, int stackSize = 1, byte prefix = 0)
        {
            Type = type;
            StackSize = stackSize;
            Prefix = prefix;
        }

        /// <summary>
        ///     Gets the prefix.
        /// </summary>
        public byte Prefix { get; }

        /// <summary>
        ///     Gets the stack size.
        /// </summary>
        public int StackSize { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public int Type { get; }

        /// <summary>
        ///     Tests the two specified items for equality.
        /// </summary>
        /// <param name="item">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <returns><c>true</c> if the two items are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Item item, Item item2) => item.Equals(item2);

        /// <summary>
        ///     Tests the two specified items for inequality.
        /// </summary>
        /// <param name="item">The first item.</param>
        /// <param name="item2">The second item.</param>
        /// <returns><c>true</c> if the two items are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Item item, Item item2) => !item.Equals(item2);

        /// <summary>
        ///     Determines whether the item equals the specified item.
        /// </summary>
        /// <param name="other">The other item.</param>
        /// <returns><c>true</c> if the two are equal; otherwise <c>false</c>.</returns>
        public bool Equals(Item other) => Prefix == other.Prefix && StackSize == other.StackSize && Type == other.Type;

        /// <summary>
        ///     Determines whether the item equals the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the two are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => obj is Item item && Equals(item);

        /// <summary>
        ///     Returns the hash code of the tile.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Prefix.GetHashCode();
                hashCode = (hashCode * 397) ^ StackSize;
                hashCode = (hashCode * 397) ^ Type;
                return hashCode;
            }
        }

        /// <summary>
        ///     Returns the string representation of the item.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"{Type} x{StackSize}, prefix {Prefix}";

        /// <summary>
        ///     Creates a new <see cref="Item" /> structure from this item with the specified prefix.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>The item.</returns>
        [Pure]
        public Item WithPrefix(byte prefix) => new Item(Type, StackSize, prefix);

        /// <summary>
        ///     Creates a new <see cref="Item" /> structure from this item with the specified stack size.
        /// </summary>
        /// <param name="stackSize">The stack size.</param>
        /// <returns>The item.</returns>
        [Pure]
        public Item WithStackSize(int stackSize) => new Item(Type, stackSize, Prefix);
    }
}
