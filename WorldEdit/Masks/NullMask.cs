using WorldEdit.Extents;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Represents a mask that always tests <c>true</c>.
    /// </summary>
    public class NullMask : Mask
    {
        /// <inheritdoc />
        protected override bool TestImpl(Extent extent, Vector position) => true;
    }
}
