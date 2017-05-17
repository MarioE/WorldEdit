using System;
using System.Linq;
using WorldEdit.Extents;
using WorldEdit.Regions;
using WorldEdit.Templates;

namespace WorldEdit.Tools
{
    /// <summary>
    /// Represents a brush tool that creates circles of patterns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BrushTool<T> : ITool where T : class, ITemplate
    {
        private readonly Pattern<T> _pattern;
        private readonly int _radius;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrushTool{T}" /> class with the specified pattern and radius.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <param name="pattern">The pattern.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pattern" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="radius" /> is negative.</exception>
        public BrushTool(int radius, Pattern<T> pattern)
        {
            _radius = radius >= 0
                ? radius
                : throw new ArgumentOutOfRangeException(nameof(radius), "Number must be non-negative.");
            _pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        }

        /// <inheritdoc />
        public int Apply(Extent extent, Vector position)
        {
            var count = 0;
            var region = new EllipticRegion(position, _radius * Vector.One);
            foreach (var position2 in region.Where(extent.IsInBounds))
            {
                if (extent.SetTile(position2, _pattern.Apply(extent.GetTile(position2))))
                {
                    ++count;
                }
            }
            return count;
        }
    }
}
