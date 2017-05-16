using System;

namespace WorldEdit
{
    /// <summary>
    /// Represents a two-dimensional integer vector.
    /// </summary>
    public struct Vector : IEquatable<Vector>
    {
        /// <summary>
        /// Gets the vector with X and Y components equal to one.
        /// </summary>
        public static readonly Vector One = new Vector(1, 1);

        /// <summary>
        /// Gets the zero vector.
        /// </summary>
        public static readonly Vector Zero = new Vector(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector" /> structure with the specified components.
        /// </summary>
        /// <param name="x">The X component.</param>
        /// <param name="y">The Y component.</param>
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Gets the X component.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets the Y component.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Adds the two specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The sum of the two vectors.</returns>
        public static Vector operator +(Vector vector1, Vector vector2) =>
            new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);

        /// <summary>
        /// Tests the two specified vectors for equality.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns><c>true</c> if the two vectors are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector vector1, Vector vector2) => vector1.Equals(vector2);

        /// <summary>
        /// Tests the two specified vectors for inequality.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns><c>true</c> if the two vectors are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector vector1, Vector vector2) => !vector1.Equals(vector2);

        /// <summary>
        /// Multiplies the specified vector by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        /// <param name="vector">The vector.</param>
        /// <returns>The multiplication of the scalar and vector.</returns>
        public static Vector operator *(int scalar, Vector vector) => new Vector(scalar * vector.X, scalar * vector.Y);

        /// <summary>
        /// Subtracts the two specified vectors.
        /// </summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The difference of the two vectors.</returns>
        public static Vector operator -(Vector vector1, Vector vector2) =>
            new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);

        /// <summary>
        /// Negates the specified vector.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The negated vector.</returns>
        public static Vector operator -(Vector vector) => -1 * vector;
        
        /// <summary>
        /// Determines whether the vector equals the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><c>true</c> if the vector and the object are equal; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) => obj is Vector vector && Equals(vector);

        /// <summary>
        /// Determines whether the vector equals the other specified vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns><c>true</c> if the two vectors are equal; otherwise <c>false</c>.</returns>
        public bool Equals(Vector other) => other.X == X && other.Y == Y;

        /// <summary>
        /// Returns the hash code of the vector.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => X ^ Y;

        /// <summary>
        /// Returns the string representation of the vector.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString() => $"{X}, {Y}";
    }
}
