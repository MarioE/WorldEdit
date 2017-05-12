using NUnit.Framework;
using WorldEdit;

namespace WorldEditTests.Regions
{
    [TestFixture]
    public class RegionSelectorTests
    {
        [Test]
        public void Clear()
        {
            var selector = new MockRegionSelector();
            selector.SelectPrimary(Vector.Zero);
            selector.SelectSecondary(Vector.One);

            selector.Clear();

            Assert.AreEqual(null, selector.PrimaryPosition);
            Assert.AreEqual(null, selector.SecondaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectPrimary(int x, int y)
        {
            var selector = new MockRegionSelector();

            selector.SelectPrimary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector.PrimaryPosition);
        }

        [TestCase(1, 5)]
        public void SelectSecondary(int x, int y)
        {
            var selector = new MockRegionSelector();

            selector.SelectSecondary(new Vector(x, y));

            Assert.AreEqual(new Vector(x, y), selector.SecondaryPosition);
        }
    }
}
