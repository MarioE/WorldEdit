using NUnit.Framework;
using WorldEdit.Regions;

namespace WorldEdit.Tests.Regions
{
    [TestFixture]
    public class EmptyRegionTests
    {
        [TestCase(-1, -1)]
        [TestCase(50, 73)]
        public void Contains(int x, int y)
        {
            var region = new EmptyRegion();

            Assert.That(!region.Contains(new Vector(x, y)));
        }

        [TestCase(1, 1)]
        public void Shift(int deltaX, int deltaY)
        {
            Assert.That(new EmptyRegion().Shift(new Vector(deltaX, deltaY)), Is.InstanceOf<EmptyRegion>());
        }
    }
}
