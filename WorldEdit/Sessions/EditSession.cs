using System;
using JetBrains.Annotations;
using WorldEdit.Extents;
using WorldEdit.History;
using WorldEdit.Masks;

namespace WorldEdit.Sessions
{
    /// <summary>
    /// Represents an edit session whose changes may be considered collectively.
    /// </summary>
    public sealed class EditSession : WrappedExtent
    {
        private readonly ChangeSet _changeSet;
        private readonly World _world;

        private EditSession(Extent extent, World world, ChangeSet changeSet) : base(extent)
        {
            _world = world;
            _changeSet = changeSet;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="EditSession" /> class with the specified world, limit, and mask.
        /// </summary>
        /// <param name="world">The world to modify, which must not be <c>null</c>.</param>
        /// <param name="limit">The limit on the number of tiles that can be set.</param>
        /// <param name="mask">The mask to test, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="world" /> or <paramref name="mask" /> is <c>null</c>.
        /// </exception>
        public static EditSession Create([NotNull] World world, int limit, [NotNull] Mask mask)
        {
            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }
            if (mask == null)
            {
                throw new ArgumentNullException(nameof(mask));
            }

            var changeSet = new ChangeSet();
            Extent extent = new LoggedExtent(world, changeSet);
            extent = new LimitedExtent(extent, limit);
            extent = new MaskedExtent(extent, mask);
            return new EditSession(extent, world, changeSet);
        }

        /// <summary>
        /// Redoes the edit session.
        /// </summary>
        /// <returns>The number of modifications.</returns>
        public int Redo() => _changeSet.Redo(_world);

        /// <summary>
        /// Undoes the edit session.
        /// </summary>
        /// <returns>The number of modifications.</returns>
        public int Undo() => _changeSet.Undo(_world);
    }
}
