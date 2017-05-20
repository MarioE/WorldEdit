using WorldEdit.Core;
using WorldEdit.Core.Regions;

namespace WorldEdit.Tests.Regions
{
    public class MockRegion : Region
    {
        public override bool CanContract => true;
        public override bool CanExpand => true;
        public override bool CanShift => true;
        public override Vector LowerBound => Vector.Zero;
        public override Vector UpperBound => Vector.One;

        public override bool Contains(Vector position) => false;
        public override Region Contract(Vector delta) => this;
        public override Region Expand(Vector delta) => this;
        public override Region Inset(int delta) => this;
        public override Region Outset(int delta) => this;
        public override Region Shift(Vector displacement) => this;
    }
}
