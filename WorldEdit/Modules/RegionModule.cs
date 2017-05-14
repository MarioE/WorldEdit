using Terraria;
using TShockAPI;
using WorldEdit.Templates;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates region-related functionality.
    /// </summary>
    public class RegionModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        public RegionModule(WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            var color = Plugin.RegisterCommand("/color", Set<Color>, "worldedit.region.color");
            color.HelpDesc = new[]
            {
                "Syntax: //color <pattern>",
                "",
                "Colors the blocks in your selection."
            };

            var colorWall = Plugin.RegisterCommand("/colorwall", Set<WallColor>, "worldedit.region.colorwall");
            colorWall.HelpDesc = new[]
            {
                "Syntax: //colorwall <pattern>",
                "",
                "Colors the blocks in your selection."
            };

            var replace = Plugin.RegisterCommand("/replace", Replace<Block>, "worldedit.region.replace");
            replace.HelpDesc = new[]
            {
                "Syntax: //replace <from-pattern>|<to-pattern>",
                "",
                "Replaces blocks in your selection that match the from pattern."
            };

            var replaceWall = Plugin.RegisterCommand("/replacewall", Replace<Wall>, "worldedit.region.replacewall");
            replaceWall.HelpDesc = new[]
            {
                "Syntax: //replacewall <from-pattern>|<to-pattern>",
                "",
                "Replaces walls in your selection that match the from pattern."
            };

            var set = Plugin.RegisterCommand("/set", Set<Block>, "worldedit.region.set");
            set.HelpDesc = new[]
            {
                "Syntax: //set <pattern>",
                "",
                "Sets the blocks in your selection."
            };

            var setState = Plugin.RegisterCommand("/setstate", Set<State>, "worldedit.region.setstate");
            setState.HelpDesc = new[]
            {
                "Syntax: //setstate <pattern>",
                "",
                "Sets the tile states in your selection."
            };

            var setWall = Plugin.RegisterCommand("/setwall", Set<Wall>, "worldedit.region.setwall");
            setWall.HelpDesc = new[]
            {
                "Syntax: //set <pattern>",
                "",
                "Sets the walls in your selection."
            };

            var shape = Plugin.RegisterCommand("/shape", Set<Shape>, "worldedit.region.shape");
            shape.HelpDesc = new[]
            {
                "Syntax: //shape <pattern>",
                "",
                "Shapes the blocks in your selection."
            };
        }

        private void Replace<T>(CommandArgs args) where T : class, ITemplate
        {
            var parameters = args.Parameters;
            var player = args.Player;
            var input = string.Join("", parameters).Split('|');
            if (parameters.Count < 1 || input.Length != 2)
            {
                var command = args.GetCommand();
                player.SendErrorMessage($"Syntax: //{command.ToLowerInvariant()} <from-pattern>|<to-pattern>");
                return;
            }

            var fromResult = Pattern<T>.Parse(input[0]);
            if (!fromResult.WasSuccessful)
            {
                player.SendErrorMessage(fromResult.ErrorMessage);
                return;
            }

            var toResult = Pattern<T>.Parse(input[1]);
            if (!toResult.WasSuccessful)
            {
                player.SendErrorMessage(toResult.ErrorMessage);
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            var count = editSession.ReplaceTiles(session.Selection, fromResult.Value, toResult.Value);
            Netplay.ResetSections();
            player.SendSuccessMessage($"Modified {count} tiles.");
        }

        private void Set<T>(CommandArgs args) where T : class, ITemplate
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count < 1)
            {
                var command = args.GetCommand();
                player.SendErrorMessage($"Syntax: //{command.ToLowerInvariant()} <pattern>");
                return;
            }

            var result = Pattern<T>.Parse(string.Join(" ", parameters));
            if (!result.WasSuccessful)
            {
                player.SendErrorMessage(result.ErrorMessage);
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            var count = editSession.SetTiles(session.Selection, result.Value);
            Netplay.ResetSections();
            player.SendSuccessMessage($"Modified {count} tiles.");
        }
    }
}
