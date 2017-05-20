namespace WorldEdit.Core
{
    /// <summary>
    /// Represents the result of some operation.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result" /> class with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public Result(string errorMessage) : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result" /> class with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public Result(T value) : base(value)
        {
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public new T Value => (T)base.Value;
    }
}
