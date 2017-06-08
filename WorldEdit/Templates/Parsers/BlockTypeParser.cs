using System;

namespace WorldEdit.Templates.Parsers
{
    /// <summary>
    ///     Represents a block type parser.
    /// </summary>
    public sealed class BlockTypeParser : TemplateParser
    {
        /// <inheritdoc />
        public override ITemplate Parse(string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var split = s.Split(':');
            if (split.Length == 3)
            {
                var blockType = (BlockType)Parse(split[0]);
                if (!short.TryParse(split[1], out var frameX) || !short.TryParse(split[2], out var frameY))
                {
                    return null;
                }
                return blockType?.WithFrames(frameX, frameY);
            }

            if (ushort.TryParse(s, out var id) && id < BlockType.MaxId)
            {
                return BlockType.FromId(id);
            }
            return (ITemplate)typeof(BlockType).GetField(s.ToPascalCase())?.GetValue(null);
        }
    }
}
