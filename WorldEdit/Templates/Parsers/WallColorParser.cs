using System;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Represents a wall color parser.
    /// </summary>
    public sealed class WallColorParser : TemplateParser
    {
        /// <inheritdoc />
        public override ITemplate Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return (ITemplate)typeof(WallColor).GetField(s.ToPascalCase())?.GetValue(null);
        }
    }
}
