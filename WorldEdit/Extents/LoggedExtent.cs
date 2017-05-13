using System;
using WorldEdit.History;

namespace WorldEdit.Extents
{
    /// <summary>
    /// Represents a composable extent that logs changes to a change set.
    /// </summary>
    public class LoggedExtent : Extent
    {
        private readonly ChangeSet _changeSet;
        private readonly Extent _extent;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggedExtent" /> class with the specified extent and change set.
        /// </summary>
        /// <param name="extent">The extent to compose with.</param>
        /// <param name="changeSet">The change set to log to.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="extent" /> or <paramref name="changeSet" /> is <c>null</c>.
        /// </exception>
        public LoggedExtent(Extent extent, ChangeSet changeSet)
        {
            _extent = extent ?? throw new ArgumentNullException(nameof(extent));
            _changeSet = changeSet ?? throw new ArgumentNullException(nameof(changeSet));
        }

        /// <inheritdoc />
        public override Vector LowerBound => _extent.LowerBound;

        /// <inheritdoc />
        public override Vector UpperBound => _extent.UpperBound;

        /// <inheritdoc />
        public override Tile GetTile(int x, int y) => _extent.GetTile(x, y);

        /// <inheritdoc />
        public override bool SetTile(int x, int y, Tile tile)
        {
            var oldTile = _extent.GetTile(x, y);
            if (_extent.SetTile(x, y, tile))
            {
                _changeSet.Add(new TileChange(new Vector(x, y), oldTile, tile));
                return true;
            }
            return false;
        }
    }
}
