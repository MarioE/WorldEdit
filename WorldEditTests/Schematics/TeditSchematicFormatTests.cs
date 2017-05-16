using System;
using System.IO;
using NUnit.Framework;
using WorldEdit;
using WorldEdit.Schematics;

namespace WorldEditTests.Schematics
{
    [TestFixture]
    public class TeditSchematicFormatTests
    {
        [Test]
        public void Read_MalformedStream_ThrowsSchematicFormatException()
        {
            var schematicFormat = new TeditSchematicFormat();

            Assert.Throws<SchematicFormatException>(() => schematicFormat.Read(new MemoryStream()));
        }

        [Test]
        public void Read_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new TeditSchematicFormat();

            Assert.Throws<ArgumentNullException>(() => schematicFormat.Read(null));
        }

        [Test]
        public void ReadWrite()
        {
            var clipboard = new Clipboard(new Tile?[20, 20]);
            for (var x = 0; x < 20; ++x)
            {
                for (var y = 0; y < 20; ++y)
                {
                    var tile = new Tile
                    {
                        IsActive = true,
                        Type = Math.Min((ushort)470, (ushort)(x * y)),
                        Wall = (byte)(x * y)
                    };
                    clipboard.SetTile(x, y, tile);
                }
            }
            var schematicFormat = new TeditSchematicFormat();

            Clipboard clipboard2;
            using (var stream = new MemoryStream())
            {
                schematicFormat.Write(clipboard, stream);
                stream.Position = 0;
                clipboard2 = schematicFormat.Read(stream);
            }

            for (var x = 0; x < 20; ++x)
            {
                for (var y = 0; y < 20; ++y)
                {
                    var tile = clipboard2.GetTile(x, y);
                    Assert.AreEqual(Math.Min((ushort)470, (ushort)(x * y)), tile.Type);
                    Assert.AreEqual((byte)(x * y), tile.Wall);
                }
            }
        }

        [Test]
        public void Write_NullClipboard_ThrowsArgumentNullException()
        {
            var schematicFormat = new TeditSchematicFormat();

            Assert.Throws<ArgumentNullException>(() => schematicFormat.Write(null, new MemoryStream()));
        }

        [Test]
        public void Write_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new TeditSchematicFormat();

            Assert.Throws<ArgumentNullException>(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), null));
        }

        [Test]
        public void Write_ReadOnlyStream_ThrowsArgumentException()
        {
            var schematicFormat = new TeditSchematicFormat();
            var stream = new MemoryStream(new byte[10], false);

            Assert.Throws<ArgumentException>(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), stream));
        }
    }
}
