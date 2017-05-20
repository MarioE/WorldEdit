using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Masks;

namespace WorldEdit.Tests.Masks
{
    [TestFixture]
    public class NegatedMaskTests
    {
        [Test]
        public void Ctor_NullMask_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new NegatedMask(null));
        }

        [TestCase(4, 5)]
        public void Test_False(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[10, 10]};
            var mask = new NegatedMask(new NullMask());

            Assert.IsFalse(mask.Test(extent, new Vector(x, y)));
        }

        [TestCase(4, 5)]
        public void Test_True(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[10, 10]};
            var mask = new NegatedMask(new NegatedMask(new NullMask()));

            Assert.IsTrue(mask.Test(extent, new Vector(x, y)));
        }
    }
}
