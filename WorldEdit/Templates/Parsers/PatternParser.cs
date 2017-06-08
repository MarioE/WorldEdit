using System;
using System.Linq;
using JetBrains.Annotations;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Represents a pattern parser.
    /// </summary>
    public sealed class PatternParser : TemplateParser
    {
        private readonly TemplateParser _parser;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PatternParser" /> class with the specified template parser.
        /// </summary>
        /// <param name="parser">The template parser, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parser" /> is <c>null</c>.</exception>
        public PatternParser([NotNull] TemplateParser parser)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        /// <inheritdoc />
        public override ITemplate Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var entries = s.Split(',').Select(ParseEntry).ToList();
            if (entries.Any(e => e == null))
            {
                return null;
            }
            return new Pattern(entries);
        }

        private PatternEntry ParseEntry(string s)
        {
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

            var template = _parser.Parse(s2);
            if (template == null)
            {
                return null;
            }
            return new PatternEntry(template, weight);
        }
    }
}
