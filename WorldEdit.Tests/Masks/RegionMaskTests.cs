using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Masks;
using WorldEdit.Core.Regions;

namespace WorldEdit.Tests.Masks
{
    [TestFixture]
    public class RegionMaskTests
    {
        [Test]
        public void Ctor_NullRegion_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RegionMask(null));
        }

        [TestCase(4, 5, 4, 4, 7, 7, true)]
        [TestCase(4, 5, 5, 5, 7, 7, false)]
        public void Test(int x, int y, int regionX, int regionY, int regionX2,
            int regionY2, bool expected)
        {
            var extent = new MockExtent {Tiles = new ITile[10, 10]};
            var region = new RectangularRegion(new Vector(regionX, regionY), new Vector(regionX2, regionY2));
            var mask = new RegionMask(region);

            Assert.AreEqual(expected, mask.Test(extent, new Vector(x, y)));
        }
    }
}
