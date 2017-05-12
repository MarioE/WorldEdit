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
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public void Redo(Extent extent)
        {
            RedoImpl(extent ?? throw new ArgumentNullException(nameof(extent)));
        }

        /// <summary>
        /// Undoes the change onto the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public void Undo(Extent extent)
        {
            UndoImpl(extent ?? throw new ArgumentNullException(nameof(extent)));
        }

        /// <summary>
        /// Redoes the change onto the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        protected abstract void RedoImpl(Extent extent);

        /// <summary>
        /// Undoes the change onto the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        protected abstract void UndoImpl(Extent extent);
    }
}
