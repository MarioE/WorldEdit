using WorldEdit.Extents;

namespace WorldEdit.History
{
    /// <summary>
    /// Represents a tile change at a position.
    /// </summary>
    public class TileChange : Change
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileChange" /> class with the specified position and tiles.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="oldTile">The old tile.</param>
        /// <param name="newTile">The new tile.</param>
        public TileChange(Vector position, Tile oldTile, Tile newTile)
        {
            Position = position;
            OldTile = oldTile;
            NewTile = newTile;
        }

        /// <summary>
        /// Gets the new tile.
        /// </summary>
        public Tile NewTile { get; }

        /// <summary>
        /// Gets the old tile.
        /// </summary>
        public Tile OldTile { get; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public Vector Position { get; }

        /// <inheritdoc />
        protected override bool RedoImpl(Extent extent) =>
            extent.IsInBounds(Position) && extent.SetTile(Position, NewTile);

        /// <inheritdoc />
        protected override bool UndoImpl(Extent extent) =>
            extent.IsInBounds(Position) && extent.SetTile(Position, OldTile);
    }
}
