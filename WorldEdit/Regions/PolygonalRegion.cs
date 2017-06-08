using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace WorldEdit.Regions
{
    /// <summary>
    ///     Represents a polygonal region.
    /// </summary>
    public sealed class PolygonalRegion : Region
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PolygonalRegion" /> class with the specified vertices.
        /// </summary>
        /// <param name="vertices">The vertices, which must not be <c>null</c> and contain at least three items.</param>
        /// <exception cref="ArgumentException"><paramref name="vertices" /> contains less than three items.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="vertices" /> is <c>null</c>.</exception>
        public PolygonalRegion([NotNull] IEnumerable<Vector> vertices)
        {
            if (vertices == null)
            {
                throw new ArgumentNullException(nameof(vertices));
            }

            Vertices = vertices.ToList().AsReadOnly();
            if (Vertices.Count < 3)
            {
                throw new ArgumentException("Vertex list must have at least three items.", nameof(vertices));
            }
        }

        /// <inheritdoc />
        public override Vector LowerBound => new Vector(Vertices.Min(v => v.X), Vertices.Min(v => v.Y));

        /// <inheritdoc />
        public override Vector UpperBound => new Vector(Vertices.Max(v => v.X) + 1, Vertices.Max(v => v.Y) + 1);

        /// <summary>
        ///     Gets a read-only view of the vertices.
        /// </summary>
        [NotNull]
        public IReadOnlyList<Vector> Vertices { get; }

        /// <inheritdoc />
        /// <remarks>
        ///     This method uses the ray casting algorithm. A vertical ray is "drawn" from the position, and if the number of edges
        ///     above the position is odd, then the position is considered inside the polygon.
        /// </remarks>
        public override bool Contains(Vector position)
        {
            var result = false;
            for (var i = 0; i < Vertices.Count; ++i)
            {
                var vertex = Vertices[i];
                if (position == vertex)
                {
                    return true;
                }

                var nextVertex = i < Vertices.Count - 1 ? Vertices[i + 1] : Vertices[0];
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
        public override Region Shift(Vector displacement) =>
            new PolygonalRegion(Vertices.Select(v => v + displacement));
    }
}
