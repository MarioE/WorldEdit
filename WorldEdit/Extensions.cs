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
        /// Converts the string into title case.
        /// </summary>
        /// <param name="s">The string to convert.</param>
        /// <returns>The converted string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s" /> is <c>null</c>.</exception>
        public static string ToTitleCase(this string s)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLowerInvariant());
        }
    }
}
