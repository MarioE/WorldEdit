using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Masks;

namespace WorldEdit.Tests.Masks
{
    [TestFixture]
    public class EmptyMaskTests
    {
        [TestCase(4, 5)]
        public void Test(int x, int y)
        {
            var mask = new EmptyMask();
            var extent = Mock.Of<Extent>();

            Assert.That(mask.Test(extent, new Vector(x, y)));
        }
    }
}
