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
        public Result<Clipboard> Read(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream must support reading.", nameof(stream));
            }

            return ReadImpl(stream);
        }

        /// <summary>
        /// Writes the specified clipboard to the stream.
        /// </summary>
        /// <param name="clipboard">The clipboard to write.</param>
        /// <param name="stream">The stream to write to.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="clipboard" /> or <paramref name="stream" /> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">The stream does not support writing.</exception>
        public void Write(Clipboard clipboard, Stream stream)
        {
            if (clipboard == null)
            {
                throw new ArgumentNullException(nameof(clipboard));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Stream must support writing.", nameof(stream));
            }

            WriteImpl(clipboard, stream);
        }

        /// <summary>
        /// Reads a clipboard from the specified stream. This method has -no- validation!
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The result.</returns>
        protected abstract Result<Clipboard> ReadImpl(Stream stream);

        /// <summary>
        /// Writes the specified clipboard to the stream. This method has -no- validation!
        /// </summary>
        /// <param name="clipboard">The clipboard to write.</param>
        /// <param name="stream">The stream to write to.</param>
        protected abstract void WriteImpl(Clipboard clipboard, Stream stream);
    }
}
