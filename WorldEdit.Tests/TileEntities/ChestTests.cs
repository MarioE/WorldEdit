using NUnit.Framework;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.TileEntities
{
    [TestFixture]
    public class ChestTests
    {
        [Test]
        public void Ctor_NullItems_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new Chest(Vector.Zero, "", null), Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new Chest(Vector.Zero, null, new Item[0]), Throws.ArgumentNullException);
        }
        
        [Test]
        public void GetItems()
        {
            var items = new[] {new Item(3, 2, 1)};
            var chest = new Chest(Vector.Zero, "", items);

            Assert.That(chest.Items.Count, Is.EqualTo(1));
            Assert.That(chest.Items[0], Is.EqualTo(items[0]));
        }

        [TestCase("abc")]
        public void GetName(string name)
        {
            var chest = new Chest(Vector.Zero, name, new Item[0]);

            Assert.That(chest.Name, Is.EqualTo(name));
        }

        [TestCase(1, 1)]
        public void GetPosition(int x, int y)
        {
            var chest = new Chest(new Vector(x, y), "", new Item[0]);

            Assert.That(chest.Position, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 1)]
        public void WithPosition(int x, int y)
        {
            ITileEntity chest = new Chest(Vector.Zero, "", new Item[0]);

            chest = chest.WithPosition(new Vector(x, y));

            Assert.That(chest.Position, Is.EqualTo(new Vector(x, y)));
        }
    }
}
