using System;
using WorldEdit.Extents;

namespace WorldEdit.History
{
    /// <summary>
    /// Specifies a change to an extent that can be undone and redone.
    /// </summary>
    public abstract class Change
    {
        /// <summary>
        /// Redoes the change onto the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public bool Redo(Extent extent) =>
            RedoImpl(extent ?? throw new ArgumentNullException(nameof(extent)));

        /// <summary>
        /// Undoes the change onto the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public bool Undo(Extent extent) =>
            UndoImpl(extent ?? throw new ArgumentNullException(nameof(extent)));

        /// <summary>
        /// Redoes the change onto the specified extent. This method will -not- check for <c>null</c>.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        protected abstract bool RedoImpl(Extent extent);

        /// <summary>
        /// Undoes the change onto the specified extent. This method will -not- check for <c>null</c>.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns><c>true</c> if the operation succeeded; otherwise, <c>false</c>.</returns>
        protected abstract bool UndoImpl(Extent extent);
    }
}
