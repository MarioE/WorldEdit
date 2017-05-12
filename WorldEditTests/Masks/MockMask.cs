using WorldEdit;
using WorldEdit.Extents;
using WorldEdit.Masks;

namespace WorldEditTests.Masks
{
    public class MockMask : Mask
    {
        protected override bool TestImpl(Extent extent, Vector position)
        {
            return true;
        }
    }
}
