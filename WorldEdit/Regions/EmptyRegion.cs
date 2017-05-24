namespace WorldEdit.Regions
{
    /// <summary>
    /// Represents an empty region.
    /// </summary>
    public sealed class EmptyRegion : Region
    {
        /// <inheritdoc />
        public override Vector LowerBound => Vector.Zero;

        /// <inheritdoc />
        public override Vector UpperBound => Vector.Zero;

        /// <inheritdoc />
        public override bool Contains(Vector position) => false;

        /// <inheritdoc />
        public override Region Shift(Vector displacement) => this;
    }
}
