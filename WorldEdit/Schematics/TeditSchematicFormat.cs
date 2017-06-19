using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using WorldEdit.Extents;
using Chest = WorldEdit.TileEntities.Chest;
using Sign = WorldEdit.TileEntities.Sign;

namespace WorldEdit.Schematics
{
    /// <summary>
    ///     Represents the TEdit schematic format.
    /// </summary>
    /// <remarks>
    ///     The TEdit schematic format is only capable of saving rectangles, and does not have support for tile entities
    ///     besides chests and signs.
    /// </remarks>
    public sealed class TeditSchematicFormat : SchematicFormat
    {
        private const short ItemsPerChest = 40;
        private const uint Version = 192;

        private static IEnumerable<Chest> DeserializeChests(BinaryReader reader)
        {
            var count = reader.ReadInt16();
            var maxItems = reader.ReadInt16();
            for (var i = 0; i < count; ++i)
            {
                var position = reader.ReadVector();
                var name = reader.ReadString();
                var items = new Item[ItemsPerChest];
                for (var j = 0; j < maxItems; ++j)
                {
                    var stackSize = reader.ReadInt16();
                    if (stackSize > 0)
                    {
                        var type = reader.ReadInt32();
                        var prefix = reader.ReadByte();

                        if (j < ItemsPerChest)
                        {
                            items[j] = new Item(type, stackSize, prefix);
                        }
                    }
                }
                yield return new Chest(position, name, items);
            }
        }

        private static IEnumerable<Sign> DeserializeSigns(BinaryReader reader)
        {
            var count = reader.ReadInt16();
            for (var i = 0; i < count; ++i)
            {
                var text = reader.ReadString();
                var position = reader.ReadVector();
                yield return new Sign(position, text);
            }
        }

        private static Tile DeserializeTile(BinaryReader reader, out int repeat)
        {
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
            var header = reader.ReadByte();

            // The format for header2 is as follows:
            // -sssbgr3
            //
            // -:   Unused
            // sss: Tile brick style
            // b:   Tile has blue wire
            // g:   Tile has green wire
            // r:   Tile has red wire
            // 3:   header3 is nonzero
            var header2 = 0;

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
            var header3 = 0;

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
                tile.BlockId = (header & 0x20) != 0 ? reader.ReadUInt16() : reader.ReadByte();

                if (Main.tileFrameImportant[tile.BlockId])
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
                    tile.BlockColor = reader.ReadByte();
                }
            }

            if ((header & 0x4) != 0)
            {
                tile.WallId = reader.ReadByte();

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

        private static Tile?[,] DeserializeTiles(BinaryReader reader, int width, int height)
        {
            var tiles = new Tile?[width, height];
            for (var x = 0; x < width; ++x)
            {
                for (var y = 0; y < height; ++y)
                {
                    var tile = DeserializeTile(reader, out var repeat);
                    tiles[x, y] = tile;
                    while (repeat-- > 0)
                    {
                        tiles[x, ++y] = tile;
                    }
                }
            }
            return tiles;
        }

        private static void SerializeChests(BinaryWriter writer, ICollection<Chest> chests)
        {
            writer.Write((short)chests.Count);
            writer.Write(ItemsPerChest);
            foreach (var chest in chests)
            {
                writer.Write(chest.Position);
                writer.Write(chest.Name);
                for (var i = 0; i < ItemsPerChest; ++i)
                {
                    var item = chest.Items[i];
                    var stackSize = (short)item.StackSize;
                    writer.Write(stackSize);
                    if (stackSize > 0)
                    {
                        writer.Write(item.Type);
                        writer.Write(item.Prefix);
                    }
                }
            }
        }

        private static void SerializeSigns(BinaryWriter writer, ICollection<Sign> signs)
        {
            writer.Write((short)signs.Count);
            foreach (var sign in signs)
            {
                writer.Write(sign.Text);
                writer.Write(sign.Position);
            }
        }

        private static byte[] SerializeTile(Tile tile, out int dataIndex, out int headerIndex)
        {
            var data = new byte[13];

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
            var header = 0;

            // The format for header2 is as follows:
            // -sssbgr3
            //
            // -:   Unused
            // sss: Tile brick style
            // b:   Tile has blue wire
            // g:   Tile has green wire
            // r:   Tile has red wire
            // 3:   header3 is nonzero
            var header2 = 0;

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
            var header3 = 0;

            dataIndex = 3;
            if (tile.IsActive)
            {
                header |= 0x2;
                data[dataIndex++] = (byte)tile.BlockId;
                if (tile.BlockId > 255)
                {
                    header |= 0x20;
                    data[dataIndex++] = (byte)(tile.BlockId >> 8);
                }

                if (Main.tileFrameImportant[tile.BlockId])
                {
                    data[dataIndex++] = (byte)tile.FrameX;
                    data[dataIndex++] = (byte)(tile.FrameX >> 8);
                    data[dataIndex++] = (byte)tile.FrameY;
                    data[dataIndex++] = (byte)(tile.FrameY >> 8);
                }

                if (tile.BlockColor != 0)
                {
                    header3 |= 0x8;
                    data[dataIndex++] = tile.BlockColor;
                }
            }

            if (tile.WallId != 0)
            {
                header |= 0x4;
                data[dataIndex++] = tile.WallId;

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

        private static void SerializeTiles(BinaryWriter writer, Extent extent)
        {
            var dimensions = extent.Dimensions;
            for (var x = 0; x < dimensions.X; ++x)
            {
                for (var y = 0; y < dimensions.Y;)
                {
                    var tile = extent.GetTile(new Vector(x, y));
                    var data = SerializeTile(tile, out var dataIndex, out var headerIndex);

                    short repeat = 0;
                    while (++y < dimensions.Y && tile == extent.GetTile(new Vector(x, y)))
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

            var reader = new BinaryReader(stream, Encoding.Default, true);
            try
            {
                var name = reader.ReadString();
                var version = reader.ReadUInt32();
                var width = reader.ReadInt32();
                var height = reader.ReadInt32();

                var clipboard = new Clipboard(DeserializeTiles(reader, width, height));
                foreach (var chest in DeserializeChests(reader))
                {
                    clipboard.AddTileEntity(chest);
                }
                foreach (var sign in DeserializeSigns(reader))
                {
                    clipboard.AddTileEntity(sign);
                }

                var name2 = reader.ReadString();
                var version2 = reader.ReadUInt32();
                var width2 = reader.ReadInt32();
                var height2 = reader.ReadInt32();
                if (name != name2 || version != version2 || width != width2 || height != height2)
                {
                    return null;
                }

                return clipboard;
            }
            catch (Exception e) when (e is EndOfStreamException || e is IndexOutOfRangeException || e is IOException)
            {
                return null;
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

                SerializeTiles(writer, clipboard);
                var chests = clipboard.GetTileEntities().OfType<Chest>().ToList();
                SerializeChests(writer, chests);
                var signs = clipboard.GetTileEntities().OfType<Sign>().ToList();
                SerializeSigns(writer, signs);

                writer.Write("Schematic");
                writer.Write(Version);
                writer.Write(dimensions.X);
                writer.Write(dimensions.Y);
            }
        }
    }
}
