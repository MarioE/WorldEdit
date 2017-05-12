﻿using System;
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
        public override Tile this[int x, int y]
        {
            get => _extent[x, y];
            set
            {
                var position = new Vector(x, y);
                _changeSet.Add(new TileChange(position, _extent[position], value));
                _extent[position] = value;
            }
        }
    }
}