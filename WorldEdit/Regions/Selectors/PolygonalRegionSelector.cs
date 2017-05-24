using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    /// Represents a polygonal region selector.
    /// </summary>
    public sealed class PolygonalRegionSelector : RegionSelector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonalRegionSelector" /> class with no selected positions.
        /// </summary>
        public PolygonalRegionSelector() : this(Enumerable.Empty<Vector>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonalRegionSelector" /> class with the specified positions.
        /// </summary>
        /// <param name="positions">The positions, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="positions" /> is <c>null</c>.</exception>
        public PolygonalRegionSelector([NotNull] IEnumerable<Vector> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException(nameof(positions));
            }

            Positions = positions.ToList().AsReadOnly();
        }

        /// <summary>
        /// Gets a read-only view of the positions.
        /// </summary>
        [NotNull]
        public IReadOnlyList<Vector> Positions { get; }

        /// <inheritdoc />
        public override Vector? PrimaryPosition => Positions.Count > 0 ? (Vector?)Positions[0] : null;

        /// <inheritdoc />
        public override RegionSelector Clear() => new PolygonalRegionSelector();

        /// <inheritdoc />
        public override Region GetRegion() =>
            Positions.Count >= 3 ? (Region)new PolygonalRegion(Positions) : new EmptyRegion();

        /// <inheritdoc />
        public override RegionSelector SelectPrimary(Vector position) => new PolygonalRegionSelector(new[] {position});

        /// <inheritdoc />
        public override RegionSelector SelectSecondary(Vector position)
        {
            var newPositions = new List<Vector>(Positions);
            if (newPositions.Count > 0)
            {
                newPositions.Add(position);
            }
            return new PolygonalRegionSelector(newPositions);
        }
    }
}
