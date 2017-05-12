using System;

namespace WorldEdit
{
    /// <summary>
    /// Represents the result of parsing a string.
    /// </summary>
    /// <typeparam name="T">The type of the resulting value.</typeparam>
    public class ParsingResult<T>
    {
        private ParsingResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            WasSuccessful = false;
        }

        private ParsingResult(T value)
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
        public T Value { get; }

        /// <summary>
        /// Gets a value indicating whether the parsing was successful.
        /// </summary>
        public bool WasSuccessful { get; }

        /// <summary>
        /// Creates a parsing result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The parsing result.</returns>
        public static ParsingResult<T> Error(string errorMessage)
        {
            return new ParsingResult<T>(errorMessage ?? throw new ArgumentNullException(nameof(errorMessage)));
        }

        public static implicit operator ParsingResult<T>(T value)
        {
            return new ParsingResult<T>(value);
        }
    }
}
