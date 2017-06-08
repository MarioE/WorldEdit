using System;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Represents a tile state parser.
    /// </summary>
    public sealed class TileStateParser : TemplateParser
    {
        /// <inheritdoc />
        public override ITemplate Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var s2 = s;
            if (s2.StartsWith("!", StringComparison.OrdinalIgnoreCase))
            {
                s2 = "Not " + s2.Substring(1);
            }
            return (ITemplate)typeof(TileState).GetField(s2.ToPascalCase())?.GetValue(null);
        }
    }
}
