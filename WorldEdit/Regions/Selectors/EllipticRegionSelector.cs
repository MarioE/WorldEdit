using JetBrains.Annotations;

namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    /// Represents an elliptic region selector.
    /// </summary>
    public sealed class EllipticRegionSelector : RegionSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EllipticRegionSelector" /> class with the specified positions.
        /// </summary>
        /// <param name="position1">The first position, or <c>null</c> if it is not selected.</param>
        /// <param name="position2">The second position, or <c>null</c> if it is not selected.</param>
        public EllipticRegionSelector([CanBeNull] Vector? position1 = null, [CanBeNull] Vector? position2 = null)
        {
            Position1 = position1;
            Position2 = position2;
        }

        /// <summary>
        /// Gets the first position of this <see cref="EllipticRegionSelector" /> instance, or <c>null</c> if it is not selected.
        /// </summary>
        [CanBeNull]
        public Vector? Position1 { get; }

        /// <summary>
        /// Gets the second position of this <see cref="EllipticRegionSelector" /> instance, or <c>null</c> if it is not selected.
        /// </summary>
        [CanBeNull]
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
                return new EmptyRegion();
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
