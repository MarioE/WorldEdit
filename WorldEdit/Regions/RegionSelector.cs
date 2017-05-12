namespace WorldEdit.Regions
{
    /// <summary>
    /// Holds selected positions, creating regions from them.
    /// </summary>
    public abstract class RegionSelector
    {
        /// <summary>
        /// Gets the primary position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? PrimaryPosition { get; protected set; }

        /// <summary>
        /// Gets the secondary position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? SecondaryPosition { get; protected set; }

        /// <summary>
        /// Clears the selected positions.
        /// </summary>
        public void Clear()
        {
            PrimaryPosition = SecondaryPosition = null;
        }

        /// <summary>
        /// Selects the primary position and returns the resulting region.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region.</returns>
        public Region SelectPrimary(Vector position)
        {
            PrimaryPosition = position;
            return GetRegion();
        }

        /// <summary>
        /// Selects the secondary position and returns the resulting region.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region.</returns>
        public Region SelectSecondary(Vector position)
        {
            SecondaryPosition = position;
            return GetRegion();
        }

        /// <summary>
        /// Gets the defined region, or an instance of <see cref="NullRegion" /> if it is not possible.
        /// </summary>
        /// <returns>The defined region.</returns>
        protected abstract Region GetRegion();
    }
}
