using JetBrains.Annotations;

namespace WorldEdit.TileEntities
{
    /// <summary>
    /// Specifies a tile entity.
    /// </summary>
    public interface ITileEntity
    {
        /// <summary>
        /// Gets the position of this <see cref="ITileEntity" /> instance.
        /// </summary>
        Vector Position { get; }

        /// <summary>
        /// Creates a new <see cref="ITileEntity" /> instance based on this <see cref="ITileEntity" /> instance but with the
        /// specified position instead.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting tile entity.</returns>
        [NotNull]
        [Pure]
        ITileEntity WithPosition(Vector position);
    }
}
