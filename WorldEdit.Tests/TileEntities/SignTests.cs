using NUnit.Framework;
using WorldEdit.TileEntities;

namespace WorldEdit.Tests.TileEntities
{
    [TestFixture]
    public class SignTests
    {
        [Test]
        public void Ctor_NullText_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new Sign(Vector.Zero, null), Throws.ArgumentNullException);
        }

        [TestCase(1, 1)]
        public void GetPosition(int x, int y)
        {
            var sign = new Sign(new Vector(x, y), "");

            Assert.That(sign.Position, Is.EqualTo(new Vector(x, y)));
        }

        [TestCase("abc")]
        public void GetText(string text)
        {
            var sign = new Sign(Vector.Zero, text);

            Assert.That(sign.Text, Is.EqualTo(text));
        }

        [TestCase(1, 1)]
        public void WithPosition(int x, int y)
        {
            ITileEntity sign = new Sign(Vector.Zero, "");

            sign = sign.WithPosition(new Vector(x, y));

            Assert.That(sign.Position, Is.EqualTo(new Vector(x, y)));
        }
    }
}
