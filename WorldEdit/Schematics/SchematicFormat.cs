using System;
using System.IO;
using JetBrains.Annotations;

namespace WorldEdit.Schematics
{
    /// <summary>
    /// Specifies a schematic format.
    /// </summary>
    public abstract class SchematicFormat
    {
        /// <summary>
        /// Reads a clipboard from the specified stream using this <see cref="SchematicFormat" /> instance. A return value of
        /// <c>null</c> indicates failure.
        /// </summary>
        /// <param name="stream">The stream to read from, which must not be <c>null</c> and support reading.</param>
        /// <returns>The clipboard, or <c>null</c> if reading failed.</returns>
        /// <exception cref="ArgumentException"><paramref name="stream" /> does not support reading.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <c>null</c>.</exception>
        [CanBeNull]
        public abstract Clipboard Read([NotNull] Stream stream);

        /// <summary>
        /// Writes the specified clipboard to the stream using this <see cref="SchematicFormat" /> instance.
        /// </summary>
        /// <param name="clipboard">The clipboard to write, which must not be <c>null</c> and support writing.</param>
        /// <param name="stream">The stream to write to, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentException"><paramref name="stream" /> does not support writing.</exception>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="clipboard" /> or <paramref name="stream" /> is <c>null</c>.
        /// </exception>
        public abstract void Write([NotNull] Clipboard clipboard, [NotNull] Stream stream);
    }
}
