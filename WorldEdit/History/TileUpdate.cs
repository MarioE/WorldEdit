using WorldEdit.Extents;

namespace WorldEdit.History
{
    /// <summary>
    ///     Represents a tile update.
    /// </summary>
    public sealed class TileUpdate : IChange
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TileUpdate" /> class with the specified position and tiles.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="oldTile">The old tile.</param>
        /// <param name="newTile">The new tile.</param>
        public TileUpdate(Vector position, Tile oldTile, Tile newTile)
        {
            Position = position;
            OldTile = oldTile;
            NewTile = newTile;
        }

        /// <summary>
        ///     Gets the new tile.
        /// </summary>
        public Tile NewTile { get; }

        /// <summary>
        ///     Gets the old tile.
        /// </summary>
        public Tile OldTile { get; }

        /// <summary>
        ///     Gets the position.
        /// </summary>
        public Vector Position { get; }

        /// <inheritdoc />
        public bool Redo(Extent extent) => extent.SetTile(Position, NewTile);

        /// <inheritdoc />
        public bool Undo(Extent extent) => extent.SetTile(Position, OldTile);
    }
}
