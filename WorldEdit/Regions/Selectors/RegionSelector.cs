using JetBrains.Annotations;

namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    ///     Specifies a region selector which creates regions from selected positions.
    /// </summary>
    public abstract class RegionSelector
    {
        /// <summary>
        ///     Gets the primary position, or <c>null</c> if it is not selected.
        /// </summary>
        [CanBeNull]
        public abstract Vector? PrimaryPosition { get; }

        /// <summary>
        ///     Creates a new <see cref="RegionSelector" /> instance from this region selector with no selected positions.
        /// </summary>
        /// <returns>The region selector.</returns>
        [NotNull]
        [Pure]
        public abstract RegionSelector Clear();

        /// <summary>
        ///     Gets the defined region.
        /// </summary>
        /// <returns>The defined region.</returns>
        [NotNull]
        [Pure]
        public abstract Region GetRegion();

        /// <summary>
        ///     Creates a new <see cref="RegionSelector" /> instance from this region selector with the specified position as the
        ///     primary position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The region selector.</returns>
        [NotNull]
        [Pure]
        public abstract RegionSelector WithPrimary(Vector position);

        /// <summary>
        ///     Creates a new <see cref="RegionSelector" /> instance from this region selector with the specified position as a
        ///     secondary position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The region selector.</returns>
        [NotNull]
        [Pure]
        public abstract RegionSelector WithSecondary(Vector position);
    }
}
