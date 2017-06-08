using System;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Represents a block color parser.
    /// </summary>
    public sealed class BlockColorParser : TemplateParser
    {
        /// <inheritdoc />
        public override ITemplate Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return (ITemplate)typeof(BlockColor).GetField(s.ToPascalCase())?.GetValue(null);
        }
    }
}
