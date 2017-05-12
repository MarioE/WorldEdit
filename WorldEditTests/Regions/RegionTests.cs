using NUnit.Framework;
using WorldEdit;

namespace WorldEditTests.Regions
{
    [TestFixture]
    public class RegionTests
    {
        [Test]
        public void Dimensions()
        {
            var region = new MockRegion();

            Assert.AreEqual(Vector.One, region.Dimensions);
        }
    }
}
