using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Represents an elliptic region.
    /// </summary>
    public class EllipticRegion : Region
    {
        private readonly List<Vector> _boundaryPositions;

        /// <summary>
        /// Initializes a new instance of the <see cref="EllipticRegion" /> class with the specified center position and radius.
        /// </summary>
        /// <param name="center">The center position.</param>
        /// <param name="radius">The radius. The components will be normalized to positive values.</param>
        public EllipticRegion(Vector center, Vector radius)
        {
            Center = center;
            Radius = new Vector(Math.Abs(radius.X), Math.Abs(radius.Y));

            // TODO: consider more efficient hashtable implementation
            _boundaryPositions = GenerateBoundaryPositions(center, Radius);
        }

        /// <summary>
        /// Gets the center position.
        /// </summary>
        public Vector Center { get; }

        /// <inheritdoc />
        public override Vector LowerBound => Center - Radius;

        /// <summary>
        /// Gets the radius.
        /// </summary>
        public Vector Radius { get; }

        /// <inheritdoc />
        public override Vector UpperBound => Center + Radius + Vector.One;

        /// <inheritdoc />
        public override bool Contains(Vector position)
        {
            if (position.X < LowerBound.X || position.X >= UpperBound.X)
            {
                return false;
            }
            if (position.Y < LowerBound.Y || position.Y >= UpperBound.Y)
            {
                return false;
            }

            var minimumY = _boundaryPositions.Where(v => v.X == position.X).Min(v => v.Y);
            var maximumY = _boundaryPositions.Where(v => v.X == position.X).Max(v => v.Y);
            return minimumY <= position.Y && position.Y <= maximumY;
        }

        /// <inheritdoc />
        public override Region Contract(Vector delta) => Change(delta, false);

        /// <inheritdoc />
        public override Region Expand(Vector delta) => Change(delta, true);

        /// <inheritdoc />
        public override IEnumerator<Vector> GetEnumerator()
        {
            for (var x = LowerBound.X; x < UpperBound.X; ++x)
            {
                var minimumY = _boundaryPositions.Where(v => v.X == x).Min(v => v.Y);
                var maximumY = _boundaryPositions.Where(v => v.X == x).Max(v => v.Y);
                for (var y = minimumY; y <= maximumY; ++y)
                {
                    yield return new Vector(x, y);
                }
            }
        }

        /// <inheritdoc />
        public override Region Inset(int delta) => Change(delta * Vector.One, false);

        /// <inheritdoc />
        public override Region Outset(int delta) => Change(delta * Vector.One, true);

        /// <inheritdoc />
        public override Region Shift(Vector displacement) => new EllipticRegion(Center + displacement, Radius);

        private static List<Vector> GenerateBoundaryPositions(Vector center, Vector radius)
        {
            // See http://stackoverflow.com/questions/15474122/is-there-a-midpoint-ellipse-algorithm
            // This algorithm works better when x < y. Thus, we exchange them if necessary.
            var boundaryPositions = new List<Vector>();
            var isFlipped = radius.X > radius.Y;
            var xSquared = isFlipped ? radius.Y * radius.Y : radius.X * radius.X;
            var ySquared = isFlipped ? radius.X * radius.X : radius.Y * radius.Y;
            var x = 0;
            var y = isFlipped ? radius.X : radius.Y;

            void AddBoundaryPosition(int dx, int dy)
            {
                if (isFlipped)
                {
                    boundaryPositions.Add(center + new Vector(dy, dx));
                    boundaryPositions.Add(center + new Vector(-dy, dx));
                    boundaryPositions.Add(center + new Vector(dy, -dx));
                    boundaryPositions.Add(center + new Vector(-dy, -dx));
                }
                else
                {
                    boundaryPositions.Add(center + new Vector(dx, dy));
                    boundaryPositions.Add(center + new Vector(-dx, dy));
                    boundaryPositions.Add(center + new Vector(dx, -dy));
                    boundaryPositions.Add(center + new Vector(-dx, -dy));
                }
            }

            // Add the top and bottom points.
            AddBoundaryPosition(0, y);

            // Add the first half of all the quadrants.
            var p = Math.Round(ySquared - xSquared * y + xSquared / 4.0);
            var px = 0;
            var py = 2 * xSquared * y;
            while (px < py)
            {
                if (p >= 0)
                {
                    --y;
                    py -= 2 * xSquared;
                    p -= py;
                }
                px += 2 * ySquared;
                p += ySquared + px;
                AddBoundaryPosition(++x, y);
            }

            // Add the second half of all the quadrants.
            p = Math.Round(ySquared * (x + 0.5) * (x + 0.5) + xSquared * (y - 1) * (y - 1) - xSquared * ySquared);
            while (y > 0)
            {
                if (p <= 0)
                {
                    ++x;
                    px += 2 * ySquared;
                    p += px;
                }
                py -= 2 * xSquared;
                p += xSquared - py;
                AddBoundaryPosition(x, --y);
            }

            return boundaryPositions;
        }

        private Region Change(Vector delta, bool isExpansion)
        {
            delta = new Vector(Math.Abs(delta.X), Math.Abs(delta.Y));
            delta = isExpansion ? delta : -delta;
            return new EllipticRegion(Center, Radius + delta);
        }
    }
}
