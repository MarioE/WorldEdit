using System;

namespace WorldEdit
{
    /// <summary>
    /// Represents the result of some operation.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result" /> class with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <exception cref="ArgumentNullException"><paramref name="errorMessage" /> is <c>null</c>.</exception>
        public Result(string errorMessage)
        {
            ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result" /> class with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public Result(object value)
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
        /// Gets a value indicating whether the operation was successful.
        /// </summary>
        public bool WasSuccessful { get; }

        /// <summary>
        /// Creates a result from the specified value.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>The result.</returns>
        public static Result<T> From<T>(T value) => new Result<T>(value);

        /// <summary>
        /// Creates a result from the specified error message.
        /// </summary>
        /// <typeparam name="T">The expected type of value.</typeparam>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>The result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="errorMessage" /> is <c>null</c>.</exception>
        public static Result<T> FromError<T>(string errorMessage)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            return new Result<T>(errorMessage);
        }
    }
}
