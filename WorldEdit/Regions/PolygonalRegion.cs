using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Represents a polygonal region.
    /// </summary>
    public sealed class PolygonalRegion : Region
    {
        private readonly List<Vector> _vertices;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonalRegion" /> class with the specified vertices.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <exception cref="ArgumentException"><paramref name="vertices" /> contains less than three items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vertices" /> is <c>null</c>.</exception>
        public PolygonalRegion(IEnumerable<Vector> vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            _vertices = vertices.ToList();

            if (_vertices.Count < 3)
            {
                throw new ArgumentException("Vertex list must have at least three items.", nameof(vertices));
            }
        }

        /// <inheritdoc />
        public override bool CanContract => false;

        /// <inheritdoc />
        public override bool CanExpand => false;

        /// <inheritdoc />
        public override bool CanShift => true;

        /// <inheritdoc />
        public override Vector LowerBound => new Vector(_vertices.Min(v => v.X), _vertices.Min(v => v.Y));

        /// <inheritdoc />
        public override Vector UpperBound => new Vector(_vertices.Max(v => v.X) + 1, _vertices.Max(v => v.Y) + 1);

        /// <summary>
        /// Gets a read-only view of the vertices.
        /// </summary>
        public IReadOnlyList<Vector> Vertices => _vertices.AsReadOnly();

        /// <inheritdoc />
        public override bool Contains(Vector position)
        {
            // First check the bounding box to avoid the expensive computation below.
            if (position.X < LowerBound.X || position.X >= UpperBound.X ||
                position.Y < LowerBound.Y || position.Y >= UpperBound.Y)
            {
                return false;
            }

            var result = false;
            for (var i = 0; i < _vertices.Count; ++i)
            {
                var vertex = _vertices[i];
                if (position == vertex)
                {
                    return true;
                }

                var nextVertex = i < _vertices.Count - 1 ? _vertices[i + 1] : _vertices[0];
                var flipped = vertex.X > nextVertex.X;
                var x1 = flipped ? nextVertex.X : vertex.X;
                var x2 = flipped ? vertex.X : nextVertex.X;

                // Consider all edges that are above and below the position.
                if (x1 <= position.X && position.X <= x2)
                {
                    var y1 = flipped ? nextVertex.Y : vertex.Y;
                    var y2 = flipped ? vertex.Y : nextVertex.Y;
                    var crossProduct = (position.Y - y1) * (x2 - x1) - (y2 - y1) * (position.X - x1);

                    // If the position lies on the edge and its Y component is between y1 and y2, then the position is
                    // considered inside of the polygon. The second condition is required in the case that x1 = x2, as
                    // otherwise any position with the same X component would be considered inside of the polygon.
                    if (crossProduct == 0 && y1 <= position.Y == position.Y <= y2)
                    {
                        return true;
                    }

                    // If the position is below the edge, then reverse the result. The position must be below an odd
                    // number of edges to be considered inside of the polygon.
                    if (crossProduct < 0 && x1 != position.X)
                    {
                        result = !result;
                    }
                }
            }
            return result;
        }

        /// <inheritdoc />
        public override Region Contract(Vector delta) =>
            throw new InvalidOperationException("Polygonal regions cannot contract.");

        /// <inheritdoc />
        public override Region Expand(Vector delta) =>
            throw new InvalidOperationException("Polygonal regions cannot expand.");

        /// <inheritdoc />
        public override Region Inset(int delta) =>
            throw new InvalidOperationException("Polygonal regions cannot contract.");

        /// <inheritdoc />
        public override Region Outset(int delta) =>
            throw new InvalidOperationException("Polygonal regions cannot expand.");

        /// <inheritdoc />
        public override Region Shift(Vector displacement) =>
            new PolygonalRegion(_vertices.Select(v => v + displacement));
    }
}
