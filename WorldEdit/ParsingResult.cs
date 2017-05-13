using System;

namespace WorldEdit
{
    /// <summary>
    /// Represents the result of parsing a string.
    /// </summary>
    public class ParsingResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingResult" /> class with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <exception cref="ArgumentNullException"><paramref name="errorMessage" /> is <c>null</c>.</exception>
        public ParsingResult(string errorMessage)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingResult" /> class with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public ParsingResult(object value)
        {
            Value = value;
            WasSuccessful = true;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Gets a value indicating whether the parsing was successful.
        /// </summary>
        public bool WasSuccessful { get; }

        /// <summary>
        /// Creates a parsing result from the specified value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The parsing result.</returns>
        public static ParsingResult<T> From<T>(T value)
        {
            return new ParsingResult<T>(value);
        }

        /// <summary>
        /// Creates a parsing result from the specified error message.
        /// </summary>
        /// <typeparam name="T">The expected type of value.</typeparam>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The parsing result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="errorMessage" /> is <c>null</c>.</exception>
        public static ParsingResult<T> FromError<T>(string errorMessage)
        {
            return new ParsingResult<T>(errorMessage ?? throw new ArgumentNullException(nameof(errorMessage)));
        }
    }
}
