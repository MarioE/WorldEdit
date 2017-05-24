using JetBrains.Annotations;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Represents a wrapped extent that limits the number of tiles that can be set.
    /// </summary>
    public sealed class LimitedExtent : WrappedExtent
    {
        private readonly int _limit;
        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedExtent" /> class with the specified extent and limit.
        /// </summary>
        /// <param name="extent">The extent to wrap, which must not be <c>null</c>.</param>
        /// <param name="limit">The limit on the number of tiles that can be set. A negative value indicates no limit.</param>
        public LimitedExtent([NotNull] Extent extent, int limit) : base(extent)
        {
            _limit = limit;
        }

        /// <inheritdoc />
        public override bool SetTile(Vector position, Tile tile)
        {
            if ((_limit < 0 || _count < _limit) && Extent.SetTile(position, tile))
            {
                ++_count;
                return true;
            }
            return false;
        }
    }
}
