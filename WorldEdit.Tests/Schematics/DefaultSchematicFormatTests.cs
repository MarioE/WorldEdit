using System.IO;
using Moq;
using NUnit.Framework;
using Terraria;
using WorldEdit.Schematics;
using WorldEdit.TileEntities;
using Chest = WorldEdit.TileEntities.Chest;
using Sign = WorldEdit.TileEntities.Sign;

namespace WorldEdit.Tests.Schematics
{
    [TestFixture]
    public class DefaultSchematicFormatTests
    {
        [Test]
        public void Read_MalformedStream()
        {
            var schematicFormat = new DefaultSchematicFormat();
            var result = schematicFormat.Read(new MemoryStream());

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Read_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new DefaultSchematicFormat();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => schematicFormat.Read(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Read_StreamCannotRead_ThrowsArgumentException()
        {
            var schematicFormat = new DefaultSchematicFormat();
            var stream = Mock.Of<Stream>(s => !s.CanRead);

            Assert.That(() => schematicFormat.Read(stream), Throws.ArgumentException);
        }

        [Test]
        public void Write_NullClipboard_ThrowsArgumentNullException()
        {
            var schematicFormat = new DefaultSchematicFormat();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => schematicFormat.Write(null, new MemoryStream()), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_NullStream_ThrowsArgumentNullException()
        {
            var schematicFormat = new DefaultSchematicFormat();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void Write_StreamCannotWrite_ThrowsArgumentException()
        {
            var schematicFormat = new DefaultSchematicFormat();
            var stream = Mock.Of<Stream>(s => !s.CanWrite);

            Assert.That(() => schematicFormat.Write(new Clipboard(new Tile?[0, 0]), stream), Throws.ArgumentException);
        }

        [Test]
        public void WriteRead()
        {
            var tiles = new Tile?[10, 10];
            Main.tileFrameImportant[1] = true;
            var tile = new Tile {IsActive = true, BlockColor = 1, WallColor = 1, BlockId = 1, WallId = 1, Liquid = 255};
            for (var x = 0; x < 10; ++x)
            {
                for (var y = 0; y < 10; ++y)
                {
                    tiles[x, y] = tile;
                }
            }
            var clipboard = new Clipboard(tiles);
            var items = new Item[40];
            items[0] = new Item(1, 2, 3);
            var chest = new Chest(new Vector(0, 1), "Test", new Item[40]);
            var itemFrame = new ItemFrame(new Vector(3, 4), new Item(1, 2, 3));
            var logicSensor = new LogicSensor(new Vector(2, 5), LogicSensorType.Day, true, 5);
            var sign = new Sign(new Vector(1, 0), "Test2");
            var trainingDummy = new TrainingDummy(new Vector(4, 5), 21);
            clipboard.AddTileEntity(chest);
            clipboard.AddTileEntity(itemFrame);
            clipboard.AddTileEntity(logicSensor);
            clipboard.AddTileEntity(sign);
            clipboard.AddTileEntity(trainingDummy);
            var schematicFormat = new DefaultSchematicFormat();

            Clipboard clipboard2;
            using (var stream = new MemoryStream())
            {
                schematicFormat.Write(clipboard, stream);
                stream.Position = 0;
                clipboard2 = schematicFormat.Read(stream);

                Assert.That(clipboard2, Is.Not.Null);
            }

            for (var x = 0; x < 10; ++x)
            {
                for (var y = 0; y < 10; ++y)
                {
                    Assert.That(clipboard2.GetTile(new Vector(x, y)), Is.EqualTo(tile));
                }
            }
            foreach (var entity in clipboard2.GetTileEntities())
            {
                if (entity is Chest chest2)
                {
                    Assert.That(chest2.Position, Is.EqualTo(chest.Position));
                    Assert.That(chest2.Name, Is.EqualTo(chest.Name));
                    Assert.That(chest2.Items, Is.EquivalentTo(chest2.Items));
                }
                else if (entity is ItemFrame itemFrame2)
                {
                    Assert.That(itemFrame2.Position, Is.EqualTo(itemFrame.Position));
                    Assert.That(itemFrame2.Item, Is.EqualTo(itemFrame.Item));
                }
                else if (entity is LogicSensor logicSensor2)
                {
                    Assert.That(logicSensor2.Position, Is.EqualTo(logicSensor.Position));
                    Assert.That(logicSensor2.Type, Is.EqualTo(logicSensor.Type));
                    Assert.That(logicSensor2.IsEnabled, Is.EqualTo(logicSensor.IsEnabled));
                    Assert.That(logicSensor2.Data, Is.EqualTo(logicSensor.Data));
                }
                else if (entity is Sign sign2)
                {
                    Assert.That(sign2.Position, Is.EqualTo(sign.Position));
                    Assert.That(sign2.Text, Is.EqualTo(sign.Text));
                }
                else if (entity is TrainingDummy trainingDummy2)
                {
                    Assert.That(trainingDummy2.Position, Is.EqualTo(trainingDummy.Position));
                    Assert.That(trainingDummy2.NpcIndex, Is.EqualTo(trainingDummy.NpcIndex));
                }
            }
        }
    }
}
