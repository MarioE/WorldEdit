using System;
using JetBrains.Annotations;
using WorldEdit.Extents;
using WorldEdit.Templates;

namespace WorldEdit.Masks
{
    /// <summary>
    /// Represents a mask that tests for a template.
    /// </summary>
    public sealed class TemplateMask : Mask
    {
        private readonly ITemplate _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateMask" /> class with the specified template.
        /// </summary>
        /// <param name="template">The template to test, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="template" /> is <c>null</c>.</exception>
        public TemplateMask([NotNull] ITemplate template)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
        }

        /// <inheritdoc />
        public override bool Test(Extent extent, Vector position) => _template.Matches(extent.GetTile(position));
    }
}
