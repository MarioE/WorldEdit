﻿using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Masks;
using WorldEdit.Regions;

namespace WorldEditTests.Masks
{
    [TestFixture]
    public class RegionMaskTests
    {
        [Test]
        public void Ctor_NullRegion_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RegionMask(null));
        }

        [TestCase(10, 10, 4, 5, 4, 4, 7, 7, true)]
        [TestCase(10, 10, 4, 5, 5, 5, 7, 7, false)]
        public void Test(int extentWidth, int extentHeight, int x, int y, int regionX, int regionY, int regionX2,
            int regionY2, bool expected)
        {
            var extent = new MockExtent {Tiles = new ITile[extentWidth, extentHeight]};
            var region = new RectangularRegion(new Vector(regionX, regionY), new Vector(regionX2, regionY2));
            var mask = new RegionMask(region);

            Assert.AreEqual(expected, mask.Test(extent, new Vector(x, y)));
        }
    }
}
