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
        /// Gets or sets the tile located at the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        /// <returns>The tile located at the coordinates.</returns>
        public abstract Tile this[int x, int y] { get; set; }

        /// <summary>
        /// Gets or sets the tile located at the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The tile located at the position.</returns>
        public Tile this[Vector position]
        {
            get => this[position.X, position.Y];
            set => this[position.X, position.Y] = value;
        }

        /// <summary>
        /// Determines whether a position is in in the bounds of the extent.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><c>true</c> if the position is in the bounds; otherwise, <c>false</c>.</returns>
        public bool IsInBounds(Vector position)
        {
            return LowerBound.X <= position.X && position.X < UpperBound.X &&
                   LowerBound.Y <= position.Y && position.Y < UpperBound.Y;
        }
    }
}
