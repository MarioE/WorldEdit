using System;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;

namespace WorldEdit.Tests.Extents
{
    [TestFixture]
    public class LimitedExtentTests
    {
        [TestCase(3, 4)]
        public void SetTile_LimitOkay_Succeeded(int x, int y)
        {
            var position = new Vector(x, y);
            var tile = new Tile();
            var extent = Mock.Of<Extent>(e => e.SetTile(position, tile));
            var limitedExtent = new LimitedExtent(extent, 1);

            Assert.That(limitedExtent.SetTile(position, tile));
            Mock.Get(extent).Verify(e => e.SetTile(position, tile), Times.Once);
        }

        [TestCase(3, 4)]
        public void SetTile_LimitPassed_Failed(int x, int y)
        {
            var position = new Vector(x, y);
            var tile = new Tile();
            var extent = Mock.Of<Extent>(e => e.SetTile(Vector.Zero, new Tile()));
            Mock.Get(extent).Setup(e => e.SetTile(position, tile)).Throws(new InvalidOperationException());
            var limitedExtent = new LimitedExtent(extent, 1);
            limitedExtent.SetTile(Vector.Zero, new Tile());

            Assert.That(!limitedExtent.SetTile(position, tile));
        }
    }
}
