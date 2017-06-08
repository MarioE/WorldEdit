using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TShockAPI;

namespace WorldEdit
{
    /// <summary>
    ///     Provides extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Gets the executed command name.
        /// </summary>
        /// <param name="args">The command arguments, which must not be <c>null</c>.</param>
        /// <returns>The executed command name.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="args" /> is <c>null</c>.</exception>
        public static string GetCommandName([NotNull] this CommandArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            return args.Message.Split(' ')[0].Substring(1);
        }

        /// <summary>
        ///     Gives the specified item if the player does not already have it.
        /// </summary>
        /// <param name="player">The player, which must not be <c>null</c>.</param>
        /// <param name="item">The item.</param>
        /// <exception cref="ArgumentNullException"><paramref name="player" /> is <c>null</c>.</exception>
        public static void GiveItemIfNot([NotNull] this TSPlayer player, Item item)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var tplayer = player.TPlayer;
            var type = item.Type;
            if (!tplayer.HasItem(type))
            {
                player.GiveItem(type, "", tplayer.width, tplayer.height, item.StackSize, item.Prefix);
            }
        }

        /// <summary>
        ///     Reads a <see cref="Tile" /> instance.
        /// </summary>
        /// <param name="reader">The reader, which must not be <c>null</c>.</param>
        /// <returns>The resulting tile.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <c>null</c>.</exception>
        public static Tile ReadTile([NotNull] this BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return new Tile
            {
                BlockId = reader.ReadUInt16(),
                BTileHeader = reader.ReadByte(),
                FrameX = reader.ReadInt16(),
                FrameY = reader.ReadInt16(),
                Liquid = reader.ReadByte(),
                STileHeader = reader.ReadInt16(),
                WallId = reader.ReadByte()
            };
        }

        /// <summary>
        ///     Reads a <see cref="Vector" /> instance.
        /// </summary>
        /// <param name="reader">The reader, which must not be <c>null</c>.</param>
        /// <returns>The resulting vector.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <c>null</c>.</exception>
        public static Vector ReadVector([NotNull] this BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return new Vector(reader.ReadInt32(), reader.ReadInt32());
        }

        /// <summary>
        ///     Creates a task that sends the exceptions of the task to the specified player.
        /// </summary>
        /// <param name="task">The task, which must not be <c>null</c>.</param>
        /// <param name="player">The player, which must not be <c>null</c>.</param>
        /// <returns>The task.</returns>
        /// <exception cref="ArgumentNullException">
        ///     Either <paramref name="task" /> or <paramref name="player" /> is <c>null</c>.
        /// </exception>
        [NotNull]
        public static Task SendExceptions([NotNull] this Task task, [NotNull] TSPlayer player)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            task.ContinueWith(t =>
            {
                player.SendErrorMessage("An exception occurred.");
                // ReSharper disable once PossibleNullReferenceException
                foreach (var exception in t.Exception.InnerExceptions)
                {
                    TShock.Log.Error(exception.ToString());
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
            return task;
        }

        /// <summary>
        ///     Converts a string to pascal case, removing whitespace in the process.
        /// </summary>
        /// <param name="s">The string to convert, which must not be <c>null</c>.</param>
        /// <returns>The resulting string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        [NotNull]
        [Pure]
        public static string ToPascalCase([NotNull] this string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            var result = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant());
            return string.Join("", result.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        ///     Writes the specified tile.
        /// </summary>
        /// <param name="writer">The writer, which must not be <c>null</c>.</param>
        /// <param name="tile">The tile to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer" /> is <c>null</c>.</exception>
        public static void Write([NotNull] this BinaryWriter writer, Tile tile)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.Write(tile.BlockId);
            writer.Write(tile.BTileHeader);
            writer.Write(tile.FrameX);
            writer.Write(tile.FrameY);
            writer.Write(tile.Liquid);
            writer.Write(tile.STileHeader);
            writer.Write(tile.WallId);
        }

        /// <summary>
        ///     Writes the specified vector.
        /// </summary>
        /// <param name="writer">The writer, which must not be <c>null</c>.</param>
        /// <param name="vector">The vector to write.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer" /> is <c>null</c>.</exception>
        public static void Write([NotNull] this BinaryWriter writer, Vector vector)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.Write(vector.X);
            writer.Write(vector.Y);
        }
    }
}
