using System;
using JetBrains.Annotations;
using WorldEdit.Extents;
using WorldEdit.Regions;

namespace WorldEdit.Masks
{
    /// <summary>
    ///     Represents a mask that tests for region membership.
    /// </summary>
    public sealed class RegionMask : Mask
    {
        private readonly Region _region;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionMask" /> class with the specified region.
        /// </summary>
        /// <param name="region">The region to check, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="region" /> is <c>null</c>.</exception>
        public RegionMask([NotNull] Region region)
        {
            _region = region ?? throw new ArgumentNullException(nameof(region));
        }

        /// <inheritdoc />
        public override bool Test(Extent extent, Vector position) => _region.Contains(position);
    }
}
