using JetBrains.Annotations;
using WorldEdit.Extents;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Specifies a mask that tests extents at positions.
    /// </summary>
    public abstract class Mask
    {
        /// <summary>
        /// Tests this <see cref="Mask" /> instance at the specified extent at a position.
        /// </summary>
        /// <param name="extent">The extent to test, which must not be <c>null</c>.</param>
        /// <param name="position">The position, which must be within the bounds of the extent.</param>
        /// <returns>The test result.</returns>
        [Pure]
        public abstract bool Test([NotNull] Extent extent, Vector position);
    }
}
