using NUnit.Framework;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class ItemTests
    {
        [TestCase(4, 1, 0, 4, 1, 0, true)]
        [TestCase(4, 1, 0, 4, 1, 1, false)]
        [TestCase(4, 1, 0, 4, 0, 0, false)]
        public void Equals(int type, int stackSize, byte prefix, int type2, int stackSize2, byte prefix2, bool expected)
        {
            var item1 = new Item(type, stackSize, prefix);
            var item2 = new Item(type2, stackSize2, prefix2);

            Assert.That(item1.Equals(item2), Is.EqualTo(expected));
        }

        [Test]
        public void Equals_Null_False()
        {
            var item = new Item();

            Assert.That(!item.Equals(null));
        }

        [Test]
        public void Equals_String_False()
        {
            var item = new Item();

            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.That(!item.Equals(""));
        }

        [TestCase(4, 1, 0)]
        public void GetHashCode_IsConsistent(int type, int stackSize, byte prefix)
        {
            var item1 = new Item(type, stackSize, prefix);
            var item2 = new Item(type, stackSize, prefix);

            Assert.That(item2.GetHashCode(), Is.EqualTo(item1.GetHashCode()));
        }

        [TestCase(6)]
        public void GetPrefix(byte prefix)
        {
            var item = new Item(1, 1, prefix);

            Assert.That(item.Prefix, Is.EqualTo(prefix));
        }

        [TestCase(5)]
        public void GetStackSize(int stackSize)
        {
            var item = new Item(1, stackSize);

            Assert.That(item.StackSize, Is.EqualTo(stackSize));
        }

        [TestCase(4)]
        public void GetType(int type)
        {
            var item = new Item(type);

            Assert.That(item.Type, Is.EqualTo(type));
        }

        [TestCase(4, 1, 0, 4, 1, 0, true)]
        [TestCase(4, 1, 0, 4, 1, 1, false)]
        [TestCase(4, 1, 0, 4, 0, 0, false)]
        public void OpEquality(int type, int stackSize, byte prefix, int type2, int stackSize2, byte prefix2,
            bool expected)
        {
            var item1 = new Item(type, stackSize, prefix);
            var item2 = new Item(type2, stackSize2, prefix2);

            Assert.That(item1 == item2, Is.EqualTo(expected));
        }

        [TestCase(4, 1, 0, 4, 1, 0, false)]
        [TestCase(4, 1, 0, 4, 1, 1, true)]
        [TestCase(4, 1, 0, 4, 0, 0, true)]
        public void OpInequality(int type, int stackSize, byte prefix, int type2, int stackSize2, byte prefix2,
            bool expected)
        {
            var item1 = new Item(type, stackSize, prefix);
            var item2 = new Item(type2, stackSize2, prefix2);

            Assert.That(item1 != item2, Is.EqualTo(expected));
        }

        [TestCase(4)]
        public void WithPrefix(byte prefix)
        {
            var item = new Item(1);

            item = item.WithPrefix(prefix);

            Assert.That(item.Prefix, Is.EqualTo(prefix));
        }

        [TestCase(4)]
        public void WithStackSize(int stackSize)
        {
            var item = new Item(1);

            item = item.WithStackSize(stackSize);

            Assert.That(item.StackSize, Is.EqualTo(stackSize));
        }
    }
}
