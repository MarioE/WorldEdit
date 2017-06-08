using System;
using JetBrains.Annotations;

namespace WorldEdit.TileEntities
{
    /// <summary>
    ///     Represents a sign tile entity.
    /// </summary>
    public sealed class Sign : ITileEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Sign" /> class with the specified position and text.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="text">The text, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="text" /> is <c>null</c>.</exception>
        public Sign(Vector position, [NotNull] string text)
        {
            Position = position;
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        [NotNull]
        public string Text { get; }

        /// <inheritdoc />
        public Vector Position { get; }

        /// <inheritdoc />
        public ITileEntity WithPosition(Vector position) => new Sign(position, Text);
    }
}
