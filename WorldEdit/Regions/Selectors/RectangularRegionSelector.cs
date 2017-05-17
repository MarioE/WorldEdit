namespace WorldEdit.Regions.Selectors
{
    /// <summary>
    /// Represents a rectangular region selector.
    /// </summary>
    public class RectangularRegionSelector : RegionSelector
    {
        private Vector? _position1;
        private Vector? _position2;

        /// <inheritdoc />
        public override Vector? PrimaryPosition => _position1;

        /// <inheritdoc />
        public override void Clear()
        {
            _position1 = _position2 = null;
        }

        /// <inheritdoc />
        public override Region SelectPrimary(Vector position)
        {
            _position1 = position;
            return _position2 != null
                ? (Region)new RectangularRegion(_position1.Value, _position2.Value)
                : new NullRegion();
        }

        /// <inheritdoc />
        public override Region SelectSecondary(Vector position)
        {
            _position2 = position;
            return _position1 != null
                ? (Region)new RectangularRegion(_position1.Value, _position2.Value)
                : new NullRegion();
        }
    }
}
