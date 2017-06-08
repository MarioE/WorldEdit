namespace WorldEdit.Regions
{
    /// <summary>
    ///     Represents an elliptic region.
    /// </summary>
    public sealed class EllipticRegion : ResizableRegion
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EllipticRegion" /> class with the specified center position and
        ///     radius.
        /// </summary>
        /// <param name="center">The center position.</param>
        /// <param name="radius">The radius. The components will be normalized to non-negative values.</param>
        public EllipticRegion(Vector center, Vector radius)
        {
            Center = center;
            Radius = Vector.Abs(radius);
        }

        /// <summary>
        ///     Gets the center position.
        /// </summary>
        public Vector Center { get; }

        /// <inheritdoc />
        public override Vector LowerBound => Center - Radius;

        /// <summary>
        ///     Gets the radius.
        /// </summary>
        public Vector Radius { get; }

        /// <inheritdoc />
        public override Vector UpperBound => Center + Radius + Vector.One;

        /// <inheritdoc />
        public override bool Contains(Vector position)
        {
            // Add 0.5 to major and minor axes for a smoother ellipse.
            var aSquared = (Radius.X + 0.5) * (Radius.X + 0.5);
            var bSquared = (Radius.Y + 0.5) * (Radius.Y + 0.5);
            var offset = position - Center;
            return bSquared * offset.X * offset.X + aSquared * offset.Y * offset.Y <= aSquared * bSquared;
        }

        /// <inheritdoc />
        public override ResizableRegion Contract(Vector delta) => Change(delta, false);

        /// <inheritdoc />
        public override ResizableRegion Expand(Vector delta) => Change(delta, true);

        /// <inheritdoc />
        public override ResizableRegion Inset(int delta) => Change(delta * Vector.One, false);

        /// <inheritdoc />
        public override ResizableRegion Outset(int delta) => Change(delta * Vector.One, true);

        /// <inheritdoc />
        public override Region Shift(Vector displacement) => new EllipticRegion(Center + displacement, Radius);

        private EllipticRegion Change(Vector delta, bool isExpansion) =>
            new EllipticRegion(Center, Radius + (isExpansion ? 1 : -1) * Vector.Abs(delta));
    }
}
