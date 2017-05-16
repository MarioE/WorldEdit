using System;
using System.Globalization;
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
        /// <param name="args">The command arguments.</param>
        /// <returns>The executed command name.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="args" /> is <c>null</c>.</exception>
        public static string GetCommandName(this CommandArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            return args.Message.Split(' ')[0].Substring(Commands.Specifier.Length);
        }

        /// <summary>
        /// Gives an item to the player with the specified type, stack size, and prefix if the player does not already have it.
        /// </summary>
        /// <param name="player">The player to give an item.</param>
        /// <param name="type">The item type.</param>
        /// <param name="stack">The item stack size.</param>
        /// <param name="prefix">The item prefix</param>
        /// <exception cref="ArgumentNullException"><paramref name="player" /> is <c>null</c>.</exception>
        public static void GiveItemIfNot(this TSPlayer player, int type, int stack = 1, int prefix = 0)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var tplayer = player.TPlayer;
            if (!tplayer.HasItem(type))
            {
                player.GiveItem(type, "", tplayer.width, tplayer.height, stack, prefix);
            }
        }

        /// <summary>
        /// Removes the whitespace from the string.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The resulting string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static string RemoveWhitespace(this string s)
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
        /// <param name="s">The string to convert.</param>
        /// <returns>The resulting string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static string ToPascalCase(this string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant()).RemoveWhitespace();
        }
    }
}
