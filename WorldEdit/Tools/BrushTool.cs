using System;
using JetBrains.Annotations;
using WorldEdit.Extents;
using WorldEdit.Regions;
using WorldEdit.Templates;

namespace WorldEdit.Tools
{
    /// <summary>
    /// Represents a brush tool that creates circles of patterns.
    /// </summary>
    /// <typeparam name="T">The type of pattern.</typeparam>
    public sealed class BrushTool<T> : ITool where T : class, ITemplate
    {
        private readonly Pattern<T> _pattern;
        private readonly int _radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushTool{T}" /> class with the specified pattern and radius.
        /// </summary>
        /// <param name="radius">The radius, which must be nonnegative.</param>
        /// <param name="pattern">The pattern, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pattern" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> is negative.</exception>
        public BrushTool(int radius, [NotNull] Pattern<T> pattern)
        {
            _radius = radius >= 0
                ? radius
                : throw new ArgumentOutOfRangeException(nameof(radius), "Number must be non-negative.");
            _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        }

        /// <inheritdoc />
        public int Apply(Extent extent, Vector position)
        {
            var region = new EllipticRegion(position, _radius * Vector.One);
            return extent.ModifyTiles(region, _pattern);
        }
    }
}
