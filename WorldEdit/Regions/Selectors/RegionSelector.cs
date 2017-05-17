namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    /// Holds selected positions, creating regions from them.
    /// </summary>
    public abstract class RegionSelector
    {
        /// <summary>
        /// Gets the primary position or <c>null</c> if it is not selected.
        /// </summary>
        public abstract Vector? PrimaryPosition { get; }

        /// <summary>
        /// Clears the selected positions.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Selects the primary position and returns the resulting region.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region.</returns>
        public abstract Region SelectPrimary(Vector position);

        /// <summary>
        /// Selects the secondary position and returns the resulting region.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region.</returns>
        public abstract Region SelectSecondary(Vector position);
    }
}
