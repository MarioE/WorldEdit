using System.Collections;
using System.Collections.Generic;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Defines a region of positions.
    /// </summary>
    public abstract class Region : IEnumerable<Vector>
    {
        /// <summary>
        /// Gets the dimensions of the region.
        /// </summary>
        public Vector Dimensions => UpperBound - LowerBound;

        /// <summary>
        /// Gets a lower bound position on the region.
        /// </summary>
        public abstract Vector LowerBound { get; }

        /// <summary>
        /// Gets a non-inclusive upper bound position on the region.
        /// </summary>
        public abstract Vector UpperBound { get; }

        /// <summary>
        /// Determines whether the region contains the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><c>true</c> if the region contains the position; otherwise, <c>false</c>.</returns>
        public abstract bool Contains(Vector position);

        /// <summary>
        /// Contracts the region by the specified delta. The signs of the delta components indicate the directions of contraction.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        public abstract Region Contract(Vector delta);

        /// <summary>
        /// Expands the region by the specified delta. The signs of the delta components indicate the directions of expansion.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        public abstract Region Expand(Vector delta);

        /// <summary>
        /// Returns an enumerator iterating through the positions in the region.
        /// </summary>
        /// <returns>An enumerator for the region.</returns>
        public abstract IEnumerator<Vector> GetEnumerator();

        /// <summary>
        /// Insets the region by the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        public abstract Region Inset(int delta);

        /// <summary>
        /// Outsets the region by the specified delta.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <returns>The resulting region.</returns>
        public abstract Region Outset(int delta);

        /// <summary>
        /// Shifts the region by the specified displacement.
        /// </summary>
        /// <param name="displacement">The displacement.</param>
        /// <returns>The resulting region.</returns>
        public abstract Region Shift(Vector displacement);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
