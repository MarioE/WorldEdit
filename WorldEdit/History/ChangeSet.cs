using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using WorldEdit.Extents;

namespace WorldEdit.History
{
    /// <summary>
    /// Represents a set of changes that can be undone and redone collectively.
    /// </summary>
    public sealed class ChangeSet : IEnumerable<IChange>
    {
        private readonly List<IChange> _changes = new List<IChange>();

        /// <summary>
        /// Adds the specified change to this <see cref="ChangeSet" /> instance.
        /// </summary>
        /// <param name="change">The change to add, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change" /> is <c>null</c>.</exception>
        public void Add([NotNull] IChange change)
        {
            if (change == null)
            {
                throw new ArgumentNullException(nameof(change));
            }

            _changes.Add(change);
        }

        /// <summary>
        /// Returns an enumerator iterating through the changes in this <see cref="ChangeSet" /> instance.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<IChange> GetEnumerator() => _changes.GetEnumerator();

        /// <summary>
        /// Redoes the changes in this <see cref="ChangeSet" /> instance onto the specified extent. The changes will be redone in
        /// forwards order.
        /// </summary>
        /// <param name="extent">The extent to modify, which must not be <c>null</c>.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public int Redo([NotNull] Extent extent)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            var count = 0;
            foreach (var change in _changes)
            {
                if (change.Redo(extent))
                {
                    ++count;
                }
            }
            return count;
        }

        /// <summary>
        /// Undoes the changes in this <see cref="ChangeSet" /> instance onto the specified extent. The changes will be redone in
        /// backwards order.
        /// </summary>
        /// <param name="extent">The extent to modify, which must not be <c>null</c>.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public int Undo([NotNull] Extent extent)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            var count = 0;
            for (var i = _changes.Count - 1; i >= 0; --i)
            {
                if (_changes[i].Undo(extent))
                {
                    ++count;
                }
            }
            return count;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
