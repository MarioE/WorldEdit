using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents a randomized pattern of template entries.
    /// </summary>
    /// <typeparam name="T">The template type.</typeparam>
    public class Pattern<T> : IEnumerable<PatternEntry<T>>, ITemplate where T : class, ITemplate
    {
        private readonly List<PatternEntry<T>> _entries;
        private readonly Random _random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="Pattern{T}" /> class with the specified entries.
        /// </summary>
        /// <param name="entries">The entries.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entries" /> is <c>null</c>.</exception>
        public Pattern(IEnumerable<PatternEntry<T>> entries)
        {
            _entries = (entries ?? throw new ArgumentNullException(nameof(entries))).ToList();
        }

        /// <summary>
        /// Gets a read-only view of the entries.
        /// </summary>
        public ReadOnlyCollection<PatternEntry<T>> Entries => _entries.AsReadOnly();

        /// <summary>
        /// Parses the specified string into a pattern.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static ParsingResult<Pattern<T>> Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var entries = new List<PatternEntry<T>>();
            foreach (var s2 in s.Split(','))
            {
                var s3 = s2;
                var weight = 1;
                var index = s2.IndexOf('*');
                if (index != -1)
                {
                    var inputWeight = s2.Substring(0, index);
                    if (!int.TryParse(inputWeight, out weight) || weight <= 0)
                    {
                        return ParsingResult.FromError<Pattern<T>>($"Invalid weight '{inputWeight}'.");
                    }
                    s3 = s2.Substring(index + 1);
                }

                var result = (ParsingResult<T>)typeof(T).GetMethod("Parse").Invoke(null, new object[] {s3});
                if (!result.WasSuccessful)
                {
                    return ParsingResult.FromError<Pattern<T>>(result.ErrorMessage);
                }
                entries.Add(new PatternEntry<T>(result.Value, weight));
            }
            return ParsingResult.From(new Pattern<T>(entries));
        }

        /// <inheritdoc />
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

        /// <summary>
        /// Gets an enumerator iterating through the pattern entries.
        /// </summary>
        /// <returns>An enumerator for the pattern.</returns>
        public IEnumerator<PatternEntry<T>> GetEnumerator() => _entries.GetEnumerator();

        /// <inheritdoc />
        public bool Matches(Tile tile) => _entries.Any(e => e.Template.Matches(tile));

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
