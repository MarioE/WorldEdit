namespace WorldEdit.Core.Regions.Selectors
{
    /// <summary>
    /// Represents a rectangular region selector.
    /// </summary>
    public sealed class EllipticRegionSelector : RegionSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EllipticRegionSelector" /> class with no selected positions.
        /// </summary>
        public EllipticRegionSelector() : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipticRegionSelector" /> class with the specified positions.
        /// </summary>
        /// <param name="position1">The first position or <c>null</c> if it is not selected.</param>
        /// <param name="position2">The second position or <c>null</c> if it is not selected.</param>
        public EllipticRegionSelector(Vector? position1, Vector? position2)
        {
            Position1 = position1;
            Position2 = position2;
        }

        /// <summary>
        /// Gets the first position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? Position1 { get; }

        /// <summary>
        /// Gets the second position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? Position2 { get; }

        /// <inheritdoc />
        public override Vector? PrimaryPosition => Position1;

        /// <inheritdoc />
        public override RegionSelector Clear() => new EllipticRegionSelector();

        /// <inheritdoc />
        public override Region GetRegion()
        {
            if (Position1 == null || Position2 == null)
            {
                return new NullRegion();
            }
            return new EllipticRegion(Position1.Value, (Position2 - Position1).Value);
        }

        /// <inheritdoc />
        public override RegionSelector SelectPrimary(Vector position)
            => new EllipticRegionSelector(position, Position2);

        /// <inheritdoc />
        public override RegionSelector SelectSecondary(Vector position)
            => new EllipticRegionSelector(Position1, position);
    }
}
