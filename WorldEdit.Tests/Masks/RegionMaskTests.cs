using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Masks;
using WorldEdit.Regions;

namespace WorldEdit.Tests.Masks
{
    [TestFixture]
    public class RegionMaskTests
    {
        [Test]
        public void Ctor_NullRegion_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new RegionMask(null), Throws.ArgumentNullException);
        }

        [TestCase(3, 3, true)]
        [TestCase(4, 4, false)]
        public void Test(int x, int y, bool expected)
        {
            var position = new Vector(x, y);
            var region = Mock.Of<Region>(r => r.Contains(position) == expected);
            var mask = new RegionMask(region);
            var extent = Mock.Of<Extent>();

            Assert.That(mask.Test(extent, new Vector(x, y)), Is.EqualTo(expected));
        }
    }
}
