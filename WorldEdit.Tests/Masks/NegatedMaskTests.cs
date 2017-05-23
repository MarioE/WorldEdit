using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Masks;

namespace WorldEdit.Tests.Masks
{
    [TestFixture]
    public class NegatedMaskTests
    {
        [Test]
        public void Ctor_NullMask_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new NegatedMask(null), Throws.ArgumentNullException);
        }

        [TestCase(3, 4)]
        public void Test(int x, int y)
        {
            var extent = Mock.Of<Extent>();
            var mask = Mock.Of<Mask>(m => m.Test(extent, It.IsAny<Vector>()));
            var negatedMask = new NegatedMask(mask);

            Assert.That(!negatedMask.Test(extent, new Vector(x, y)));
            Mock.Get(mask).Verify(m => m.Test(extent, new Vector(x, y)), Times.Once);
        }
    }
}
