using System.IO;
using Moq;
using NUnit.Framework;
using WorldEdit.Schematics;

namespace WorldEdit.Tests.Schematics
{
    [TestFixture]
    public class TeditSchematicFormatTests
    {
        [Test]
        public void Read_MalformedStream()
        {
            var schematicFormat = new TeditSchematicFormat();
            var result = schematicFormat.Read(new MemoryStream());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Read_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new TeditSchematicFormat();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => schematicFormat.Read(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Read_StreamCannotRead_ThrowsArgumentException()
        {
            var schematicFormat = new TeditSchematicFormat();
            var stream = Mock.Of<Stream>(s => !s.CanRead);

            Assert.That(() => schematicFormat.Read(stream), Throws.ArgumentException);
        }

        [Test]
        public void Write_NullClipboard_ThrowsArgumentNullException()
        {
            var schematicFormat = new TeditSchematicFormat();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => schematicFormat.Write(null, new MemoryStream()), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new TeditSchematicFormat();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Write_StreamCannotWrite_ThrowsArgumentException()
        {
            var schematicFormat = new TeditSchematicFormat();
            var stream = Mock.Of<Stream>(s => !s.CanWrite);

            Assert.That(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), stream), Throws.ArgumentException);
        }
    }
}
