namespace WorldEdit.Regions
{
    /// <summary>
    /// Represents a rectangular region selector.
    /// </summary>
    public class RectangularRegionSelector : RegionSelector
    {
        /// <inheritdoc />
        protected override Region GetRegion()
        {
            if (PrimaryPosition == null || SecondaryPosition == null)
            {
                return new NullRegion();
            }
            return new RectangularRegion(PrimaryPosition.Value, SecondaryPosition.Value);
        }
    }
}
