using System;
using JetBrains.Annotations;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Specifies a template parser.
    /// </summary>
    public abstract class TemplateParser
    {
        /// <summary>
        ///     Parses the specified string into a template. A return value of <c>null</c> indicates failure.
        /// </summary>
        /// <param name="s">The string to parse, which must not be <c>null</c>.</param>
        /// <returns>The template, or <c>null</c> if parsing failed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        [CanBeNull]
        [Pure]
        public abstract ITemplate Parse([NotNull] string s);
    }
}
