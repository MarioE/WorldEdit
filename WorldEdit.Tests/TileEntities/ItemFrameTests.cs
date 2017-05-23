using NUnit.Framework;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.TileEntities
{
    [TestFixture]
    public class ItemFrameTests
    {
        [Test]
        public void GetItem()
        {
            var item = new Item(3, 2, 1);
            var itemFrame = new ItemFrame(Vector.Zero, item);

            Assert.That(itemFrame.Item, Is.EqualTo(item));
        }

        [TestCase(1, 1)]
        public void GetPosition(int x, int y)
        {
            var itemFrame = new ItemFrame(new Vector(x, y), new Item());

            Assert.That(itemFrame.Position, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 1)]
        public void WithPosition(int x, int y)
        {
            ITileEntity itemFrame = new ItemFrame(Vector.Zero, new Item());

            itemFrame = itemFrame.WithPosition(new Vector(x, y));

            Assert.That(itemFrame.Position, Is.EqualTo(new Vector(x, y)));
        }
    }
}
