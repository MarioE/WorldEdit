using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core.Masks;

namespace WorldEdit.Core.Tests.Masks
{
    [TestFixture]
    public class NullMaskTests
    {
        [TestCase(4, 5)]
        public void Test(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[10, 10]};
            var mask = new NullMask();

            Assert.IsTrue(mask.Test(extent, new Vector(x, y)));
        }
    }
}
