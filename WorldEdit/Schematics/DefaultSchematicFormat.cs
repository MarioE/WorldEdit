using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using WorldEdit.TileEntities;

namespace WorldEdit.Schematics
{
    /// <summary>
    ///     Represents the default schematic format.
    /// </summary>
    public sealed class DefaultSchematicFormat : SchematicFormat
    {
        private const uint Version = 1;

        private static IEnumerable<ITileEntity> ReadTileEntities(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (var i = 0; i < count; ++i)
            {
                var position = reader.ReadVector();
                switch (reader.ReadInt32())
                {
                    case 0:
                        var name = reader.ReadString();
                        var items = new Item[reader.ReadInt32()];
                        for (var j = 0; j < items.Length; ++j)
                        {
                            items[j] = reader.ReadItem();
                        }
                        yield return new Chest(position, name, items);
                        break;
                    case 1:
                        yield return new ItemFrame(position, reader.ReadItem());
                        break;
                    case 2:
                        yield return new LogicSensor(position, (LogicSensorType)reader.ReadInt32(),
                            reader.ReadBoolean(), reader.ReadInt32());
                        break;
                    case 3:
                        yield return new Sign(position, reader.ReadString());
                        break;
                    case 4:
                        yield return new TrainingDummy(position, reader.ReadInt32());
                        break;
                }
            }
        }

        private static void WriteTileEntities(BinaryWriter writer, ICollection<ITileEntity> tileEntities)
        {
            writer.Write(tileEntities.Count);
            foreach (var tileEntity in tileEntities)
            {
                writer.Write(tileEntity.Position);
                switch (tileEntity)
                {
                    case Chest chest:
                        writer.Write(0);
                        writer.Write(chest.Name);
                        var items = chest.Items;
                        writer.Write(items.Count);
                        foreach (var item in items)
                        {
                            writer.Write(item);
                        }
                        break;
                    case ItemFrame itemFrame:
                        writer.Write(1);
                        writer.Write(itemFrame.Item);
                        break;
                    case LogicSensor logicSensor:
                        writer.Write(2);
                        writer.Write((int)logicSensor.Type);
                        writer.Write(logicSensor.IsEnabled);
                        writer.Write(logicSensor.Data);
                        break;
                    case Sign sign:
                        writer.Write(3);
                        writer.Write(sign.Text);
                        break;
                    case TrainingDummy trainingDummy:
                        writer.Write(4);
                        writer.Write(trainingDummy.NpcIndex);
                        break;
                }
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override Clipboard Read(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream must support reading.", nameof(stream));
            }

            Clipboard clipboard;
            var gzipStream = new GZipStream(stream, CompressionMode.Decompress, true);
            var reader = new BinaryReader(gzipStream, Encoding.Default, true);
            try
            {
                reader.ReadUInt32(); // Version, currently not used.
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();
                clipboard = new Clipboard(new Tile?[width, height]);
                for (var x = 0; x < width; ++x)
                {
                    for (var y = 0; y < height; ++y)
                    {
                        clipboard.SetTile(new Vector(x, y), reader.ReadTile());
                    }
                }
                foreach (var tileEntity in ReadTileEntities(reader))
                {
                    clipboard.AddTileEntity(tileEntity);
                }
            }
            catch (Exception e) when (e is EndOfStreamException || e is IndexOutOfRangeException || e is IOException)
            {
                return null;
            }
            finally
            {
                gzipStream.Dispose();
                reader.Dispose();
            }
            return clipboard;
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public override void Write(Clipboard clipboard, Stream stream)
        {
            if (clipboard == null)
            {
                throw new ArgumentNullException(nameof(clipboard));
            }
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Stream must support writing.", nameof(stream));
            }

            var dimensions = clipboard.Dimensions;
            using (var gzipStream = new GZipStream(stream, CompressionLevel.Fastest, true))
            using (var writer = new BinaryWriter(gzipStream, Encoding.Default, true))
            {
                writer.Write(Version);
                writer.Write(dimensions.X);
                writer.Write(dimensions.Y);
                for (var x = 0; x < dimensions.X; ++x)
                {
                    for (var y = 0; y < dimensions.Y; ++y)
                    {
                        writer.Write(clipboard.GetTile(new Vector(x, y)));
                    }
                }
                WriteTileEntities(writer, clipboard.GetTileEntities().ToList());
            }
        }
    }
}
