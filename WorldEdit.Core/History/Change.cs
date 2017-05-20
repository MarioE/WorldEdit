using WorldEdit.Core.Extents;

namespace WorldEdit.Core.History
{
    /// <summary>
    /// Specifies a change to an extent that can be undone and redone.
    /// </summary>
    public abstract class Change
    {
        /// <summary>
        /// Redoes the change onto the specified extent.
        /// <para />
        /// For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        public abstract bool Redo(Extent extent);

        /// <summary>
        /// Undoes the change onto the specified extent.
        /// <para />
        /// For speed purposes, this method has -no- validation!
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        public abstract bool Undo(Extent extent);
    }
}
