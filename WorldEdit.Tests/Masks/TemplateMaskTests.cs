using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Masks;
using WorldEdit.Templates;

namespace WorldEdit.Tests.Masks
{
    [TestFixture]
    public class TemplateMaskTests
    {
        [Test]
        public void Ctor_NullTemplate_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new TemplateMask(null), Throws.ArgumentNullException);
        }

        [TestCase(4, 5, true)]
        [TestCase(4, 5, false)]
        public void Test(int x, int y, bool expected)
        {
            var tile = new Tile();
            var template = Mock.Of<ITemplate>(t => t.Matches(tile) == expected);
            var mask = new TemplateMask(template);
            var position = new Vector(x, y);
            var extent = Mock.Of<Extent>(m => m.GetTile(position) == tile);

            Assert.That(mask.Test(extent, new Vector(x, y)), Is.EqualTo(expected));
        }
    }
}
