using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TShockAPI;
using WorldEdit.Masks;
using WorldEdit.Templates;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Modules
{
    /// <summary>
    ///     Represents a module that encapsulates region-related functionality.
    /// </summary>
    [UsedImplicitly]
    public sealed class RegionModule : Module
    {
        private static readonly Dictionary<Type, TemplateParser> Parsers = new Dictionary<Type, TemplateParser>
        {
            [typeof(BlockColor)] = new BlockColorParser(),
            [typeof(BlockShape)] = new BlockShapeParser(),
            [typeof(BlockType)] = new BlockTypeParser(),
            [typeof(TileState)] = new TileStateParser(),
            [typeof(WallColor)] = new WallColorParser(),
            [typeof(WallType)] = new WallTypeParser()
        };

        /// <summary>
        ///     Initializes a new instance of the <see cref="RegionModule" /> class with the specified WorldEdit plugin.
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
            var command = Plugin.RegisterCommand("/paint", Set<BlockColor>, "worldedit.region.paint");
            command.HelpText = "Syntax: //paint <pattern>\n" +
                               "Paints the blocks in your selection.";

            command = Plugin.RegisterCommand("/paintwall", Set<WallColor>, "worldedit.region.paintwall");
            command.HelpText = "Syntax: //paintwall <pattern>\n" +
                               "Paints the walls in your selection.";

            command = Plugin.RegisterCommand("/replace", Replace<BlockType>, "worldedit.region.replace");
            command.HelpText = "Syntax: //replace <from-pattern>|<to-pattern>\n" +
                               "Replaces the blocks in your selection.";

            command = Plugin.RegisterCommand("/replacewall", Replace<WallType>, "worldedit.region.replacewall");
            command.HelpText = "Syntax: //replacewall <from-pattern>|<to-pattern>\n" +
                               "Replaces the walls in your selection.";

            command = Plugin.RegisterCommand("/set", Set<BlockType>, "worldedit.region.set");
            command.HelpText = "Syntax: //set <pattern>\n" +
                               "Sets the blocks in your selection.";

            command = Plugin.RegisterCommand("/setstate", Set<TileState>, "worldedit.region.setstate");
            command.HelpText = "Syntax: //setstate <pattern>\n" +
                               "Sets the tile states in your selection.";

            command = Plugin.RegisterCommand("/setwall", Set<WallType>, "worldedit.region.setwall");
            command.HelpText = "Syntax: //setwall <pattern>\n" +
                               "Sets the walls in your selection.";

            command = Plugin.RegisterCommand("/shape", Set<BlockShape>, "worldedit.region.shape");
            command.HelpText = "Syntax: //shape <pattern>\n" +
                               "Shapes the blocks in your selection.";
        }

        private async void Replace<T>(CommandArgs args) where T : class, ITemplate
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

            var parser = new PatternParser(Parsers[typeof(T)]);
            var fromPattern = parser.Parse(inputPatterns[0]);
            if (fromPattern == null)
            {
                player.SendErrorMessage($"Invalid from-pattern '{inputPatterns[0]}'.");
                return;
            }

            var toPattern = parser.Parse(inputPatterns[1]);
            if (toPattern == null)
            {
                player.SendErrorMessage($"Invalid to-pattern '{inputPatterns[1]}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession();
            var region = session.Selection;
            await Task.Run(() => session.Submit(() =>
            {
                var count = editSession.ModifyTiles(region, toPattern, new TemplateMask(fromPattern));
                player.SendSuccessMessage($"Modified {count} tiles.");
            })).SendExceptions(player);
        }

        private async void Set<T>(CommandArgs args) where T : class, ITemplate
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count < 1)
            {
                var commandName = args.GetCommandName();
                player.SendErrorMessage($"Syntax: //{commandName.ToLowerInvariant()} <pattern>");
                return;
            }

            var parser = new PatternParser(Parsers[typeof(T)]);
            var inputPattern = string.Join(" ", parameters);
            var pattern = parser.Parse(inputPattern);
            if (pattern == null)
            {
                player.SendErrorMessage($"Invalid pattern '{inputPattern}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession();
            var region = session.Selection;
            await Task.Run(() => session.Submit(() =>
            {
                var count = editSession.ModifyTiles(region, pattern);
                player.SendSuccessMessage($"Modified {count} tiles.");
            })).SendExceptions(player);
        }
    }
}
