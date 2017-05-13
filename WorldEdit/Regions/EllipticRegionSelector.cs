namespace WorldEdit.Regions
{
    /// <summary>
    /// Represents an elliptic region selector.
    /// </summary>
    public class EllipticRegionSelector : RegionSelector
    {
        /// <inheritdoc />
        protected override Region GetRegion()
        {
            if (PrimaryPosition == null || SecondaryPosition == null)
            {
                return new NullRegion();
            }

            return new EllipticRegion(PrimaryPosition.Value, (SecondaryPosition - PrimaryPosition).Value);
        }
    }
}
