﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace WorldEdit.TileEntities
{
    /// <summary>
    /// Represents a chest tile entity.
    /// </summary>
    public sealed class Chest : ITileEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Chest" /> class with the specified position, name, items, and dresser
        /// status.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="name">The name, which must not be <c>null</c>.</param>
        /// <param name="items">The items, which must not be <c>null</c>.</param>
        /// <param name="isDresser"><c>true</c> if the chest is a dresser; otherwise, <c>false</c>.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="name" /> or <paramref name="items" /> is <c>null</c>.
        /// </exception>
        public Chest(Vector position, [NotNull] string name, [NotNull] IEnumerable<Item> items, bool isDresser = false)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Position = position;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Items = items.ToList().AsReadOnly();
            IsDresser = isDresser;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Chest" /> instance is a dresser.
        /// </summary>
        public bool IsDresser { get; }

        /// <summary>
        /// Gets a read-only view of the items of this <see cref="Chest" /> instance.
        /// </summary>
        [NotNull]
        public IReadOnlyList<Item> Items { get; }

        /// <summary>
        /// Gets the name of this <see cref="Chest" /> instance.
        /// </summary>
        [NotNull]
        public string Name { get; }

        /// <inheritdoc />
        public Vector Position { get; }

        /// <inheritdoc />
        public ITileEntity WithPosition(Vector position) => new Chest(position, Name, Items);
    }
}