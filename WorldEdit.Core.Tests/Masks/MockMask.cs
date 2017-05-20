using WorldEdit.Core.Extents;
using WorldEdit.Core.Masks;

namespace WorldEdit.Core.Tests.Masks
{
    public class MockMask : Mask
    {
        public override bool Test(Extent extent, Vector position) => true;
    }
}
