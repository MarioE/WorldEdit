using WorldEdit.Core.Extents;

namespace WorldEdit.Core.Tools
{
    /// <summary>
    /// Specifies a tool that can be applied to extents at positions.
    /// </summary>
    public interface ITool
    {
        /// <summary>
        /// Applies the tool to the specified extent at a position. For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <param name="position">The position.</param>
        /// <returns>The number of modifications.</returns>
        int Apply(Extent extent, Vector position);
    }
}
