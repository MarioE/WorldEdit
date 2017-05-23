using System;

namespace WorldEdit
{
    /// <summary>
    /// Represents a configuration.
    /// </summary>
    public sealed class Config
    {
        /// <summary>
        /// Gets the session grace period for this <see cref="Config" /> instance.
        /// </summary>
        public TimeSpan SessionGracePeriod { get; } = TimeSpan.FromSeconds(600);

        /// <summary>
        /// Gets the session history limit for this <see cref="Config" /> instance.
        /// </summary>
        public int SessionHistoryLimit { get; } = 10;
    }
}
