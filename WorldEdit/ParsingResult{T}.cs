namespace WorldEdit
{
    /// <summary>
    /// Represents the result of parsing a string.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    public class ParsingResult<T> : ParsingResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingResult" /> class with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public ParsingResult(string errorMessage) : base(errorMessage)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParsingResult" /> class with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public ParsingResult(T value) : base(value)
        {
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public new T Value => (T)base.Value;
    }
}
