using System;
using WorldEdit.Extents;
using WorldEdit.Regions;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Represents a mask that tests for region containment.
    /// </summary>
    public class RegionMask : Mask
    {
        private readonly Region _region;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegionMask" /> class with the specified region.
        /// </summary>
        /// <param name="region">The region to test with.</param>
        /// <exception cref="ArgumentNullException"><paramref name="region" /> is <c>null</c>.</exception>
        public RegionMask(Region region)
        {
            _region = region ?? throw new ArgumentNullException(nameof(region));
        }

        /// <inheritdoc />
        protected override bool TestImpl(Extent extent, Vector position) => _region.Contains(position);
    }
}
