using System;
using WorldEdit.Extents;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Specifies a mechanism that tests extents at positions.
    /// </summary>
    public abstract class Mask
    {
        /// <summary>
        /// Tests the specified extent at a position. If the position is out of the bounds of the extent, then the test fails.
        /// </summary>
        /// <param name="extent">The extent to test.</param>
        /// <param name="position">The position.</param>
        /// <returns>The test result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public bool Test(Extent extent, Vector position)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            return extent.IsInBounds(position) && TestImpl(extent, position);
        }

        /// <summary>
        /// Tests the specified extent at a position.
        /// </summary>
        /// <param name="extent">The extent to test.</param>
        /// <param name="position">The position.</param>
        /// <returns>The test result.</returns>
        protected abstract bool TestImpl(Extent extent, Vector position);
    }
}
