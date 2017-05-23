using NUnit.Framework;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;

namespace WorldEdit.Tests.Regions.Selectors
{
    [TestFixture]
    public class PolygonalRegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            RegionSelector selector = new PolygonalRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);
            selector = selector.SelectSecondary(Vector.One);
            selector = selector.SelectSecondary(Vector.One);

            var selector2 = (PolygonalRegionSelector)selector.Clear();

            Assert.That(selector2.Positions, Is.Empty);
        }

        [Test]
        public void Ctor_NullPositions_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new PolygonalRegionSelector(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetRegion()
        {
            RegionSelector selector = new PolygonalRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);
            selector = selector.SelectSecondary(Vector.One);
            selector = selector.SelectSecondary(Vector.One);

            var region = (PolygonalRegion)selector.GetRegion();

            Assert.That(region.Vertices, Has.Count.EqualTo(3));
            Assert.That(region.Vertices[0], Is.EqualTo(Vector.Zero));
            Assert.That(region.Vertices[1], Is.EqualTo(Vector.One));
            Assert.That(region.Vertices[2], Is.EqualTo(Vector.One));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetRegion_LessThanThreePositions_NullRegion(int count)
        {
            RegionSelector selector = new PolygonalRegionSelector();
            if (count-- > 0)
            {
                selector = selector.SelectPrimary(Vector.Zero);
            }
            while (count-- > 0)
            {
                selector = selector.SelectSecondary(Vector.Zero);
            }

            Assert.That(selector.GetRegion(), Is.InstanceOf<EmptyRegion>());
        }

        [Test]
        public void Positions()
        {
            var selector = new PolygonalRegionSelector(new[] {Vector.Zero, Vector.One, Vector.One});

            Assert.That(selector.Positions, Has.Count.EqualTo(3));
            Assert.That(selector.Positions[0], Is.EqualTo(Vector.Zero));
            Assert.That(selector.Positions[1], Is.EqualTo(Vector.One));
            Assert.That(selector.Positions[2], Is.EqualTo(Vector.One));
        }

        [TestCase(1, 5)]
        public void PrimaryPosition(int x, int y)
        {
            var selector = new PolygonalRegionSelector();

            var selector2 = (PolygonalRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.That(selector2.PrimaryPosition, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 5)]
        public void SelectPrimary(int x, int y)
        {
            var selector = new PolygonalRegionSelector();

            var selector2 = (PolygonalRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.That(selector2.PrimaryPosition, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 5)]
        public void SelectPrimary_PreviousSelected(int x, int y)
        {
            RegionSelector selector = new PolygonalRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);
            selector = selector.SelectSecondary(Vector.One);

            var selector2 = (PolygonalRegionSelector)selector.SelectPrimary(new Vector(x, y));

            Assert.That(selector2.PrimaryPosition, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 5)]
        public void SelectSecondary(int x, int y)
        {
            RegionSelector selector = new PolygonalRegionSelector();
            selector = selector.SelectPrimary(Vector.Zero);

            var selector2 = (PolygonalRegionSelector)selector.SelectSecondary(new Vector(x, y));

            Assert.That(selector2.Positions, Has.Count.EqualTo(2));
            Assert.That(selector2.Positions[1], Is.EqualTo(new Vector(x, y)));
        }
    }
}
