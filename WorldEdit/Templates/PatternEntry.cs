using System;

namespace WorldEdit.Templates
{
    /// <summary>
    /// Represents an entry in a pattern.
    /// </summary>
    /// <typeparam name="T">The template type.</typeparam>
    public class PatternEntry<T> where T : class, ITemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternEntry{T}" /> class with the specified template and weight.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="weight">The weight.</param>
        public PatternEntry(T template, int weight)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
            Weight = weight > 0
                ? weight
                : throw new ArgumentOutOfRangeException(nameof(weight), "Positive number required.");
        }

        /// <summary>
        /// Gets the template.
        /// </summary>
        public T Template { get; }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        public int Weight { get; }
    }
}
