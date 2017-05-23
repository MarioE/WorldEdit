using JetBrains.Annotations;
using WorldEdit.Extents;

namespace WorldEdit.Tools
{
    /// <summary>
    /// Specifies a tool that can be applied to extents at positions.
    /// </summary>
    public interface ITool
    {
        /// <summary>
        /// Applies this <see cref="ITool" /> instance to the specified extent at a position.
        /// </summary>
        /// <param name="extent">The extent to modify, which must not be <c>null</c>.</param>
        /// <param name="position">The position.</param>
        /// <returns>The number of modifications.</returns>
        int Apply([NotNull] Extent extent, Vector position);
    }
}
