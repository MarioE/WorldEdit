using NUnit.Framework;
using WorldEdit.Core.Regions;
using WorldEdit.Core.Regions.Selectors;

namespace WorldEdit.Core.Tests.Regions.Selectors
{
    [TestFixture]
    public class RectangularRegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            RegionSelector selector = new RectangularRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);
            selector = selector.SelectSecondary(Vector.One);

            var selector2 = (RectangularRegionSelector)selector.Clear();

            Assert.AreEqual(null, selector2.Position1);
            Assert.AreEqual(null, selector2.Position2);
        }

        [TestCase(0, 0, 4, 4)]
        public void GetRegion(int x, int y, int x2, int y2)
        {
            RegionSelector selector = new RectangularRegionSelector();
            selector = selector.SelectPrimary(new Vector(x, y));
            selector = selector.SelectSecondary(new Vector(x2, y2));

            var region = (RectangularRegion)selector.GetRegion();

            Assert.AreEqual(new Vector(x, y), region.Position1);
            Assert.AreEqual(new Vector(x2, y2), region.Position2);
        }

        [Test]
        public void GetRegion_NoPrimary_NullRegion()
        {
            RegionSelector selector = new RectangularRegionSelector();
            selector = selector.SelectSecondary(Vector.Zero);

            Assert.IsInstanceOf<NullRegion>(selector.GetRegion());
        }

        [Test]
        public void GetRegion_NoSecondary_NullRegion()
        {
            RegionSelector selector = new RectangularRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);

            Assert.IsInstanceOf<NullRegion>(selector.GetRegion());
        }

        [TestCase(1, 5)]
        public void PrimaryPosition(int x, int y)
        {
            var selector = new RectangularRegionSelector();

            var selector2 = (RectangularRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector2.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectPrimary(int x, int y)
        {
            var selector = new RectangularRegionSelector();

            var selector2 = (RectangularRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector2.Position1);
        }

        [TestCase(1, 5)]
        public void SelectSecondary(int x, int y)
        {
            var selector = new RectangularRegionSelector();

            var selector2 = (RectangularRegionSelector)selector.SelectSecondary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector2.Position2);
        }
    }
}
