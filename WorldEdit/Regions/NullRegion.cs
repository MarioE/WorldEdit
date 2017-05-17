using System;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Represents a null region.
    /// </summary>
    public class NullRegion : Region
    {
        /// <inheritdoc />
        public override bool CanContract => false;

        /// <inheritdoc />
        public override bool CanExpand => false;

        /// <inheritdoc />
        public override bool CanShift => false;

        /// <inheritdoc />
        public override Vector LowerBound => Vector.Zero;

        /// <inheritdoc />
        public override Vector UpperBound => Vector.Zero;

        /// <inheritdoc />
        public override bool Contains(Vector position) => false;

        /// <inheritdoc />
        public override Region Contract(Vector delta) =>
            throw new InvalidOperationException("Null regions cannot contract.");

        /// <inheritdoc />
        public override Region Expand(Vector delta) =>
            throw new InvalidOperationException("Null regions cannot expand.");

        /// <inheritdoc />
        public override Region Inset(int delta) =>
            throw new InvalidOperationException("Null regions cannot contract.");

        /// <inheritdoc />
        public override Region Outset(int delta) =>
            throw new InvalidOperationException("Null regions cannot expand.");

        /// <inheritdoc />
        public override Region Shift(Vector displacement) =>
            throw new InvalidOperationException("Null regions cannot shift.");
    }
}
