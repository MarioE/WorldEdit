using NUnit.Framework;
using WorldEdit;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;

namespace WorldEditTests.Regions.Selectors
{
    [TestFixture]
    public class PolygonalRegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            var selector = new PolygonalRegionSelector();
            selector.SelectPrimary(Vector.Zero);
            selector.SelectSecondary(Vector.One);
            selector.SelectSecondary(Vector.One);

            selector.Clear();

            Assert.AreEqual(null, selector.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectPrimary_NullRegion(int x, int y)
        {
            var selector = new PolygonalRegionSelector();

            var region = selector.SelectPrimary(new Vector(x, y));

            Assert.IsInstanceOf<NullRegion>(region);
            Assert.AreEqual(new Vector(x, y), selector.PrimaryPosition);
        }

        [TestCase(6, 20)]
        public void SelectSecondary_CorrectRegion(int x2, int y2)
        {
            var selector = new PolygonalRegionSelector();
            selector.SelectPrimary(Vector.Zero);
            selector.SelectSecondary(Vector.One);

            var region = (PolygonalRegion)selector.SelectSecondary(new Vector(x2, y2));

            Assert.AreEqual(3, region.Vertices.Count);
            Assert.AreEqual(new Vector(x2, y2), region.Vertices[2]);
        }

        [TestCase(5, 5)]
        public void SelectSecondary_OnlyTwo_NullRegion(int x2, int y2)
        {
            var selector = new PolygonalRegionSelector();
            selector.SelectPrimary(Vector.Zero);
            var region = selector.SelectPrimary(Vector.Zero);

            Assert.IsInstanceOf<NullRegion>(region);
        }
    }
}
