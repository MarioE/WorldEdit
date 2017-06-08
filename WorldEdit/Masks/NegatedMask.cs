using System;
using JetBrains.Annotations;
using WorldEdit.Extents;

namespace WorldEdit.Masks
{
    /// <summary>
    ///     Represents a negated mask.
    /// </summary>
    public sealed class NegatedMask : Mask
    {
        private readonly Mask _mask;

        /// <summary>
        ///     Initializes a new instance of the <see cref="NegatedMask" /> class negating the specified mask.
        /// </summary>
        /// <param name="mask">The mask to negate, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mask" /> is <c>null</c>.</exception>
        public NegatedMask([NotNull] Mask mask)
        {
            _mask = mask ?? throw new ArgumentNullException(nameof(mask));
        }

        /// <inheritdoc />
        public override bool Test(Extent extent, Vector position) => !_mask.Test(extent, position);
    }
}
