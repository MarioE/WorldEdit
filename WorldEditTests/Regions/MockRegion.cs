using System.Collections.Generic;
using WorldEdit;
using WorldEdit.Regions;

namespace WorldEditTests.Regions
{
    public class MockRegion : Region
    {
        public override Vector LowerBound => Vector.Zero;
        public override Vector UpperBound => Vector.One;

        public override bool Contains(Vector position)
        {
            return false;
        }

        public override Region Contract(Vector delta)
        {
            return this;
        }

        public override Region Expand(Vector delta)
        {
            return this;
        }

        public override IEnumerator<Vector> GetEnumerator()
        {
            yield break;
        }

        public override Region Inset(int delta)
        {
            return this;
        }

        public override Region Outset(int delta)
        {
            return this;
        }

        public override Region Shift(Vector displacement)
        {
            return this;
        }
    }
}
