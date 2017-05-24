using JetBrains.Annotations;
using WorldEdit.Extents;

namespace WorldEdit.History
{
    /// <summary>
    /// Specifies a change that can be undone or redone on an extent.
    /// </summary>
    public interface IChange
    {
        /// <summary>
        /// Redoes this <see cref="IChange" /> instance on the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify, which must not be <c>null</c>.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        bool Redo([NotNull] Extent extent);

        /// <summary>
        /// Undoes this <see cref="IChange" /> instance on the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify, which must not be <c>null</c>.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        bool Undo([NotNull] Extent extent);
    }
}
