namespace WorldEdit.Core.Templates
{
    /// <summary>
    /// Specifies a template that can be applied to and matched with tiles.
    /// </summary>
    public interface ITemplate
    {
        /// <summary>
        /// Applies the template to the specified tile.
        /// </summary>
        /// <param name="tile">The tile to modify.</param>
        /// <returns>The resulting tile.</returns>
        Tile Apply(Tile tile);

        /// <summary>
        /// Determines whether the template matches the specified tile.
        /// </summary>
        /// <param name="tile">The tile to check.</param>
        /// <returns><c>true</c> if the template matches; otherwise, <c>false</c>.</returns>
        bool Matches(Tile tile);
    }
}
