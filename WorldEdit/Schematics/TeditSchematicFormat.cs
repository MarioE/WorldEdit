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
        public override Result<Clipboard> Read(Stream stream)
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
                                return Result.FromError<Clipboard>("RLE was too long.");
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
                    return Result.FromError<Clipboard>("Verification failed.");
                }

                return Result.From(new Clipboard(tiles));
            }
            catch (Exception e) when (e is EndOfStreamException || e is IOException)
            {
                return Result.FromError<Clipboard>("Stream is malformed.");
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
            // The format for header is as follows:
            // sbullwa2
            //
            // s:   RLE length is a short
            // b:   RLE length is a byte
            // u:   Tile block type is a ushort
            // ll:  Tile liquid type
            // w:   Tile has a wall
            // a:   Tile has a block
            // 2:   header2 is nonzero

            var header2 = 0;
            // The format for header2 is as follows:
            // -sssbgr3
            //
            // -:   Unused
            // sss: Tile brick style
            // b:   Tile has blue wire
            // g:   Tile has green wire
            // r:   Tile has red wire
            // 3:   header3 is nonzero

            var header3 = 0;
            // The format for header3 is as follows:
            // --ywbna-
            //
            // --:  Unused
            // y:   Tile has yellow wire
            // w:   Tile has a wall color
            // b:   Tile has a block color
            // n:   Tile is actuated
            // a:   Tile has actuator
            // -:   Unused

            if ((header & 0x1) != 0)
            {
                header2 = reader.ReadByte();
                if ((header2 & 0x1) != 0x0)
                {
                    header3 = reader.ReadByte();
                }
            }

            var tile = new Tile();
            if ((header & 0x2) != 0)
            {
                tile.IsActive = true;
                tile.Type = (header & 0x20) != 0 ? reader.ReadUInt16() : reader.ReadByte();

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

                if ((header3 & 0x8) != 0)
                {
                    tile.Color = reader.ReadByte();
                }
            }

            if ((header & 0x4) != 0)
            {
                tile.Wall = reader.ReadByte();

                if ((header3 & 0x10) != 0)
                {
                    tile.WallColor = reader.ReadByte();
                }
            }

            var liquidType = (byte)((header & 0x18) >> 3);
            if (liquidType != 0)
            {
                tile.Liquid = reader.ReadByte();
                tile.LiquidType = liquidType - 1;
            }

            if (header2 > 1)
            {
                tile.HasRedWire = (header2 & 0x2) != 0;
                tile.HasBlueWire = (header2 & 0x4) != 0;
                tile.HasGreenWire = (header2 & 0x8) != 0;
                var brickStyle = (header2 & 0x70) >> 4;
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
                tile.HasActuator = (header3 & 0x2) != 0;
                tile.IsActuated = (header3 & 0x4) != 0;
                tile.HasYellowWire = (header3 & 0x20) != 0;
            }

            repeat = 0;
            if ((header & 0x40) != 0)
            {
                repeat = reader.ReadByte();
            }
            else if ((header & 0x80) != 0)
            {
                repeat = reader.ReadInt16();
            }

            return tile;
        }

        private static byte[] SerializeTile(Tile tile, out int dataIndex, out int headerIndex)
        {
            var data = new byte[13];
            var header = 0;
            // The format for header is as follows:
            // sbullwa2
            //
            // s:   RLE length is a short
            // b:   RLE length is a byte
            // u:   Tile block type is a ushort
            // ll:  Tile liquid type
            // w:   Tile has a wall
            // a:   Tile has a block
            // 2:   header2 is nonzero

            var header2 = 0;
            // The format for header2 is as follows:
            // -sssbgr3
            //
            // -:   Unused
            // sss: Tile brick style
            // b:   Tile has blue wire
            // g:   Tile has green wire
            // r:   Tile has red wire
            // 3:   header3 is nonzero

            var header3 = 0;
            // The format for header3 is as follows:
            // --ywbna-
            //
            // --:  Unused
            // y:   Tile has yellow wire
            // w:   Tile has a wall color
            // b:   Tile has a block color
            // n:   Tile is actuated
            // a:   Tile has actuator
            // -:   Unused

            dataIndex = 3;
            if (tile.IsActive)
            {
                // header:
                // ------a-
                // Tile has a block
                header |= 0x2;
                data[dataIndex++] = (byte)tile.Type;
                if (tile.Type > 255)
                {
                    header |= 0x20;
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
                    header3 |= 0x8;
                    data[dataIndex++] = tile.Color;
                }
            }

            if (tile.Wall != 0)
            {
                header |= 0x4;
                data[dataIndex++] = tile.Wall;

                if (tile.WallColor != 0)
                {
                    header3 |= 0x10;
                    data[dataIndex++] = tile.WallColor;
                }
            }

            if (tile.Liquid != 0)
            {
                header |= (tile.LiquidType + 1) << 3;
                data[dataIndex++] = tile.Liquid;
            }

            header2 |= tile.HasRedWire ? 0x2 : 0;
            header2 |= tile.HasBlueWire ? 0x4 : 0;
            header2 |= tile.HasGreenWire ? 0x8 : 0;
            var brickStyle = tile.IsHalfBlock ? 1 : 0;
            if (tile.Slope > 0)
            {
                brickStyle = tile.Slope + 1;
            }
            header2 |= brickStyle << 4;

            header3 |= tile.HasActuator ? 0x2 : 0;
            header3 |= tile.IsActuated ? 0x4 : 0;
            header3 |= tile.HasYellowWire ? 0x20 : 0;

            headerIndex = 2;
            if (header3 != 0)
            {
                header2 |= 0x1;
                data[headerIndex--] = (byte)header3;
            }
            if (header2 != 0)
            {
                header |= 0x1;
                data[headerIndex--] = (byte)header2;
            }
            data[headerIndex] = (byte)header;

            return data;
        }
    }
}
