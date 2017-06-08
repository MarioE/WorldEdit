using System;
using JetBrains.Annotations;
using WorldEdit.Extents;
using WorldEdit.Regions;
using WorldEdit.Templates;

namespace WorldEdit.Tools
{
    /// <summary>
    ///     Represents a brush tool that creates circles of patterns.
    /// </summary>
    public sealed class BrushTool : ITool
    {
        private readonly ITemplate _pattern;
        private readonly int _radius;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BrushTool" /> class with the specified template and radius.
        /// </summary>
        /// <param name="radius">The radius, which must be nonnegative.</param>
        /// <param name="template">The template, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="template" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> is negative.</exception>
        public BrushTool(int radius, [NotNull] ITemplate template)
        {
            _radius = radius >= 0
                ? radius
                : throw new ArgumentOutOfRangeException(nameof(radius), "Number must be non-negative.");
            _pattern = template ?? throw new ArgumentNullException(nameof(template));
        }

        /// <inheritdoc />
        public int Apply(Extent extent, Vector position)
        {
            var region = new EllipticRegion(position, _radius * Vector.One);
            return extent.ModifyTiles(region, _pattern);
        }
    }
}
