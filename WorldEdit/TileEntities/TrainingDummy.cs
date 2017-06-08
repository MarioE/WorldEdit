namespace WorldEdit.TileEntities
{
    /// <summary>
    ///     Represents a training dummy tile entity.
    /// </summary>
    public sealed class TrainingDummy : ITileEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TrainingDummy" /> class with the specified position and NPC index.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="npcIndex">The NPC index.</param>
        public TrainingDummy(Vector position, int npcIndex)
        {
            Position = position;
            NpcIndex = npcIndex;
        }

        /// <summary>
        ///     Gets the NPC index.
        /// </summary>
        public int NpcIndex { get; }

        /// <inheritdoc />
        public Vector Position { get; }

        /// <inheritdoc />
        public ITileEntity WithPosition(Vector position) => new TrainingDummy(position, NpcIndex);
    }
}
