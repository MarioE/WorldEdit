using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Specifies a region of positions.
    /// </summary>
    public abstract class Region : IEnumerable<Vector>
    {
        /// <summary>
        /// Gets the dimensions.
        /// </summary>
        public Vector Dimensions => UpperBound - LowerBound;

        /// <summary>
        /// Gets the lower bound position.
        /// </summary>
        public abstract Vector LowerBound { get; }

        /// <summary>
        /// Gets the non-inclusive upper bound position.
        /// </summary>
        public abstract Vector UpperBound { get; }

        /// <summary>
        /// Determines whether the specified position is contained.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><c>true</c> if the position is contained; otherwise, <c>false</c>.</returns>
        [Pure]
        public abstract bool Contains(Vector position);

        /// <summary>
        /// Returns an enumerator iterating through the positions contained.
        /// </summary>
        /// <returns>An enumerator.</returns>
        [Pure]
        public IEnumerator<Vector> GetEnumerator()
        {
            for (var x = LowerBound.X; x < UpperBound.X; ++x)
            {
                for (var y = LowerBound.Y; y < UpperBound.Y; ++y)
                {
                    var position = new Vector(x, y);
                    if (Contains(position))
                    {
                        yield return position;
                    }
                }
            }
        }

        /// <summary>
        /// Shifts the region by the specified displacement.
        /// </summary>
        /// <param name="displacement">The displacement.</param>
        /// <returns>The resulting region.</returns>
        [NotNull]
        [Pure]
        public abstract Region Shift(Vector displacement);

        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
