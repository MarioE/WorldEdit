using System.Linq;
using TShockAPI;
using WorldEdit.Templates;
using WorldEdit.Tools;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the tool functionality.
    /// </summary>
    public class ToolModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        public ToolModule(WorldEditPlugin plugin) : base(plugin)
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

            var brush = Plugin.RegisterCommand("/brush", Brush<Block>, "worldedit.tool.brush");
            brush.HelpDesc = new[]
            {
                "Syntax: //brush <size> <pattern>",
                "",
                "Changes your tool to a brush tool that changes blocks."
            };

            var brushWall = Plugin.RegisterCommand("/brushwall", Brush<Wall>, "worldedit.tool.brushwall");
            brushWall.HelpDesc = new[]
            {
                "Syntax: //brushwall <size> <pattern>",
                "",
                "Changes your tool to a brush tool that changes walls."
            };

            var colorBrush = Plugin.RegisterCommand("/paintbrush", Brush<Color>, "worldedit.tool.paintbrush");
            colorBrush.HelpDesc = new[]
            {
                "Syntax: //paintbrush <size> <pattern>",
                "",
                "Changes your tool to a brush tool that paints blocks."
            };

            var colorWallBrush = Plugin.RegisterCommand("/paintbrushwall",
                Brush<Color>,
                "worldedit.tool.paintbrushwall");
            colorWallBrush.HelpDesc = new[]
            {
                "Syntax: //paintbrushwall <size> <pattern>",
                "",
                "Changes your tool to a brush tool that paints walls."
            };
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

            var patternResult = Pattern<T>.Parse(string.Join(" ", parameters.Skip(1)));
            if (!patternResult.WasSuccessful)
            {
                player.SendErrorMessage(patternResult.ErrorMessage);
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            session.Tool = new BrushTool<T>(size, patternResult.Value);
            player.SendSuccessMessage("Set brush.");
        }

        private void OnTileEdit(object sender, GetDataHandlers.TileEditEventArgs args)
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

                // TODO: implement tool undo. Create new EditSession every 500 blocks?
                var editSession = session.CreateEditSession();
                session.Tool.Apply(editSession, position);
                args.Handled = true;
                player.SendTileSquare(x, y, 1);
            }
        }
    }
}
