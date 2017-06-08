using System;

namespace WorldEdit
{
    /// <summary>
    ///     Represents a configuration.
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        ///     Gets the session grace period.
        /// </summary>
        public TimeSpan SessionGracePeriod { get; } = TimeSpan.FromSeconds(600);

        /// <summary>
        ///     Gets the session history limit.
        /// </summary>
        public int SessionHistoryLimit { get; } = 10;
    }
}
