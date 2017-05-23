using WorldEdit.Extents;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Represents an empty mask.
    /// </summary>
    public sealed class EmptyMask : Mask
    {
        /// <inheritdoc />
        public override bool Test(Extent extent, Vector position) => true;
    }
}
