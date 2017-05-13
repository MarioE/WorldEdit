using System;
using System.Collections.Generic;
using Terraria.ID;
using TShockAPI;
using WorldEdit.Regions;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the selection functionality.
    /// </summary>
    public class SelectionModule : Module
    {
        private readonly Dictionary<string, Vector> _directions =
            new Dictionary<string, Vector>(StringComparer.OrdinalIgnoreCase)
            {
                ["down"] = new Vector(0, 1),
                ["left"] = new Vector(-1, 0),
                ["right"] = new Vector(1, 0),
                ["up"] = new Vector(0, -1)
            };

        private readonly Dictionary<string, Func<RegionSelector>> _selectors =
            new Dictionary<string, Func<RegionSelector>>(StringComparer.OrdinalIgnoreCase)
            {
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

            var select = Plugin.RegisterCommand("/select", Select, "worldedit.selection.select");
            select.HelpDesc = new[]
            {
                "Syntax: //select <selector>",
                "",
                "Changes your selector. Valid selectors are:",
                "- rectangular: Select two opposite points of a rectangle."
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
                "Toggles wand mode. This allows you to use wrenches to select positions."
            };
        }

        private void ContractExpandShift(CommandArgs args)
        {
            var command = args.Message.Split(' ')[0].Substring(1).ToLowerInvariant();
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 2)
            {
                player.SendErrorMessage($"Syntax: //{command} <direction> <distance>");
                return;
            }

            var inputDirection = parameters[0];
            if (!_directions.TryGetValue(inputDirection, out var direction))
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
            session.Selection = (Region)typeof(Region).GetMethod(command.ToTitleCase())
                .Invoke(session.Selection, new object[] {distance * direction});
            player.SendSuccessMessage(
                $"{command.ToTitleCase()}ed selection {inputDirection.ToLowerInvariant()} by {distance} tiles.");
        }

        private void InsetOutset(CommandArgs args)
        {
            var command = args.Message.Split(' ')[0].Substring(1).ToLowerInvariant();
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 1)
            {
                player.SendErrorMessage($"Syntax: //{command} <distance>");
                return;
            }

            var inputDistance = parameters[0];
            if (!int.TryParse(inputDistance, out var distance) || distance <= 0)
            {
                player.SendErrorMessage($"Invalid distance '{inputDistance}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            session.Selection = (Region)typeof(Region).GetMethod(command.ToTitleCase())
                .Invoke(session.Selection, new object[] {distance});
            player.SendSuccessMessage($"{command.ToTitleCase()} selection by {distance} tiles.");
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
                player.SendSuccessMessage($"Set primary position at {position}.");
                args.Handled = true;
                player.SendTileSquare(x, y, 1);
            }
            else if (action == GetDataHandlers.EditAction.PlaceWire2)
            {
                session.Selection = regionSelector.SelectSecondary(position);
                player.SendSuccessMessage($"Set secondary position at {position}.");
                args.Handled = true;
                player.SendTileSquare(x, y, 1);
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
            if (!_selectors.TryGetValue(inputSelector, out var selector))
            {
                player.SendErrorMessage($"Invalid selector '{inputSelector}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            session.RegionSelector = selector();
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
                player.GiveItemIfNot(ItemID.Wire, 999);
                player.SendInfoMessage("Use the red wrench to set the primary position.");
                player.SendInfoMessage("Use the blue wrench to set the secondary position.");
            }
        }
    }
}
