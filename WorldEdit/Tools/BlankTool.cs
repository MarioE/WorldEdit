using WorldEdit.Extents;

namespace WorldEdit.Tools
{
    /// <summary>
    /// Represents a blank tool that does nothing.
    /// </summary>
    public sealed class BlankTool : ITool
    {
        /// <inheritdoc />
        public int Apply(Extent extent, Vector position) => 0;
    }
}
