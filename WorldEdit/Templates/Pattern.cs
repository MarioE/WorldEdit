using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    ///     Represents a randomized pattern of templates.
    /// </summary>
    public sealed class Pattern : ITemplate
    {
        private readonly Random _random = new Random();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Pattern" /> class with the specified pattern entries.
        /// </summary>
        /// <param name="entries">The entries, which must not be <c>null</c> or contain <c>null</c>.</param>
        /// <exception cref="ArgumentException"><paramref name="entries" /> contains <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="entries" /> is <c>null</c>.</exception>
        public Pattern([ItemNotNull] [NotNull] IEnumerable<PatternEntry> entries)
        {
            if (entries == null)
            {
                throw new ArgumentNullException(nameof(entries));
            }

            Entries = entries.ToList().AsReadOnly();
            if (Entries.Contains(null))
            {
                throw new ArgumentException("Null entries are not allowed.", nameof(entries));
            }
        }

        /// <summary>
        ///     Gets a read-only view of the pattern entries.
        /// </summary>
        [ItemNotNull]
        [NotNull]
        public IReadOnlyList<PatternEntry> Entries { get; }

        /// <inheritdoc />
        public Tile Apply(Tile tile)
        {
            var rand = _random.Next(Entries.Sum(e => e.Weight));
            var current = 0;
            foreach (var entry in Entries)
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
        public bool Matches(Tile tile) => Entries.Any(e => e.Template.Matches(tile));
    }
}
