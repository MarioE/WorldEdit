using System;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Represents a composable extent that limits the number of tiles that can be set.
    /// </summary>
    public class LimitedExtent : Extent
    {
        private readonly Extent _extent;
        private readonly int _limit;
        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedExtent" /> class with the specified extent and limit.
        /// </summary>
        /// <param name="extent">The extent to compose with.</param>
        /// <param name="limit">The limit on the number of tiles that can be set. A negative value indicates no limit.</param>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public LimitedExtent(Extent extent, int limit)
        {
            _extent = extent ?? throw new ArgumentNullException(nameof(extent));
            _limit = limit;
        }

        /// <inheritdoc />
        public override Vector LowerBound => _extent.LowerBound;

        /// <inheritdoc />
        public override Vector UpperBound => _extent.UpperBound;

        /// <inheritdoc />
        public override Tile this[int x, int y]
        {
            get => _extent[x, y];
            set
            {
                if (_limit < 0 || _count++ < _limit)
                {
                    _extent[x, y] = value;
                }
            }
        }
    }
}
