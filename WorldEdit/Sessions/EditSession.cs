using System;
using WorldEdit.Extents;
using WorldEdit.History;
using WorldEdit.Masks;

namespace WorldEdit.Sessions
{
    /// <summary>
    /// Represents an edit session whose changes can be considered collectively.
    /// </summary>
    public class EditSession : Extent
    {
        private readonly ChangeSet _changeSet = new ChangeSet();
        private readonly Extent _extent;
        private readonly World _world;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditSession" /> class with the specified world, limit, and mask.
        /// </summary>
        /// <param name="world">The world to modify.</param>
        /// <param name="limit">The limit on the number of tiles that can be set.</param>
        /// <param name="mask">The mask to test.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="world" /> or <paramref name="mask" /> is <c>null</c>.
        /// </exception>
        public EditSession(World world, int limit, Mask mask)
        {
            if (mask == null)
            {
                throw new ArgumentNullException(nameof(mask));
            }

            _world = world ?? throw new ArgumentNullException(nameof(world));
            _extent = new LoggedExtent(_world, _changeSet);
            _extent = new LimitedExtent(_extent, limit);
            _extent = new MaskedExtent(_extent, mask);
        }

        /// <inheritdoc />
        public override Vector LowerBound => _extent.LowerBound;

        /// <inheritdoc />
        public override Vector UpperBound => _extent.UpperBound;

        /// <inheritdoc />
        public override Tile GetTile(int x, int y) => _extent.GetTile(x, y);

        /// <summary>
        /// Redoes the edit session.
        /// </summary>
        /// <returns>The number of redone changes.</returns>
        public int Redo() => _changeSet.Redo(_world);

        /// <inheritdoc />
        public override bool SetTile(int x, int y, Tile tile) => _extent.SetTile(x, y, tile);

        /// <summary>
        /// Undoes the edit session.
        /// </summary>
        /// <returns>The number of undone changes.</returns>
        public int Undo() => _changeSet.Undo(_world);
    }
}
