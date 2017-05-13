using System.Collections.Generic;
using WorldEdit;
using WorldEdit.Regions;

namespace WorldEditTests.Regions
{
    public class MockRegion : Region
    {
        public override Vector LowerBound => Vector.Zero;
        public override Vector UpperBound => Vector.One;

        public override bool Contains(Vector position) => false;

        public override Region Contract(Vector delta) => this;

        public override Region Expand(Vector delta) => this;

        public override IEnumerator<Vector> GetEnumerator()
        {
            yield break;
        }

        public override Region Inset(int delta) => this;

        public override Region Outset(int delta) => this;

        public override Region Shift(Vector displacement) => this;
    }
}
