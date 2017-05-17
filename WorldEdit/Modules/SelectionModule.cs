using System;
using System.Collections.Generic;
using Terraria.ID;
using TShockAPI;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the selection functionality.
    /// </summary>
    public class SelectionModule : Module
    {
        private static readonly Dictionary<string, Vector> Directions =
            new Dictionary<string, Vector>(StringComparer.OrdinalIgnoreCase)
            {
                ["down"] = new Vector(0, 1),
                ["left"] = new Vector(-1, 0),
                ["right"] = new Vector(1, 0),
                ["up"] = new Vector(0, -1)
            };

        private static readonly Dictionary<string, Func<RegionSelector>> RegionSelectors =
            new Dictionary<string, Func<RegionSelector>>(StringComparer.OrdinalIgnoreCase)
            {
                ["elliptic"] = () => new EllipticRegionSelector(),
                ["polygonal"] = () => new PolygonalRegionSelector(),
                ["rectangular"] = () => new RectangularRegionSelector()
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        public SelectionModule(WorldEditPlugin plugin) : base(plugin)
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

            var clear = Plugin.RegisterCommand("/clear", Clear, "worldedit.selection.clear");
            clear.HelpDesc = new[]
            {
                "Syntax: //clear",
                "",
                "Clears your selection."
            };

            var contract = Plugin.RegisterCommand("/contract", ContractExpandShift, "worldedit.selection.contract");
            contract.HelpDesc = new[]
            {
                "Syntax: //contract <direction> <distance>",
                "",
                "Contracts your selection."
            };

            var expand = Plugin.RegisterCommand("/expand", ContractExpandShift, "worldedit.selection.expand");
            expand.HelpDesc = new[]
            {
                "Syntax: //expand <direction> <distance>",
                "",
                "Expands your selection."
            };

            var inset = Plugin.RegisterCommand("/inset", InsetOutset, "worldedit.selection.inset");
            inset.HelpDesc = new[]
            {
                "Syntax: //inset <distance>",
                "",
                "Insets your selection. This is equivalent to contracting on all sides."
            };

            var outset = Plugin.RegisterCommand("/outset", InsetOutset, "worldedit.selection.outset");
            outset.HelpDesc = new[]
            {
                "Syntax: //outset <distance>",
                "",
                "Outsets your selection. This is equivalent to expanding on all sides."
            };

            var pos1 = Plugin.RegisterCommand("/pos1", Pos1Pos2, "worldedit.selection.pos1");
            pos1.HelpDesc = new[]
            {
                "Syntax: //pos1 <x> <y>",
                "",
                "Selects your primary position."
            };

            var pos2 = Plugin.RegisterCommand("/pos2", Pos1Pos2, "worldedit.selection.pos2");
            pos2.HelpDesc = new[]
            {
                "Syntax: //pos2 <x> <y>",
                "",
                "Selects your secondary position."
            };

            var select = Plugin.RegisterCommand("/select", Select, "worldedit.selection.select");
            select.HelpDesc = new[]
            {
                "Syntax: //select <selector>",
                "",
                "Changes your selector. Valid selectors are:",
                "- elliptic: Select the center and radius of an ellipse.",
                "- polygonal: Select the vertices of a polygon.",
                "- rectangular: Select the two opposite points of a rectangle."
            };

            var shift = Plugin.RegisterCommand("/shift", ContractExpandShift, "worldedit.selection.shift");
            shift.HelpDesc = new[]
            {
                "Syntax: //shift <direction> <distance>",
                "",
                "Shifts your selection."
            };

            var wand = Plugin.RegisterCommand("/wand", Wand, "worldedit.selection.wand");
            wand.AllowServer = false;
            wand.HelpDesc = new[]
            {
                "Syntax: //wand",
                "",
                "Toggles wand mode. This allows you to use wrenches to select positions and use tools."
            };
        }

        private void Clear(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            session.Selection = new NullRegion();
            session.RegionSelector.Clear();
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
            if (commandName.Equals("contract", StringComparison.OrdinalIgnoreCase))
            {
                if (!selection.CanContract)
                {
                    player.SendErrorMessage("Cannot contract selection.");
                    return;
                }

                session.Selection = selection.Contract(distance * direction);
                player.SendSuccessMessage(
                    $"Contracted selection {inputDirection.ToLowerInvariant()} by {distance} tiles.");
            }
            else if (commandName.Equals("expand", StringComparison.OrdinalIgnoreCase))
            {
                if (!selection.CanExpand)
                {
                    player.SendErrorMessage("Cannot expand selection.");
                    return;
                }

                session.Selection = selection.Expand(distance * direction);
                player.SendSuccessMessage(
                    $"Expanded selection {inputDirection.ToLowerInvariant()} by {distance} tiles.");
            }
            else if (commandName.Equals("shift", StringComparison.OrdinalIgnoreCase))
            {
                if (!selection.CanShift)
                {
                    player.SendErrorMessage("Cannot shift selection.");
                    return;
                }

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
            if (commandName.Equals("inset", StringComparison.OrdinalIgnoreCase))
            {
                if (!selection.CanContract)
                {
                    player.SendErrorMessage("Cannot inset selection.");
                    return;
                }

                session.Selection = selection.Inset(distance);
                player.SendSuccessMessage($"Inset selection by {distance} tiles.");
            }
            else if (commandName.Equals("outset", StringComparison.OrdinalIgnoreCase))
            {
                if (!selection.CanExpand)
                {
                    player.SendErrorMessage("Cannot outset selection.");
                    return;
                }

                session.Selection = selection.Outset(distance);
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
            var regionSelector = session.RegionSelector;
            if (action == GetDataHandlers.EditAction.PlaceWire)
            {
                session.Selection = regionSelector.SelectPrimary(position);
                player.SendSuccessMessage($"Set primary position to {position}.");
                args.Handled = true;
                player.SendTileSquare(x, y, 1);
            }
            else if (action == GetDataHandlers.EditAction.PlaceWire2)
            {
                session.Selection = regionSelector.SelectSecondary(position);
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
            var regionSelector = session.RegionSelector;
            var position = new Vector(x, y);
            if (commandName.Equals("pos1", StringComparison.OrdinalIgnoreCase))
            {
                session.Selection = regionSelector.SelectPrimary(position);
                player.SendSuccessMessage($"Set primary position to {position}.");
            }
            else if (commandName.Equals("pos2", StringComparison.OrdinalIgnoreCase))
            {
                session.Selection = regionSelector.SelectSecondary(position);
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
            if (!RegionSelectors.TryGetValue(inputSelector, out var selector))
            {
                player.SendErrorMessage($"Invalid selector '{inputSelector}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            session.RegionSelector = selector();
            session.Selection = new NullRegion();
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
                player.GiveItemIfNot(ItemID.Wrench);
                player.GiveItemIfNot(ItemID.BlueWrench);
                player.GiveItemIfNot(ItemID.GreenWrench);
                player.GiveItemIfNot(ItemID.Wire, 999);
                player.SendInfoMessage("Use the red wrench to set the primary position.");
                player.SendInfoMessage("Use the blue wrench to set the secondary position.");
                player.SendInfoMessage("Use the green wrench to use tools.");
            }
        }
    }
}
