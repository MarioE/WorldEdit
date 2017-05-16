using System;
using System.IO;
using System.Text;
using Terraria;

namespace WorldEdit.Schematics
{
    /// <summary>
    /// Represents the TEdit schematic format.
    /// </summary>
    public class TeditSchematicFormat : SchematicFormat
    {
        private const uint Version = 192;
        
        /// <inheritdoc />
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

            var reader = new BinaryReader(stream, Encoding.Default, true);
            try
            {
                var name = reader.ReadString();
                var version = reader.ReadUInt32();
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();

                var tiles = new Tile?[width, height];
                for (var x = 0; x < width; ++x)
                {
                    for (var y = 0; y < height; ++y)
                    {
                        var tile = DeserializeTile(reader, out var repeat);
                        tiles[x, y] = tile;
                        while (repeat-- > 0)
                        {
                            if (++y >= height)
                            {
                                throw new SchematicFormatException("Stream is malformed.");
                            }

                            tiles[x, y] = tile;
                        }
                    }
                }

                DeserializeChests(reader);
                DeserializeSigns(reader);

                var name2 = reader.ReadString();
                var version2 = reader.ReadUInt32();
                var width2 = reader.ReadInt32();
                var height2 = reader.ReadInt32();
                if (name != name2 || version != version2 || width != width2 || height != height2)
                {
                    throw new SchematicFormatException("Stream is malformed.");
                }

                return new Clipboard(tiles);
            }
            catch (Exception e) when (e is EndOfStreamException || e is IOException)
            {
                throw new SchematicFormatException("Stream is malformed.", e);
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <inheritdoc />
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

            using (var writer = new BinaryWriter(stream, Encoding.Default, true))
            {
                var dimensions = clipboard.Dimensions;
                writer.Write("Schematic");
                writer.Write(Version);
                writer.Write(dimensions.X);
                writer.Write(dimensions.Y);

                var lowerBound = clipboard.LowerBound;
                var upperBound = clipboard.UpperBound;
                for (var x = lowerBound.X; x < upperBound.X; ++x)
                {
                    for (var y = lowerBound.Y; y < upperBound.Y;)
                    {
                        var tile = clipboard.GetTile(x, y);
                        var data = SerializeTile(tile, out var dataIndex, out var headerIndex);

                        short repeat = 0;
                        while (++y < upperBound.Y && tile.Equals(clipboard.GetTile(x, y)))
                        {
                            ++repeat;
                        }

                        if (repeat > 0)
                        {
                            data[dataIndex++] = (byte)repeat;
                            if (repeat <= 255)
                            {
                                data[headerIndex] |= 64;
                            }
                            else
                            {
                                data[headerIndex] |= 128;
                                data[dataIndex++] = (byte)(repeat >> 8);
                            }
                        }

                        writer.Write(data, headerIndex, dataIndex - headerIndex);
                    }
                }

                // TODO: chest data
                writer.Write((short)0);
                writer.Write((short)0);
                // TODO: sign data
                writer.Write((short)0);

                writer.Write("Schematic");
                writer.Write(Version);
                writer.Write(dimensions.X);
                writer.Write(dimensions.Y);
            }
        }

        // TODO: Update to properly handle chests whenever extents get entities
        private static void DeserializeChests(BinaryReader reader)
        {
            var count = reader.ReadInt16();
            var maxItems = reader.ReadInt16();
            for (var i = 0; i < count; ++i)
            {
                reader.ReadInt32(); // X
                reader.ReadInt32(); // Y
                reader.ReadString(); // Name

                for (var j = 0; j < maxItems; ++j)
                {
                    var stackSize = reader.ReadInt16();
                    if (stackSize > 0)
                    {
                        reader.ReadInt32(); // ID
                        reader.ReadByte(); // Prefix
                    }
                }
            }
        }

        // TODO: Update to properly handle signs whenever extents get entities
        private static void DeserializeSigns(BinaryReader reader)
        {
            var count = reader.ReadInt16();
            for (var i = 0; i < count; ++i)
            {
                reader.ReadString(); // Text
                reader.ReadInt32(); // X
                reader.ReadInt32(); // Y
            }
        }

        private static Tile DeserializeTile(BinaryReader reader, out int repeat)
        {
            var header = reader.ReadByte();
            byte header2 = 0;
            byte header3 = 0;
            if ((header & 1) != 0)
            {
                header2 = reader.ReadByte();
                if ((header2 & 1) != 0)
                {
                    header3 = reader.ReadByte();
                }
            }

            var tile = new Tile();
            if ((header & 2) != 0)
            {
                tile.IsActive = true;
                tile.Type = (header & 32) == 0 ? reader.ReadByte() : reader.ReadUInt16();

                if (Main.tileFrameImportant[tile.Type])
                {
                    tile.FrameX = reader.ReadInt16();
                    tile.FrameY = reader.ReadInt16();
                }
                else
                {
                    tile.FrameX = -1;
                    tile.FrameY = -1;
                }

                if ((header3 & 8) != 0)
                {
                    tile.Color = reader.ReadByte();
                }
            }

            if ((header & 4) != 0)
            {
                tile.Wall = reader.ReadByte();

                if ((header3 & 16) != 0)
                {
                    tile.WallColor = reader.ReadByte();
                }
            }

            var liquidType = (byte)((header & 24) >> 3);
            if (liquidType != 0)
            {
                tile.Liquid = reader.ReadByte();
                tile.LiquidType = liquidType - 1;
            }

            if (header2 > 1)
            {
                tile.HasRedWire = (header2 & 2) != 0;
                tile.HasBlueWire = (header2 & 4) != 0;
                tile.HasGreenWire = (header2 & 8) != 0;
                var brickStyle = (header2 & 112) >> 4;
                if (brickStyle == 1)
                {
                    tile.IsHalfBlock = true;
                }
                else if (brickStyle > 0)
                {
                    tile.Slope = brickStyle - 1;
                }
            }

            if (header3 > 1)
            {
                tile.HasActuator = (header3 & 2) != 0;
                tile.IsActuated = (header3 & 4) != 0;
                tile.HasYellowWire = (header3 & 32) != 0;
            }

            repeat = 0;
            if ((header & 64) != 0)
            {
                repeat = reader.ReadByte();
            }
            else if ((header & 128) != 0)
            {
                repeat = reader.ReadInt16();
            }

            return tile;
        }

        private static byte[] SerializeTile(Tile tile, out int dataIndex, out int headerIndex)
        {
            var data = new byte[13];
            byte header = 0;
            byte header2 = 0;
            byte header3 = 0;
            dataIndex = 3;

            if (tile.IsActive)
            {
                header |= 2;
                data[dataIndex++] = (byte)tile.Type;
                if (tile.Type > 255)
                {
                    header |= 32;
                    data[dataIndex++] = (byte)(tile.Type >> 8);
                }

                if (Main.tileFrameImportant[tile.Type])
                {
                    data[dataIndex++] = (byte)tile.FrameX;
                    data[dataIndex++] = (byte)(tile.FrameX >> 8);
                    data[dataIndex++] = (byte)tile.FrameY;
                    data[dataIndex++] = (byte)(tile.FrameY >> 8);
                }

                if (tile.Color != 0)
                {
                    header3 |= 8;
                    data[dataIndex++] = tile.Color;
                }
            }

            if (tile.Wall != 0)
            {
                header |= 4;
                data[dataIndex++] = tile.Wall;

                if (tile.WallColor != 0)
                {
                    header3 |= 16;
                    data[dataIndex++] = tile.WallColor;
                }
            }

            if (tile.Liquid != 0)
            {
                header |= (byte)((tile.LiquidType + 1) << 3);
                data[dataIndex++] = tile.Liquid;
            }

            header2 |= (byte)(tile.HasRedWire ? 2 : 0);
            header2 |= (byte)(tile.HasBlueWire ? 4 : 0);
            header2 |= (byte)(tile.HasGreenWire ? 8 : 0);
            var brickStyle = tile.IsHalfBlock ? 1 : 0;
            if (tile.Slope > 0)
            {
                brickStyle = tile.Slope + 1;
            }
            header2 |= (byte)(brickStyle << 4);

            header3 |= (byte)(tile.HasActuator ? 2 : 0);
            header3 |= (byte)(tile.IsActuated ? 4 : 0);
            header3 |= (byte)(tile.HasYellowWire ? 32 : 0);

            headerIndex = 2;
            if (header3 != 0)
            {
                header2 |= 1;
                data[headerIndex--] = header3;
            }
            if (header2 != 0)
            {
                header |= 1;
                data[headerIndex--] = header2;
            }
            data[headerIndex] = header;

            return data;
        }
    }
}
