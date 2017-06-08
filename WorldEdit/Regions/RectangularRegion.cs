using System;

namespace WorldEdit.Regions
{
    /// <summary>
    ///     Represents a rectangular region.
    /// </summary>
    public sealed class RectangularRegion : ResizableRegion
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RectangularRegion" /> class with the specified positions.
        /// </summary>
        /// <param name="position1">The first position.</param>
        /// <param name="position2">The second position.</param>
        public RectangularRegion(Vector position1, Vector position2)
        {
            Position1 = position1;
            Position2 = position2;
            LowerBound = new Vector(Math.Min(position1.X, position2.X), Math.Min(position1.Y, position2.Y));
            UpperBound = new Vector(Math.Max(position1.X, position2.X) + 1, Math.Max(position1.Y, position2.Y) + 1);
        }

        /// <inheritdoc />
        public override Vector LowerBound { get; }

        /// <summary>
        ///     Gets the first position.
        /// </summary>
        public Vector Position1 { get; }

        /// <summary>
        ///     Gets the second position.
        /// </summary>
        public Vector Position2 { get; }

        /// <inheritdoc />
        public override Vector UpperBound { get; }

        /// <inheritdoc />
        public override bool Contains(Vector position) =>
            LowerBound.X <= position.X && position.X < UpperBound.X &&
            LowerBound.Y <= position.Y && position.Y < UpperBound.Y;

        /// <inheritdoc />
        public override ResizableRegion Contract(Vector delta) => Change(delta, false);

        /// <inheritdoc />
        public override ResizableRegion Expand(Vector delta) => Change(delta, true);

        /// <inheritdoc />
        public override ResizableRegion Inset(int delta) => Contract(delta * Vector.One).Contract(-delta * Vector.One);

        /// <inheritdoc />
        public override ResizableRegion Outset(int delta) => Expand(delta * Vector.One).Expand(-delta * Vector.One);

        /// <inheritdoc />
        public override Region Shift(Vector displacement) =>
            new RectangularRegion(Position1 + displacement, Position2 + displacement);

        private RectangularRegion Change(Vector delta, bool isExpansion)
        {
            var position1 = Position1;
            var position2 = Position2;
            var multiplier = isExpansion ? 1 : -1;

            // If we're expanding or contracting the same side that position1 is on relative to position2, then modify
            // position1. Otherwise, modify position2.
            if ((position1.X < position2.X) ^ (delta.X >= 0))
            {
                position1 += multiplier * new Vector(delta.X, 0);
            }
            else
            {
                position2 += multiplier * new Vector(delta.X, 0);
            }
            if ((position1.Y < position2.Y) ^ (delta.Y >= 0))
            {
                position1 += multiplier * new Vector(0, delta.Y);
            }
            else
            {
                position2 += multiplier * new Vector(0, delta.Y);
            }
            return new RectangularRegion(position1, position2);
        }
    }
}
