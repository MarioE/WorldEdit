using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldEdit.Core.Regions.Selectors
{
    /// <summary>
    /// Represents a polygonal region selector.
    /// </summary>
    public sealed class PolygonalRegionSelector : RegionSelector
    {
        private readonly List<Vector> _positions;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonalRegionSelector" /> class with no selected positions.
        /// </summary>
        public PolygonalRegionSelector() : this(Enumerable.Empty<Vector>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonalRegionSelector" /> class with the specified positions.
        /// </summary>
        /// <param name="positions">The positions.</param>
        /// <exception cref="ArgumentNullException"><paramref name="positions" /> is <c>null</c>.</exception>
        public PolygonalRegionSelector(IEnumerable<Vector> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException(nameof(positions));
            }

            _positions = positions.ToList();
        }

        /// <summary>
        /// Gets a read-only view of the positions.
        /// </summary>
        public IReadOnlyList<Vector> Positions => _positions.AsReadOnly();

        /// <inheritdoc />
        public override Vector? PrimaryPosition => _positions.Count > 0 ? (Vector?)_positions[0] : null;

        /// <inheritdoc />
        public override RegionSelector Clear() => new PolygonalRegionSelector();

        /// <inheritdoc />
        public override Region GetRegion() =>
            _positions.Count >= 3 ? (Region)new PolygonalRegion(_positions) : new NullRegion();

        /// <inheritdoc />
        public override RegionSelector SelectPrimary(Vector position) => new PolygonalRegionSelector(new[] {position});

        /// <inheritdoc />
        public override RegionSelector SelectSecondary(Vector position)
        {
            var newPositions = new List<Vector>(_positions);
            if (newPositions.Count > 0)
            {
                newPositions.Add(position);
            }
            return new PolygonalRegionSelector(newPositions);
        }
    }
}
