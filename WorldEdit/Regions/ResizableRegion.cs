using JetBrains.Annotations;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Specifies a resizable region.
    /// </summary>
    public abstract class ResizableRegion : Region
    {
        /// <summary>
        /// Contracts the region by the specified delta. The signs of the delta components indicate the directions of contraction.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        [NotNull]
        [Pure]
        public abstract ResizableRegion Contract(Vector delta);

        /// <summary>
        /// Expands the region by the specified delta. The signs of the delta components indicate the directions of expansion.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        [NotNull]
        [Pure]
        public abstract ResizableRegion Expand(Vector delta);

        /// <summary>
        /// Insets the region by the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        [NotNull]
        [Pure]
        public abstract ResizableRegion Inset(int delta);

        /// <summary>
        /// Outsets the region by the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        [NotNull]
        [Pure]
        public abstract ResizableRegion Outset(int delta);
    }
}
