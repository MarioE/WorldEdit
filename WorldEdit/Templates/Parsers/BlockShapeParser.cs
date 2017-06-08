using System;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Represents a block shape parser.
    /// </summary>
    public sealed class BlockShapeParser : TemplateParser
    {
        /// <inheritdoc />
        public override ITemplate Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return (ITemplate)typeof(BlockShape).GetField(s.ToPascalCase())?.GetValue(null);
        }
    }
}
