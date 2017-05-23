using JetBrains.Annotations;
using TShockAPI;
using WorldEdit.Masks;
using WorldEdit.Templates;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates region-related functionality.
    /// </summary>
    public sealed class RegionModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public RegionModule([NotNull] WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            var command = Plugin.RegisterCommand("/paint", Set<Color>, "worldedit.region.paint");
            command.HelpText = "Syntax: //paint <pattern>\n" +
                               "Paints the blocks in your selection.";

            command = Plugin.RegisterCommand("/paintwall", Set<WallColor>, "worldedit.region.paintwall");
            command.HelpText = "Syntax: //paintwall <pattern>\n" +
                               "Paints the walls in your selection.";

            command = Plugin.RegisterCommand("/replace", Replace<Block>, "worldedit.region.replace");
            command.HelpText = "Syntax: //replace <from-pattern>|<to-pattern>\n" +
                               "Replaces the blocks in your selection.";

            command = Plugin.RegisterCommand("/replacewall", Replace<Wall>, "worldedit.region.replacewall");
            command.HelpText = "Syntax: //replacewall <from-pattern>|<to-pattern>\n" +
                               "Replaces the walls in your selection.";

            command = Plugin.RegisterCommand("/set", Set<Block>, "worldedit.region.set");
            command.HelpText = "Syntax: //set <pattern>\n" +
                               "Sets the blocks in your selection.";

            command = Plugin.RegisterCommand("/setstate", Set<State>, "worldedit.region.setstate");
            command.HelpText = "Syntax: //setstate <pattern>\n" +
                               "Sets the tile states in your selection.";

            command = Plugin.RegisterCommand("/setwall", Set<Wall>, "worldedit.region.setwall");
            command.HelpText = "Syntax: //setwall <pattern>\n" +
                               "Sets the walls in your selection.";

            command = Plugin.RegisterCommand("/shape", Set<Shape>, "worldedit.region.shape");
            command.HelpText = "Syntax: //shape <pattern>\n" +
                               "Shapes the blocks in your selection.";
        }

        private void Replace<T>(CommandArgs args) where T : class, ITemplate
        {
            var parameters = args.Parameters;
            var player = args.Player;
            var inputPatterns = string.Join(" ", parameters).Split('|');
            if (parameters.Count < 1 || inputPatterns.Length != 2)
            {
                var commandName = args.GetCommandName();
                player.SendErrorMessage($"Syntax: //{commandName.ToLowerInvariant()} <from-pattern>|<to-pattern>");
                return;
            }

            var fromPattern = Pattern<T>.TryParse(inputPatterns[0]);
            if (fromPattern == null)
            {
                player.SendErrorMessage($"Invalid from-pattern '{inputPatterns[0]}'.");
                return;
            }

            var toPattern = Pattern<T>.TryParse(inputPatterns[1]);
            if (toPattern == null)
            {
                player.SendErrorMessage($"Invalid to-pattern '{inputPatterns[1]}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            var count = editSession.ModifyTiles(session.Selection, toPattern, new TemplateMask(fromPattern));
            player.SendSuccessMessage($"Modified {count} tiles.");
        }

        private void Set<T>(CommandArgs args) where T : class, ITemplate
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count < 1)
            {
                var commandName = args.GetCommandName();
                player.SendErrorMessage($"Syntax: //{commandName.ToLowerInvariant()} <pattern>");
                return;
            }

            var inputPattern = string.Join(" ", parameters);
            var pattern = Pattern<T>.TryParse(inputPattern);
            if (pattern == null)
            {
                player.SendErrorMessage($"Invalid pattern '{inputPattern}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            var count = editSession.ModifyTiles(session.Selection, pattern);
            player.SendSuccessMessage($"Modified {count} tiles.");
        }
    }
}
