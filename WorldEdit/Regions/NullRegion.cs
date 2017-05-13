using System.Collections.Generic;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Represents a null region that contains no positions.
    /// </summary>
    public class NullRegion : Region
    {
        /// <inheritdoc />
        public override Vector LowerBound => Vector.Zero;

        /// <inheritdoc />
        public override Vector UpperBound => Vector.Zero;

        /// <inheritdoc />
        public override bool Contains(Vector position) => false;

        /// <inheritdoc />
        public override Region Contract(Vector delta) => this;

        /// <inheritdoc />
        public override Region Expand(Vector delta) => this;

        /// <inheritdoc />
        public override IEnumerator<Vector> GetEnumerator()
        {
            yield break;
        }

        /// <inheritdoc />
        public override Region Inset(int delta) => this;

        /// <inheritdoc />
        public override Region Outset(int delta) => this;

        /// <inheritdoc />
        public override Region Shift(Vector displacement) => this;
    }
}
