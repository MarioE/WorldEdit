namespace WorldEdit.TileEntities
{
    /// <summary>
    ///     Represents a logic sensor tile entity.
    /// </summary>
    public sealed class LogicSensor : ITileEntity
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LogicSensor" /> class with the specified position, type, and status.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="type">The type.</param>
        /// <param name="isEnabled"><c>true</c> if the sensor is enabled; otherwise, <c>false</c>.</param>
        /// <param name="data">The data.</param>
        public LogicSensor(Vector position, LogicSensorType type, bool isEnabled, int data)
        {
            Position = position;
            Type = type;
            IsEnabled = isEnabled;
            Data = data;
        }

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public int Data { get; }

        /// <summary>
        ///     Gets a value indicating whether the sensor is enabled.
        /// </summary>
        public bool IsEnabled { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public LogicSensorType Type { get; }

        /// <inheritdoc />
        public Vector Position { get; }

        /// <inheritdoc />
        public ITileEntity WithPosition(Vector position) => new LogicSensor(position, Type, IsEnabled, Data);
    }
}
