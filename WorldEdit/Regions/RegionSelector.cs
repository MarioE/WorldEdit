using System;

namespace WorldEdit.Regions
{
    /// <summary>
    /// Holds selected positions, creating regions from them.
    /// </summary>
    public class RegionSelector
    {
        private Type _regionType = typeof(RectangularRegion);

        /// <summary>
        /// Gets the primary position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? PrimaryPosition { get; private set; }

        /// <summary>
        /// Gets or sets the region type.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The value does not inherit from <see cref="Region"/>.</exception>
        public Type RegionType
        {
            get => _regionType;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                if (!value.IsSubclassOf(typeof(Region)))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Type must inherit from region.");
                }

                _regionType = value;
            }
        }

        /// <summary>
        /// Gets the secondary position or <c>null</c> if it is not selected.
        /// </summary>
        public Vector? SecondaryPosition { get; private set; }
        
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

            return (Region)Activator.CreateInstance(_regionType, PrimaryPosition.Value, SecondaryPosition.Value);
        }
    }
}
