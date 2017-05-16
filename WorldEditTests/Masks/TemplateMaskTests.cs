using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Masks;
using WorldEdit.Templates;

namespace WorldEditTests.Masks
{
    [TestFixture]
    public class TemplateMaskTests
    {
        [Test]
        public void Ctor_NullTemplate_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TemplateMask(null));
        }

        [TestCase(4, 5)]
        public void Test_False(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[10, 10]};
            var template = Block.Water;
            var mask = new TemplateMask(template);

            Assert.IsFalse(mask.Test(extent, new Vector(x, y)));
        }

        [TestCase(4, 5)]
        public void Test_True(int x, int y)
        {
            var extent = new MockExtent {Tiles = new ITile[10, 10]};
            var template = Block.Water;
            var mask = new TemplateMask(template);
            extent.SetTile(x, y, template.Apply(extent.GetTile(x, y)));

            Assert.IsTrue(mask.Test(extent, new Vector(x, y)));
        }
    }
}
