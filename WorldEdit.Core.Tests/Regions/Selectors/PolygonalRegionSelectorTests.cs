using System.Linq;
using NUnit.Framework;
using WorldEdit.Core.Regions;
using WorldEdit.Core.Regions.Selectors;

namespace WorldEdit.Core.Tests.Regions.Selectors
{
    [TestFixture]
    public class PolygonalRegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            RegionSelector selector = new PolygonalRegionSelector(Enumerable.Empty<Vector>());
            selector = selector.SelectPrimary(Vector.Zero);
            selector = selector.SelectSecondary(Vector.One);
            selector = selector.SelectSecondary(Vector.One);

            var selector2 = (PolygonalRegionSelector)selector.Clear();

            Assert.AreEqual(0, selector2.Positions.Count);
        }

        [Test]
        public void GetRegion()
        {
            RegionSelector selector = new PolygonalRegionSelector(Enumerable.Empty<Vector>());
            selector = selector.SelectPrimary(Vector.Zero);
            selector = selector.SelectSecondary(Vector.One);
            selector = selector.SelectSecondary(Vector.One);

            var region = (PolygonalRegion)selector.GetRegion();

            Assert.AreEqual(3, region.Vertices.Count);
            Assert.AreEqual(Vector.Zero, region.Vertices[0]);
            Assert.AreEqual(Vector.One, region.Vertices[1]);
            Assert.AreEqual(Vector.One, region.Vertices[2]);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetRegion_LessThanThreePositions_NullRegion(int count)
        {
            RegionSelector selector = new PolygonalRegionSelector(Enumerable.Empty<Vector>());
            if (count-- > 0)
            {
                selector = selector.SelectPrimary(Vector.Zero);
            }
            while (count-- > 0)
            {
                selector = selector.SelectSecondary(Vector.Zero);
            }

            Assert.IsInstanceOf<NullRegion>(selector.GetRegion());
        }

        [TestCase(1, 5)]
        public void PrimaryPosition(int x, int y)
        {
            var selector = new PolygonalRegionSelector();

            var selector2 = (PolygonalRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector2.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectPrimary(int x, int y)
        {
            var selector = new PolygonalRegionSelector();

            var selector2 = (PolygonalRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector2.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectPrimary_PreviousSelected(int x, int y)
        {
            RegionSelector selector = new PolygonalRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);
            selector = selector.SelectSecondary(Vector.One);

            var selector2 = (PolygonalRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector2.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectSecondary(int x, int y)
        {
            RegionSelector selector = new PolygonalRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);

            var selector2 = (PolygonalRegionSelector)selector.SelectSecondary(new Vector(x, y));

            Assert.AreEqual(2, selector2.Positions.Count);
            Assert.AreEqual(new Vector(x, y), selector2.Positions[1]);
        }
    }
}
