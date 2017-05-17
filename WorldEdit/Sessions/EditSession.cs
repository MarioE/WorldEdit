using System;
using System.Linq;
using WorldEdit.Extents;
using WorldEdit.History;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Templates;

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

        /// <summary>
        /// Clears the tiles in the specified region.
        /// </summary>
        /// <param name="region">The region to modify.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="region" /> is <c>null</c>.</exception>
        public int ClearTiles(Region region)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                if (SetTile(position, new Tile()))
                {
                    ++count;
                }
            }
            return count;
        }

        /// <inheritdoc />
        public override Tile GetTile(int x, int y) => _extent.GetTile(x, y);

        /// <summary>
        /// Redoes the edit session.
        /// </summary>
        /// <returns>The number of redone changes.</returns>
        public int Redo() => _changeSet.Redo(_world);

        /// <summary>
        /// Replaces the tiles in the specified region using the specified templates.
        /// </summary>
        /// <param name="region">The region to modify.</param>
        /// <param name="fromTemplate">The template to match with.</param>
        /// <param name="toTemplate">The template to apply.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="region" />, <paramref name="fromTemplate" />, or <paramref name="toTemplate" /> is <c>null</c>.
        /// </exception>
        public int ReplaceTiles(Region region, ITemplate fromTemplate, ITemplate toTemplate)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (fromTemplate == null)
            {
                throw new ArgumentNullException(nameof(fromTemplate));
            }
            if (toTemplate == null)
            {
                throw new ArgumentNullException(nameof(toTemplate));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                var tile = GetTile(position);
                if (fromTemplate.Matches(tile) && SetTile(position, toTemplate.Apply(tile)))
                {
                    ++count;
                }
            }
            return count;
        }

        /// <inheritdoc />
        public override bool SetTile(int x, int y, Tile tile) => _extent.SetTile(x, y, tile);

        /// <summary>
        /// Sets the tiles in the specified region using the specified template.
        /// </summary>
        /// <param name="region">The region to modify.</param>
        /// <param name="template">The template to apply.</param>
        /// <returns>The number of modifications.</returns>
        public int SetTiles(Region region, ITemplate template)
        {
            if (region == null)
            {
                throw new ArgumentNullException(nameof(region));
            }
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var count = 0;
            foreach (var position in region.Where(IsInBounds))
            {
                if (SetTile(position, template.Apply(GetTile(position))))
                {
                    ++count;
                }
            }
            return count;
        }

        /// <summary>
        /// Undoes the edit session.
        /// </summary>
        /// <returns>The number of undone changes.</returns>
        public int Undo() => _changeSet.Undo(_world);
    }
}
