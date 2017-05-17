using System;
using System.IO;

namespace WorldEdit.Schematics
{
    /// <summary>
    /// Specifies a schematic format.
    /// </summary>
    public abstract class SchematicFormat
    {
        /// <summary>
        /// Reads a clipboard from the specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The stream does not support reading.</exception>
        public abstract Result<Clipboard> Read(Stream stream);

        /// <summary>
        /// Writes the specified clipboard to the stream.
        /// </summary>
        /// <param name="clipboard">The clipboard to write.</param>
        /// <param name="stream">The stream to write to.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="clipboard" /> or <paramref name="stream" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">The stream does not support writing.</exception>
        public abstract void Write(Clipboard clipboard, Stream stream);
    }
}
