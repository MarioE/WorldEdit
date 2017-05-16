using System;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Regions;

namespace WorldEditTests.Regions
{
    [TestFixture]
    public class RegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            var selector = new RegionSelector();
            selector.SelectPrimary(Vector.Zero);
            selector.SelectSecondary(Vector.One);

            selector.Clear();

            Assert.AreEqual(null, selector.PrimaryPosition);
            Assert.AreEqual(null, selector.SecondaryPosition);
        }

        [Test]
        public void GetSetRegionType()
        {
            var selector = new RegionSelector();
            var regionType = typeof(RectangularRegion);

            selector.RegionType = regionType;

            Assert.AreEqual(regionType, selector.RegionType);
        }

        [TestCase(1, 5)]
        public void SelectPrimary(int x, int y)
        {
            var selector = new RegionSelector();
            selector.SelectSecondary(Vector.Zero);

            var region = (RectangularRegion)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), region.Position1);
        }

        [TestCase(1, 5)]
        public void SelectPrimary_NoSecondary(int x, int y)
        {
            var selector = new RegionSelector();

            var region = selector.SelectPrimary(new Vector(x, y));

            Assert.IsInstanceOf<NullRegion>(region);
            Assert.AreEqual(new Vector(x, y), selector.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectSecondary(int x, int y)
        {
            var selector = new RegionSelector();
            selector.SelectPrimary(Vector.Zero);

            var region = (RectangularRegion)selector.SelectSecondary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), region.Position2);
        }

        [TestCase(1, 5)]
        public void SelectSecondary_NoPrimary(int x, int y)
        {
            var selector = new RegionSelector();

            var region = selector.SelectSecondary(new Vector(x, y));

            Assert.IsInstanceOf<NullRegion>(region);
            Assert.AreEqual(new Vector(x, y), selector.SecondaryPosition);
        }

        [Test]
        public void SetRegionType_NullValue_ThrowsArgumentNullException()
        {
            var selector = new RegionSelector();

            Assert.Throws<ArgumentNullException>(() => selector.RegionType = null);
        }

        [Test]
        public void SetRegionType_ValueDoesNotInheritFromRegion_ThrowsArgumentOutOfRangeException()
        {
            var selector = new RegionSelector();

            Assert.Throws<ArgumentOutOfRangeException>(() => selector.RegionType = typeof(int));
        }
    }
}
