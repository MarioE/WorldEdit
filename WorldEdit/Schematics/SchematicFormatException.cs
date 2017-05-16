using System;
using System.Runtime.Serialization;

namespace WorldEdit.Schematics
{
    /// <summary>
    /// The exception that is thrown when a schematic is malformed.
    /// </summary>
    [Serializable]
    public class SchematicFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicFormatException" /> class.
        /// </summary>
        public SchematicFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicFormatException" /> class with the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public SchematicFormatException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicFormatException" /> class with the specified message and inner
        /// exception.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SchematicFormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicFormatException" /> class from the serialization information.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        protected SchematicFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
