using System;
using JetBrains.Annotations;

namespace WorldEdit.Templates
{
    /// <summary>
    ///     Represents an entry in a pattern.
    /// </summary>
    public sealed class PatternEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PatternEntry" /> class with the specified template and weight.
        /// </summary>
        /// <param name="template">The template, which must not be <c>null</c>.</param>
        /// <param name="weight">The weight, which must be positive.</param>
        /// <exception cref="ArgumentNullException"><paramref name="template" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="weight" /> is not positive.</exception>
        public PatternEntry([NotNull] ITemplate template, int weight)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            Weight = weight > 0
                ? weight
                : throw new ArgumentOutOfRangeException(nameof(weight), "Weight cannot be zero or negative.");
        }

        /// <summary>
        ///     Gets the template.
        /// </summary>
        [NotNull]
        public ITemplate Template { get; }

        /// <summary>
        ///     Gets the weight.
        /// </summary>
        public int Weight { get; }
    }
}
