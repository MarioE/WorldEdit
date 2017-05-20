using NUnit.Framework;

namespace WorldEdit.Core.Tests.Regions
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
