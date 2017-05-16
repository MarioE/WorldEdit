using System;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Holds selected positions, creating regions from them.
    /// </summary>
    public class RegionSelector
    {
        private Func<Vector, Vector, Region> _selectorFunction = (v1, v2) => new RectangularRegion(v1, v2);

        /// <summary>
        /// Gets the primary position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? PrimaryPosition { get; private set; }

        /// <summary>
        /// Gets the secondary position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? SecondaryPosition { get; private set; }

        /// <summary>
        /// Gets or sets the selector function for creating regions.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public Func<Vector, Vector, Region> SelectorFunction
        {
            get => _selectorFunction;
            set => _selectorFunction = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Clears the selected positions.
        /// </summary>
        public void Clear()
        {
            PrimaryPosition = SecondaryPosition = null;
        }

        /// <summary>
        /// Selects the primary position and returns the resulting region.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region.</returns>
        public Region SelectPrimary(Vector position)
        {
            PrimaryPosition = position;
            return GetRegion();
        }

        /// <summary>
        /// Selects the secondary position and returns the resulting region.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>The resulting region.</returns>
        public Region SelectSecondary(Vector position)
        {
            SecondaryPosition = position;
            return GetRegion();
        }

        private Region GetRegion()
        {
            if (PrimaryPosition == null || SecondaryPosition == null)
            {
                return new NullRegion();
            }

            return SelectorFunction(PrimaryPosition.Value, SecondaryPosition.Value);
        }
    }
}
