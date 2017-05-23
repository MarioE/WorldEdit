using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TShockAPI;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the selection functionality.
    /// </summary>
    public sealed class SelectionModule : Module
    {
        private static readonly Dictionary<string, Vector> Directions =
            new Dictionary<string, Vector>(StringComparer.OrdinalIgnoreCase)
            {
                ["down"] = new Vector(0, 1),
                ["left"] = new Vector(-1, 0),
                ["right"] = new Vector(1, 0),
                ["up"] = new Vector(0, -1)
            };

        private static readonly Dictionary<string, RegionSelector> SelectorTypes =
            new Dictionary<string, RegionSelector>(StringComparer.OrdinalIgnoreCase)
            {
                ["elliptic"] = new EllipticRegionSelector(),
                ["polygonal"] = new PolygonalRegionSelector(),
                ["rectangular"] = new RectangularRegionSelector()
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public SelectionModule([NotNull] WorldEditPlugin plugin) : base(plugin)
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

            var command = Plugin.RegisterCommand("/clear", Clear, "worldedit.selection.clear");
            command.HelpText = "Syntax: //clear\n" +
                               "Clears your selection.";

            command = Plugin.RegisterCommand("/contract", ContractExpandShift, "worldedit.selection.contract");
            command.HelpText = "Syntax: //contract <direction> <distance>\n" +
                               "Contracts your selection.";

            command = Plugin.RegisterCommand("/expand", ContractExpandShift, "worldedit.selection.expand");
            command.HelpText = "Syntax: //expand <direction> <distance>\n" +
                               "Expands your selection.";

            command = Plugin.RegisterCommand("/inset", InsetOutset, "worldedit.selection.inset");
            command.HelpText = "Syntax: //inset <distance>\n" +
                               "Insets your selection. This is equivalent to contracting on all sides.";

            command = Plugin.RegisterCommand("/outset", InsetOutset, "worldedit.selection.outset");
            command.HelpText = "Syntax: //outset <distance>\n" +
                               "Outsets your selection. This is equivalent to expanding on all sides.";

            command = Plugin.RegisterCommand("/pos1", Pos1Pos2, "worldedit.selection.pos1");
            command.HelpText = "Syntax: //pos1 <x> <y>\n" +
                               "Selects your primary position.";

            command = Plugin.RegisterCommand("/pos2", Pos1Pos2, "worldedit.selection.pos2");
            command.HelpText = "Syntax: //pos2 <x> <y>\n" +
                               "Selects your secondary position.";

            command = Plugin.RegisterCommand("/select", Select, "worldedit.selection.select");
            command.HelpText = "Syntax: //select <selector>\n" +
                               "Changes your selector. Valid selectors are:\n" +
                               "- elliptic: Select the center and radius of an ellipse.\n" +
                               "- polygonal: Select the vertices of a polygon.\n" +
                               "- rectangular: Select the two opposite points of a rectangle.";

            command = Plugin.RegisterCommand("/shift", ContractExpandShift, "worldedit.selection.shift");
            command.HelpText = "Syntax: //shift <direction> <distance>\n" +
                               "Shifts your selection.";

            command = Plugin.RegisterCommand("/wand", Wand, "worldedit.selection.wand");
            command.AllowServer = false;
            command.HelpText = "Syntax: //wand\n" +
                               "Toggles wand mode. This allows you to use wrenches to select positions and use tools.";
        }

        private void Clear(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            session.RegionSelector = session.RegionSelector.Clear();
            player.SendSuccessMessage("Cleared selection.");
        }

        private void ContractExpandShift(CommandArgs args)
        {
            var commandName = args.GetCommandName();
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 2)
            {
                player.SendErrorMessage($"Syntax: //{commandName.ToLowerInvariant()} <direction> <distance>");
                return;
            }

            var inputDirection = parameters[0];
            if (!Directions.TryGetValue(inputDirection, out var direction))
            {
                player.SendErrorMessage($"Invalid direction '{inputDirection}'.");
                return;
            }

            var inputDistance = parameters[1];
            if (!int.TryParse(inputDistance, out var distance) || distance <= 0)
            {
                player.SendErrorMessage($"Invalid distance '{inputDistance}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var selection = session.Selection;
            var resizableSelection = selection as ResizableRegion;
            if (commandName.Equals("contract", StringComparison.OrdinalIgnoreCase))
            {
                if (resizableSelection == null)
                {
                    player.SendErrorMessage("Cannot resize selection.");
                    return;
                }

                session.Selection = resizableSelection.Contract(distance * direction);
                player.SendSuccessMessage(
                    $"Contracted selection {inputDirection.ToLowerInvariant()} by {distance} tiles.");
            }
            else if (commandName.Equals("expand", StringComparison.OrdinalIgnoreCase))
            {
                if (resizableSelection == null)
                {
                    player.SendErrorMessage("Cannot resize selection.");
                    return;
                }

                session.Selection = resizableSelection.Expand(distance * direction);
                player.SendSuccessMessage(
                    $"Expanded selection {inputDirection.ToLowerInvariant()} by {distance} tiles.");
            }
            else if (commandName.Equals("shift", StringComparison.OrdinalIgnoreCase))
            {
                session.Selection = selection.Shift(distance * direction);
                player.SendSuccessMessage(
                    $"Shifted selection {inputDirection.ToLowerInvariant()} by {distance} tiles.");
            }
        }

        private void InsetOutset(CommandArgs args)
        {
            var commandName = args.GetCommandName();
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 1)
            {
                player.SendErrorMessage($"Syntax: //{commandName.ToLowerInvariant()} <distance>");
                return;
            }

            var inputDistance = parameters[0];
            if (!int.TryParse(inputDistance, out var distance) || distance <= 0)
            {
                player.SendErrorMessage($"Invalid distance '{inputDistance}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var selection = session.Selection;
            if (!(selection is ResizableRegion resizableSelection))
            {
                player.SendErrorMessage("Cannot resize selection.");
                return;
            }

            if (commandName.Equals("inset", StringComparison.OrdinalIgnoreCase))
            {
                session.Selection = resizableSelection.Inset(distance);
                player.SendSuccessMessage($"Inset selection by {distance} tiles.");
            }
            else if (commandName.Equals("outset", StringComparison.OrdinalIgnoreCase))
            {
                session.Selection = resizableSelection.Outset(distance);
                player.SendSuccessMessage($"Outset selection by {distance} tiles.");
            }
        }

        private void OnTileEdit(object sender, GetDataHandlers.TileEditEventArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            if (args.Handled || !session.IsWandMode)
            {
                return;
            }

            var action = args.Action;
            var x = args.X;
            var y = args.Y;
            var position = new Vector(x, y);
            if (action == GetDataHandlers.EditAction.PlaceWire)
            {
                session.RegionSelector = session.RegionSelector.SelectPrimary(position);
                player.SendSuccessMessage($"Set primary position to {position}.");
                args.Handled = true;
                player.SendTileSquare(x, y, 1);
            }
            else if (action == GetDataHandlers.EditAction.PlaceWire2)
            {
                session.RegionSelector = session.RegionSelector.SelectSecondary(position);
                player.SendSuccessMessage($"Set secondary position to {position}.");
                args.Handled = true;
                player.SendTileSquare(x, y, 1);
            }
        }

        private void Pos1Pos2(CommandArgs args)
        {
            var commandName = args.GetCommandName();
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 2)
            {
                player.SendErrorMessage($"Syntax: //{commandName.ToLowerInvariant()} <x> <y>");
                return;
            }

            var inputX = parameters[0];
            if (!int.TryParse(inputX, out var x))
            {
                player.SendErrorMessage($"Invalid X coordinate '{inputX}'.");
                return;
            }

            var inputY = parameters[1];
            if (!int.TryParse(inputY, out var y))
            {
                player.SendErrorMessage($"Invalid Y coordinate '{inputY}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var position = new Vector(x, y);
            if (commandName.Equals("pos1", StringComparison.OrdinalIgnoreCase))
            {
                session.RegionSelector = session.RegionSelector.SelectPrimary(position);
                player.SendSuccessMessage($"Set primary position to {position}.");
            }
            else if (commandName.Equals("pos2", StringComparison.OrdinalIgnoreCase))
            {
                session.RegionSelector = session.RegionSelector.SelectSecondary(position);
                player.SendSuccessMessage($"Set secondary position to {position}.");
            }
        }

        private void Select(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 1)
            {
                player.SendErrorMessage("Syntax: //select <selector>");
                return;
            }

            var inputSelector = parameters[0];
            if (!SelectorTypes.TryGetValue(inputSelector, out var selector))
            {
                player.SendErrorMessage($"Invalid selector '{inputSelector}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            session.RegionSelector = selector;
            player.SendSuccessMessage($"Set selector to {inputSelector.ToLowerInvariant()}.");
        }

        private void Wand(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            session.IsWandMode = !session.IsWandMode;
            player.SendSuccessMessage($"{(session.IsWandMode ? "En" : "Dis")}abled wand mode.");

            if (session.IsWandMode)
            {
                player.GiveItemIfNot(Item.RedWrench);
                player.GiveItemIfNot(Item.BlueWrench);
                player.GiveItemIfNot(Item.GreenWrench);
                player.GiveItemIfNot(Item.Wire.WithStackSize(999));
                player.SendInfoMessage("Use the red wrench to set the primary position.");
                player.SendInfoMessage("Use the blue wrench to set the secondary position.");
                player.SendInfoMessage("Use the green wrench to use tools.");
            }
        }
    }
}
