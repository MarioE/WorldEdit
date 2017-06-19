using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using TShockAPI;
using WorldEdit.Templates;
using WorldEdit.Templates.Parsers;
using WorldEdit.Tools;

namespace WorldEdit.Modules
{
    /// <summary>
    ///     Represents a module that encapsulates the tool functionality.
    /// </summary>
    [UsedImplicitly]
    public sealed class ToolModule : Module
    {
        private static readonly Dictionary<Type, TemplateParser> Parsers = new Dictionary<Type, TemplateParser>
        {
            [typeof(BlockColor)] = new PatternParser(new BlockColorParser()),
            [typeof(BlockType)] = new PatternParser(new BlockTypeParser()),
            [typeof(WallColor)] = new PatternParser(new WallColorParser()),
            [typeof(WallType)] = new PatternParser(new WallTypeParser())
        };

        private static readonly TimeSpan ToolInactivityTime = TimeSpan.FromSeconds(5);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ToolModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public ToolModule([NotNull] WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
            GetDataHandlers.TileEdit -= OnTileEdit;
        }

        /// <inheritdoc />
        public override void Register()
        {
            GetDataHandlers.TileEdit += OnTileEdit;

            var command = Plugin.RegisterCommand("/brush", Brush<BlockType>, "worldedit.tool.brush");
            command.HelpText = "Syntax: //brush <size> <pattern>\n" +
                               "Changes your tool to a brush tool that changes blocks.";

            command = Plugin.RegisterCommand("/brushwall", Brush<WallType>, "worldedit.tool.brushwall");
            command.HelpText = "Syntax: //brushwall <size> <pattern>\n" +
                               "Changes your tool to a brush tool that changes walls.";

            command = Plugin.RegisterCommand("/paintbrush", Brush<BlockColor>, "worldedit.tool.paintbrush");
            command.HelpText = "Syntax: //paintbrush <size> <pattern>\n" +
                               "Changes your tool to a brush tool that paints blocks.";

            command = Plugin.RegisterCommand("/paintbrushwall", Brush<WallColor>, "worldedit.tool.paintbrushwall");
            command.HelpText = "Syntax: //paintbrushwall <size> <pattern>\n" +
                               "Changes your tool to a brush tool that paints walls.";
        }

        private void Brush<T>(CommandArgs args) where T : class, ITemplate
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count < 2)
            {
                var commandName = args.GetCommandName();
                player.SendErrorMessage($"Syntax: //{commandName.ToLowerInvariant()} <size> <pattern>");
                return;
            }

            var inputSize = parameters[0];
            if (!int.TryParse(inputSize, out var size) || size < 0)
            {
                player.SendErrorMessage($"Invalid size '{inputSize}'.");
                return;
            }

            var inputPattern = string.Join(" ", parameters.Skip(1));
            var parser = Parsers[typeof(T)];
            var pattern = parser.Parse(inputPattern);
            if (pattern == null)
            {
                player.SendErrorMessage($"Invalid pattern '{inputPattern}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            session.Tool = new BrushTool(size, pattern);
            player.SendSuccessMessage("Set brush.");
        }

        private async void OnTileEdit(object sender, GetDataHandlers.TileEditEventArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            if (args.Handled || !session.IsWandMode)
            {
                return;
            }

            if (args.Action == GetDataHandlers.EditAction.PlaceWire3)
            {
                var x = args.X;
                var y = args.Y;
                var position = new Vector(x, y);
                player.SendTileSquare(x, y, 1);
                args.Handled = true;

                var editSession = session.ToolSession;
                if (editSession == null || DateTime.UtcNow - session.LastToolUse > ToolInactivityTime)
                {
                    editSession = session.CreateEditSession();
                    session.ToolSession = editSession;
                }

                session.LastToolUse = DateTime.UtcNow;
                await Task.Run(() => session.Tool.Apply(editSession, position)).SendExceptions(player);
            }
        }
    }
}
