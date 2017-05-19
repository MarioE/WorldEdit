using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="entries">The entries.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entries" /> is <c>null</c>.</exception>
        public Pattern(IEnumerable<PatternEntry<T>> entries)
        {
            _entries = entries?.ToList() ?? throw new ArgumentNullException(nameof(entries));
        }

        /// <summary>
        /// Parses the specified string into a pattern.
        /// </summary>
        /// <param name="s">The string to parse.</param>
        /// <returns>The result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static Result<Pattern<T>> Parse(string s)
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
                var split = s2.Split('*');
                if (split.Length == 2)
                {
                    var inputWeight = split[0];
                    if (!int.TryParse(inputWeight, out weight) || weight <= 0)
                    {
                        return Result.FromError<Pattern<T>>($"Invalid weight '{inputWeight}'.");
                    }

                    s3 = split[1];
                }

                var templateResult = (Result<T>)typeof(T).GetMethod("Parse").Invoke(null, new object[] {s3});
                if (!templateResult.WasSuccessful)
                {
                    return Result.FromError<Pattern<T>>(templateResult.ErrorMessage);
                }

                entries.Add(new PatternEntry<T>(templateResult.Value, weight));
            }
            return Result.From(new Pattern<T>(entries));
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

        /// <inheritdoc />
        public bool Matches(Tile tile) => _entries.Any(e => e.Template.Matches(tile));
    }
}
