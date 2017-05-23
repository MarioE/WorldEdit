using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a randomized pattern of template entries.
    /// </summary>
    /// <typeparam name="T">The template type.</typeparam>
    public sealed class Pattern<T> : ITemplate where T : class, ITemplate
    {
        private readonly List<PatternEntry<T>> _entries;
        private readonly Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern{T}" /> class with the specified entries.
        /// </summary>
        /// <param name="entries">The entries, which must not be <c>null</c> or contain <c>null</c>.</param>
        /// <exception cref="ArgumentException"><paramref name="entries" /> contains <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="entries" /> is <c>null</c>.</exception>
        // TODO: consider non-generic pattern entries?
        public Pattern([NotNull] [ItemNotNull] IEnumerable<PatternEntry<T>> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            _entries = entries.ToList();
            if (_entries.Contains(null))
            {
                throw new ArgumentException("Null entries are not allowed.", nameof(entries));
            }
        }

        /// <summary>
        /// Tries to parse the specified string into a <see cref="Pattern{T}" /> instance. A return value of <c>null</c> indicates
        /// failure.
        /// </summary>
        /// <param name="s">The string to parse, which must not be <c>null</c>.</param>
        /// <returns>The pattern, or <c>null</c> if parsing failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        /// <remarks>
        /// This method will parse strings of the form "entry 1, entry 2, ..." where each entry parses into a
        /// <see cref="PatternEntry{T}" /> instance.
        /// </remarks>
        [CanBeNull]
        [Pure]
        public static Pattern<T> TryParse([NotNull] string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var entries = s.Split(',').Select(PatternEntry<T>.TryParse).ToList();
            if (entries.Any(e => e == null))
            {
                return null;
            }
            return new Pattern<T>(entries);
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method randomly picks an entry from the pattern to be applied to the tile.
        /// </remarks>
        public Tile Apply(Tile tile)
        {
            var rand = _random.Next(_entries.Sum(e => e.Weight));
            var current = 0;
            foreach (var entry in _entries)
            {
                if (current <= rand && rand < current + entry.Weight)
                {
                    return entry.Template.Apply(tile);
                }

                current += entry.Weight;
            }
            return tile;
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method checks if any of the entries match the tile.
        /// </remarks>
        public bool Matches(Tile tile) => _entries.Any(e => e.Template.Matches(tile));
    }
}
