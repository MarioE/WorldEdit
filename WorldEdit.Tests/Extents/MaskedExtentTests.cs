using System;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Masks;

namespace WorldEdit.Tests.Extents
{
    [TestFixture]
    public class MaskedExtentTests
    {
        [Test]
        public void Ctor_NullChangeSet_ThrowsArgumentNullException()
        {
            var extent = Mock.Of<Extent>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new MaskedExtent(extent, null), Throws.ArgumentNullException);
        }
        
        [TestCase(3, 4)]
        public void SetTile_MaskFailed(int x, int y)
        {
            var position = new Vector(x, y);
            var tile = new Tile {Wall = 1};
            var extent = Mock.Of<Extent>();
            Mock.Get(extent).Setup(e => e.SetTile(position, tile)).Throws(new InvalidOperationException());
            var mask = Mock.Of<Mask>(m => !m.Test(extent, It.IsAny<Vector>()));
            var maskedExtent = new MaskedExtent(extent, mask);

            Assert.That(!maskedExtent.SetTile(position, tile));
        }

        [TestCase(3, 4)]
        public void SetTile_MaskPassed(int x, int y)
        {
            var position = new Vector(x, y);
            var tile = new Tile {Wall = 1};
            var extent = Mock.Of<Extent>(e => e.SetTile(position, tile));
            var mask = Mock.Of<Mask>(m => m.Test(extent, It.IsAny<Vector>()));
            var maskedExtent = new MaskedExtent(extent, mask);

            Assert.That(maskedExtent.SetTile(position, tile));
            Mock.Get(extent).Verify(e => e.SetTile(position, tile));
        }
    }
}
