using System;
using System.Globalization;
using System.IO;
using JetBrains.Annotations;
using TShockAPI;

namespace WorldEdit
{
    /// <summary>
    /// Provides extension methods.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Gets the executed command name.
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
        /// Gives the specified item if the player does not already have it.
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
        /// Reads a <see cref="Vector" /> instance.
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
        /// Removes the whitespace from the string.
        /// </summary>
        /// <param name="s">The string to convert, which must not be <c>null</c>.</param>
        /// <returns>The resulting string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        [NotNull]
        [Pure]
        public static string RemoveWhiteSpace([NotNull] this string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return string.Join("", s.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Converts a string to pascal case.
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

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant()).RemoveWhiteSpace();
        }

        /// <summary>
        /// Writes the specified vector.
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
