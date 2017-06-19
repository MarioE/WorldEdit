using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TShockAPI;

namespace WorldEdit.Tests
{
    [TestFixture]
    public class ExtensionsTests
    {
        [TestCase("/test abc", "test")]
        public void GetCommandName(string message, string expected)
        {
            var args = new CommandArgs(message, null, null);

            Assert.That(args.GetCommandName(), Is.EqualTo(expected));
        }

        [Test]
        public void GetCommandName_NullArgs_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((CommandArgs)null).GetCommandName(), Throws.ArgumentNullException);
        }

        [Test]
        public void ReadItem_NullReader_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((BinaryReader)null).ReadItem(), Throws.ArgumentNullException);
        }

        [Test]
        public void ReadTile_NullReader_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((BinaryReader)null).ReadTile(), Throws.ArgumentNullException);
        }

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

        [Test]
        public void SendExceptions_NullPlayer_ThrowsArgumentNullException()
        {
            var task = Task.Run(() => { });
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => task.SendExceptions(null), Throws.ArgumentNullException);
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

        [Test]
        public void WriteItem_NullWriter_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((BinaryWriter)null).Write(new Item()), Throws.ArgumentNullException);
        }

        [Test]
        public void WriteReadItem()
        {
            var item = new Item(1, 2, 3);

            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write(item);
                writer.BaseStream.Position = 0;

                using (var reader = new BinaryReader(writer.BaseStream, Encoding.Default, true))
                {
                    Assert.That(reader.ReadItem(), Is.EqualTo(item));
                }
            }
        }

        [Test]
        public void WriteReadTile()
        {
            var tile = new Tile {BlockId = 1, BTileHeader = 2, FrameX = 3, FrameY = 4, Liquid = 5, STileHeader = 6};

            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write(tile);
                writer.BaseStream.Position = 0;

                using (var reader = new BinaryReader(writer.BaseStream, Encoding.Default, true))
                {
                    Assert.That(reader.ReadTile(), Is.EqualTo(tile));
                }
            }
        }

        [Test]
        public void WriteTile_NullWriter_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => ((BinaryWriter)null).Write(new Tile()), Throws.ArgumentNullException);
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
