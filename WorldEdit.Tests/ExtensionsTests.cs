using System.IO;
using System.Text;
using NUnit.Framework;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [TestCase(3, 4)]
        public void ReadVector(int x, int y)
        {
            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write(x);
                writer.Write(y);
                writer.BaseStream.Position = 0;

                using (var reader = new BinaryReader(writer.BaseStream, Encoding.Default, true))
                {
                    Assert.That(reader.ReadVector(), Is.EqualTo(new Vector(x, y)));
                }
            }
        }

        [Test]
        public void ReadVector_NullReader_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((BinaryReader)null).ReadVector(), Throws.ArgumentNullException);
        }

        [TestCase("       a   ab", "aab")]
        [TestCase("     \n\n\n\t a  \t\r\n ab", "aab")]
        public void RemoveWhiteSpace(string s, string expected)
        {
            Assert.That(s.RemoveWhiteSpace(), Is.EqualTo(expected));
        }

        [Test]
        public void RemoveWhiteSpace_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((string)null).RemoveWhiteSpace(), Throws.ArgumentNullException);
        }

        [TestCase("asdf", "Asdf")]
        [TestCase("as df", "AsDf")]
        public void ToPascalCase(string s, string expected)
        {
            Assert.That(s.ToPascalCase(), Is.EqualTo(expected));
        }

        [Test]
        public void ToPascalCase_NullS_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((string)null).ToPascalCase(), Throws.ArgumentNullException);
        }

        [TestCase(3, 4)]
        public void WriteVector(int x, int y)
        {
            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write(new Vector(x, y));
                writer.BaseStream.Position = 0;

                using (var reader = new BinaryReader(writer.BaseStream, Encoding.Default, true))
                {
                    Assert.That(reader.ReadInt32(), Is.EqualTo(x));
                    Assert.That(reader.ReadInt32(), Is.EqualTo(y));
                }
            }
        }

        [Test]
        public void WriteVector_NullWriter_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((BinaryWriter)null).Write(Vector.Zero), Throws.ArgumentNullException);
        }
    }
}
