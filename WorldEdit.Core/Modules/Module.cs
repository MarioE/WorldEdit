using System;

namespace WorldEdit.Core.Modules
{
    /// <summary>
    /// Defines a WorldEdit module that encapsulates a specific area of functionality.
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Module" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        /// <exception cref="ArgumentNullException"><paramref name="plugin" /> is <c>null</c>.</exception>
        protected Module(WorldEditPlugin plugin)
        {
            Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
        }

        /// <summary>
        /// Gets the WorldEdit plugin.
        /// </summary>
        protected WorldEditPlugin Plugin { get; }

        /// <summary>
        /// Deregisters the module.
        /// </summary>
        public abstract void Deregister();

        /// <summary>
        /// Registers the module.
        /// </summary>
        public abstract void Register();
    }
}
