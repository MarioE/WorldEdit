using WorldEdit;
using WorldEdit.Extents;
using WorldEdit.Masks;

namespace WorldEditTests.Masks
{
    public class MockMask : Mask
    {
        public override bool Test(Extent extent, Vector position) => true;
    }
}
