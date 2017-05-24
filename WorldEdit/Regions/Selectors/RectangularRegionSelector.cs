using JetBrains.Annotations;

namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    /// Represents a rectangular region selector.
    /// </summary>
    public sealed class RectangularRegionSelector : RegionSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RectangularRegionSelector" /> class with the specified positions.
        /// </summary>
        /// <param name="position1">The first position, or <c>null</c> if it is not selected.</param>
        /// <param name="position2">The second position, or <c>null</c> if it is not selected.</param>
        public RectangularRegionSelector([CanBeNull] Vector? position1 = null, [CanBeNull] Vector? position2 = null)
        {
            Position1 = position1;
            Position2 = position2;
        }

        /// <summary>
        /// Gets the first position, or <c>null</c> if it is not selected.
        /// </summary>
        [CanBeNull]
        public Vector? Position1 { get; }

        /// <summary>
        /// Gets the second position, or <c>null</c> if it is not selected.
        /// </summary>
        [CanBeNull]
        public Vector? Position2 { get; }

        /// <inheritdoc />
        public override Vector? PrimaryPosition => Position1;

        /// <inheritdoc />
        public override RegionSelector Clear() => new RectangularRegionSelector();

        /// <inheritdoc />
        public override Region GetRegion()
        {
            if (Position1 == null || Position2 == null)
            {
                return new EmptyRegion();
            }
            return new RectangularRegion(Position1.Value, Position2.Value);
        }

        /// <inheritdoc />
        public override RegionSelector SelectPrimary(Vector position)
            => new RectangularRegionSelector(position, Position2);

        /// <inheritdoc />
        public override RegionSelector SelectSecondary(Vector position)
            => new RectangularRegionSelector(Position1, position);
    }
}
