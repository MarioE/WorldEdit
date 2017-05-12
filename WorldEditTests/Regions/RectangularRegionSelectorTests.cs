using NUnit.Framework;
using WorldEdit;
using WorldEdit.Regions;

namespace WorldEditTests.Regions
{
    [TestFixture]
    public class RectangularRegionSelectorTests
    {
        [TestCase(1, 5)]
        public void SelectPrimary_CorrectRegion(int x, int y)
        {
            var selector = new RectangularRegionSelector();
            selector.SelectSecondary(Vector.Zero);

            var region = (RectangularRegion)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), region.Position1);
            Assert.AreEqual(Vector.Zero, region.Position2);
        }

        [TestCase(5, 5)]
        public void SelectPrimary_NoSecondary_NullRegion(int x, int y)
        {
            var selector = new RectangularRegionSelector();

            var region = selector.SelectPrimary(new Vector(x, y));

            Assert.IsInstanceOf<NullRegion>(region);
            Assert.AreEqual(new Vector(x, y), selector.PrimaryPosition);
        }

        [TestCase(6, 20)]
        public void SelectSecondary_CorrectRegion(int x2, int y2)
        {
            var selector = new RectangularRegionSelector();
            selector.SelectPrimary(Vector.Zero);

            var region = (RectangularRegion)selector.SelectSecondary(new Vector(x2, y2));

            Assert.AreEqual(Vector.Zero, region.Position1);
            Assert.AreEqual(new Vector(x2, y2), region.Position2);
        }

        [TestCase(5, 5)]
        public void SelectSecondary_NoPrimary_NullRegion(int x2, int y2)
        {
            var selector = new RectangularRegionSelector();

            var region = selector.SelectSecondary(new Vector(x2, y2));

            Assert.IsInstanceOf<NullRegion>(region);
            Assert.AreEqual(new Vector(x2, y2), selector.SecondaryPosition);
        }
    }
}
