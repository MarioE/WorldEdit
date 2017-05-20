using WorldEdit.Core.Extents;

namespace WorldEdit.Core.Masks
{
    /// <summary>
    /// Represents a mask that always tests <c>true</c>.
    /// </summary>
    public sealed class NullMask : Mask
    {
        /// <inheritdoc />
        public override bool Test(Extent extent, Vector position) => true;
    }
}
