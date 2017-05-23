using System;
using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents an entry in a <see cref="Pattern{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The template type, which must contain a static method with signature <c>T TryParse(string)</c>.</typeparam>
    public sealed class PatternEntry<T> where T : class, ITemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternEntry{T}" /> class with the specified template and weight.
        /// </summary>
        /// <param name="template">The template, which must not be <c>null</c>.</param>
        /// <param name="weight">The weight, which must be positive.</param>
        /// <exception cref="ArgumentNullException"><paramref name="template" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="weight" /> is not positive.</exception>
        public PatternEntry([NotNull] T template, int weight)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            Weight = weight > 0
                ? weight
                : throw new ArgumentOutOfRangeException(nameof(weight), "Weight cannot be zero or negative.");
        }

        /// <summary>
        /// Gets the template of this <see cref="PatternEntry{T}" /> instance.
        /// </summary>
        [NotNull]
        public T Template { get; }

        /// <summary>
        /// Gets the weight of this <see cref="PatternEntry{T}" /> instance.
        /// </summary>
        public int Weight { get; }

        /// <summary>
        /// Tries to parse the specified string into a <see cref="PatternEntry{T}" /> instance. A return value of <c>null</c>
        /// indicates failure.
        /// </summary>
        /// <param name="s">The string to parse, which must not be <c>null</c>.</param>
        /// <returns>The pattern entry, or <c>null</c> if parsing failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        /// <remarks>
        /// This method will parse strings of the form "template" or "weight*template", where weight is a positive integer and
        /// template parses into a template instance of type <typeparamref name="T" />.
        /// </remarks>
        [CanBeNull]
        [Pure]
        public static PatternEntry<T> TryParse([NotNull] string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var s2 = s;
            var weight = 1;
            var split = s.Split('*');
            if (split.Length == 2)
            {
                if (!int.TryParse(split[0], out weight) || weight <= 0)
                {
                    return null;
                }
                s2 = split[1];
            }

            var template = typeof(T).GetMethod("TryParse")?.Invoke(null, new object[] {s2});
            if (template == null)
            {
                return null;
            }
            return new PatternEntry<T>((T)template, weight);
        }
    }
}
