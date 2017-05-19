using System.Collections.Generic;

namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    /// Represents a polygonal region selector.
    /// </summary>
    public sealed class PolygonalRegionSelector : RegionSelector
    {
        private readonly List<Vector> _positions = new List<Vector>();

        /// <inheritdoc />
        public override Vector? PrimaryPosition => _positions.Count > 0 ? (Vector?)_positions[0] : null;

        /// <inheritdoc />
        public override void Clear()
        {
            _positions.Clear();
        }

        /// <inheritdoc />
        public override Region SelectPrimary(Vector position)
        {
            _positions.Clear();
            _positions.Add(position);
            return new NullRegion();
        }

        /// <inheritdoc />
        public override Region SelectSecondary(Vector position)
        {
            if (_positions.Count > 0)
            {
                _positions.Add(position);
            }
            return _positions.Count >= 3 ? (Region)new PolygonalRegion(_positions) : new NullRegion();
        }
    }
}
