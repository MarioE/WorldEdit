using WorldEdit.Extents;

namespace WorldEdit.Tools
{
    /// <summary>
    /// Represents a null tool that does nothing.
    /// </summary>
    public sealed class NullTool : ITool
    {
        /// <inheritdoc />
        public int Apply(Extent extent, Vector position) => 0;
    }
}
