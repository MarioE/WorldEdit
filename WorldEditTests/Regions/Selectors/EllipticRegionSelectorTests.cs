using NUnit.Framework;
using WorldEdit;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;

namespace WorldEditTests.Regions.Selectors
{
    [TestFixture]
    public class EllipticRegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            var selector = new EllipticRegionSelector();
            selector.SelectPrimary(Vector.Zero);
            selector.SelectSecondary(Vector.One);

            selector.Clear();

            Assert.AreEqual(null, selector.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectPrimary_CorrectRegion(int x, int y)
        {
            var selector = new EllipticRegionSelector();
            selector.SelectSecondary(Vector.Zero);

            var region = (EllipticRegion)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), region.Center);
            Assert.AreEqual(new Vector(x, y), region.Radius);
        }

        [TestCase(5, 5)]
        public void SelectPrimary_NoSecondary_NullRegion(int x, int y)
        {
            var selector = new EllipticRegionSelector();

            var region = selector.SelectPrimary(new Vector(x, y));

            Assert.IsInstanceOf<NullRegion>(region);
            Assert.AreEqual(new Vector(x, y), selector.PrimaryPosition);
        }

        [TestCase(6, 20)]
        public void SelectSecondary_CorrectRegion(int x2, int y2)
        {
            var selector = new EllipticRegionSelector();
            selector.SelectPrimary(Vector.Zero);

            var region = (EllipticRegion)selector.SelectSecondary(new Vector(x2, y2));

            Assert.AreEqual(Vector.Zero, region.Center);
            Assert.AreEqual(new Vector(x2, y2), region.Radius);
        }

        [TestCase(5, 5)]
        public void SelectSecondary_NoPrimary_NullRegion(int x2, int y2)
        {
            var selector = new EllipticRegionSelector();

            var region = selector.SelectSecondary(new Vector(x2, y2));

            Assert.IsInstanceOf<NullRegion>(region);
        }
    }
}
