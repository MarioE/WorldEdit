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
        /// <remarks>
        /// This method will always return <c>false</c>.
        /// </remarks>
        public override bool Contains(Vector position) => false;

        /// <inheritdoc />
        /// <remarks>
        /// This method will always return the same <see cref="EmptyRegion" /> instance.
        /// </remarks>
        public override Region Shift(Vector displacement) => this;
    }
}
