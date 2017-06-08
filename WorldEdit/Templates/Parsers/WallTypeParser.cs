using System;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Represents a wall type parser.
    /// </summary>
    public sealed class WallTypeParser : TemplateParser
    {
        /// <inheritdoc />
        public override ITemplate Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            if (byte.TryParse(s, out var id) && id < WallType.MaxId)
            {
                return WallType.FromId(id);
            }
            return (ITemplate)typeof(WallType).GetField(s.ToPascalCase())?.GetValue(null);
        }
    }
}
