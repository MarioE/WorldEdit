using WorldEdit.Extents;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Specifies a mechanism that tests extents at positions.
    /// </summary>
    public abstract class Mask
    {
        /// <summary>
        /// Tests the specified extent at a position.
        /// <para />
        /// For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="extent">The extent to test.</param>
        /// <param name="position">The position.</param>
        /// <returns>The test result.</returns>
        public abstract bool Test(Extent extent, Vector position);
    }
}
