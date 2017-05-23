using JetBrains.Annotations;

namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    /// Holds selected positions, creating regions from them.
    /// </summary>
    public abstract class RegionSelector
    {
        /// <summary>
        /// Gets the primary position of this <see cref="RegionSelector" /> instance, or <c>null</c> if it is not selected.
        /// </summary>
        [CanBeNull]
        public abstract Vector? PrimaryPosition { get; }

        /// <summary>
        /// Clears the selected positions of this <see cref="RegionSelector" /> instance.
        /// </summary>
        /// <returns>The resulting region selector.</returns>
        [NotNull]
        [Pure]
        public abstract RegionSelector Clear();

        /// <summary>
        /// Gets the region defined by this <see cref="RegionSelector" /> instance.
        /// </summary>
        /// <returns>The defined region.</returns>
        [NotNull]
        [Pure]
        public abstract Region GetRegion();

        /// <summary>
        /// Selects the primary position of this <see cref="RegionSelector" /> instance using the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region selector.</returns>
        [NotNull]
        [Pure]
        public abstract RegionSelector SelectPrimary(Vector position);

        /// <summary>
        /// Selects the secondary position of this <see cref="RegionSelector" /> instance using the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region selector.</returns>
        [NotNull]
        [Pure]
        public abstract RegionSelector SelectSecondary(Vector position);
    }
}
