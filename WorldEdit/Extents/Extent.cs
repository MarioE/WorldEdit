namespace WorldEdit.Extents
{
    /// <summary>
    /// Specifies a tile container.
    /// </summary>
    public abstract class Extent
    {
        /// <summary>
        /// Gets a lower bound position on the extent.
        /// </summary>
        public abstract Vector LowerBound { get; }

        /// <summary>
        /// Gets a non-inclusive upper bound position on the extent.
        /// </summary>
        public abstract Vector UpperBound { get; }

        /// <summary>
        /// Returns the tile located at the specified coordinates. This method will -not- check bounds.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The tile.</returns>
        public abstract Tile GetTile(int x, int y);

        /// <summary>
        /// Returns the tile located at the specified position. This method will -not- check bounds.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The tile.</returns>
        public Tile GetTile(Vector position) => GetTile(position.X, position.Y);

        /// <summary>
        /// Determines whether the specified coordinates are in the bounds of the extent.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns><c>true</c> if the coordinates are in the bounds; otherwise, <c>false</c>.</returns>
        public bool IsInBounds(int x, int y) =>
            LowerBound.X <= x && x < UpperBound.X && LowerBound.Y <= y && y < UpperBound.Y;

        /// <summary>
        /// Determines whether the specified position is in in the bounds of the extent.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><c>true</c> if the position is in the bounds; otherwise, <c>false</c>.</returns>
        public bool IsInBounds(Vector position) => IsInBounds(position.X, position.Y);

        /// <summary>
        /// Sets the tile located at the specified coordinates. This method will -not- check bounds.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <param name="tile">The tile.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        public abstract bool SetTile(int x, int y, Tile tile);

        /// <summary>
        /// Sets the tile located at the specified position. This method will -not- check bounds.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="tile">The tile.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        public bool SetTile(Vector position, Tile tile) => SetTile(position.X, position.Y, tile);
    }
}
