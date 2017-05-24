using JetBrains.Annotations;

namespace WorldEdit.TileEntities
{
    /// <summary>
    /// Specifies a tile entity.
    /// </summary>
    public interface ITileEntity
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        Vector Position { get; }

        /// <summary>
        /// Creates a new <see cref="ITileEntity" /> instance with the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting tile entity.</returns>
        [NotNull]
        [Pure]
        ITileEntity WithPosition(Vector position);
    }
}
