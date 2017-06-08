using System;
using JetBrains.Annotations;

namespace WorldEdit.Modules
{
    /// <summary>
    ///     Defines a WorldEdit module that encapsulates a specific area of functionality.
    /// </summary>
    public abstract class Module
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Module" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="plugin" /> is <c>null</c>.</exception>
        protected Module([NotNull] WorldEditPlugin plugin)
        {
            Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
        }

        /// <summary>
        ///     Gets the WorldEdit plugin.
        /// </summary>
        [NotNull]
        protected WorldEditPlugin Plugin { get; }

        /// <summary>
        ///     Deregisters the module.
        /// </summary>
        public abstract void Deregister();

        /// <summary>
        ///     Registers the module.
        /// </summary>
        public abstract void Register();
    }
}
