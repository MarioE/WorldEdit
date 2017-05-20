using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Tools;

namespace WorldEdit.Tests.Tools
{
    [TestFixture]
    public class NullToolTests
    {
        [Test]
        public void Apply()
        {
            using (var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]}))
            {
                var tool = new NullTool();

                Assert.AreEqual(0, tool.Apply(world, Vector.Zero));
            }
        }
    }
}
