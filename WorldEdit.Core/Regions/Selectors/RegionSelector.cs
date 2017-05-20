namespace WorldEdit.Core.Regions.Selectors
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
        /// <returns>The resulting region selector.</returns>
        public abstract RegionSelector Clear();

        /// <summary>
        /// Gets the defined region.
        /// </summary>
        /// <returns>The defined region.</returns>
        public abstract Region GetRegion();

        /// <summary>
        /// Selects the primary position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region selector.</returns>
        public abstract RegionSelector SelectPrimary(Vector position);

        /// <summary>
        /// Selects the secondary position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region selector.</returns>
        public abstract RegionSelector SelectSecondary(Vector position);
    }
}
