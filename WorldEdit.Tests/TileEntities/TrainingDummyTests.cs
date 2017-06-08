using NUnit.Framework;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.TileEntities
{
    [TestFixture]
    public class TrainingDummyTests
    {
        [TestCase(5)]
        public void GetNpcIndex(int index)
        {
            var dummy = new TrainingDummy(Vector.Zero, index);

            Assert.That(dummy.NpcIndex, Is.EqualTo(index));
        }

        [TestCase(1, 1)]
        public void GetPosition(int x, int y)
        {
            var dummy = new TrainingDummy(new Vector(x, y), -1);

            Assert.That(dummy.Position, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase(1, 1)]
        public void WithPosition(int x, int y)
        {
            ITileEntity dummy = new TrainingDummy(Vector.Zero, -1);

            dummy = dummy.WithPosition(new Vector(x, y));

            Assert.That(dummy.Position, Is.EqualTo(new Vector(x, y)));
        }
    }
}
