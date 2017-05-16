using System;
using System.IO;
using NUnit.Framework;
using WorldEdit;

namespace WorldEditTests.Schematics
{
    [TestFixture]
    public class SchematicFormatTests
    {
        [Test]
        public void Read_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new MockSchematicFormat();

            Assert.Throws<ArgumentNullException>(() => schematicFormat.Read(null));
        }


        [Test]
        public void Write_NullClipboard_ThrowsArgumentNullException()
        {
            var schematicFormat = new MockSchematicFormat();

            Assert.Throws<ArgumentNullException>(() => schematicFormat.Write(null, new MemoryStream()));
        }

        [Test]
        public void Write_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new MockSchematicFormat();

            Assert.Throws<ArgumentNullException>(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), null));
        }

        [Test]
        public void Write_ReadOnlyStream_ThrowsArgumentException()
        {
            var schematicFormat = new MockSchematicFormat();
            var stream = new MemoryStream(new byte[10], false);

            Assert.Throws<ArgumentException>(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), stream));
        }
    }
}
