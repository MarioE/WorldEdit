namespace WorldEdit.TileEntities
{
    /// <summary>
    ///     Represents an item frame tile entity.
    /// </summary>
    public sealed class ItemFrame : ITileEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemFrame" /> class with the specified position and item.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="item">The item.</param>
        public ItemFrame(Vector position, Item item)
        {
            Position = position;
            Item = item;
        }

        /// <summary>
        ///     Gets the item.
        /// </summary>
        public Item Item { get; }

        /// <inheritdoc />
        public Vector Position { get; }

        /// <inheritdoc />
        public ITileEntity WithPosition(Vector position) => new ItemFrame(position, Item);
    }
}
