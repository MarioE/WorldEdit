using System;
using System.Collections;
using System.Collections.Generic;
using WorldEdit.Extents;

namespace WorldEdit.History
{
    /// <summary>
    /// Specifies a set of changes that can be collectively undone and redone.
    /// </summary>
    public class ChangeSet : IEnumerable<Change>
    {
        private readonly List<Change> _changes = new List<Change>();

        /// <summary>
        /// Adds the specified change to the change set.
        /// </summary>
        /// <param name="change">The change to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change" /> is <c>null</c>.</exception>
        public void Add(Change change)
        {
            _changes.Add(change ?? throw new ArgumentNullException(nameof(change)));
        }

        /// <summary>
        /// Returns an enumerator iterating through the changes.
        /// </summary>
        /// <returns>An enumerator for the change set.</returns>
        public IEnumerator<Change> GetEnumerator() => _changes.GetEnumerator();

        /// <summary>
        /// Redoes the changes onto the specified extent. The changes will be redone in forwards order.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns>The number of redone changes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public int Redo(Extent extent)
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
        /// Undoes the changes onto the specified extent. The changes will be undone in backwards order.
        /// </summary>
        /// <param name="extent">The extent to modify.</param>
        /// <returns>The number of undone changes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public int Undo(Extent extent)
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
