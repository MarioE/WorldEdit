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

            // TODO: provide detailed HelpDesc
            Plugin.RegisterCommand("/contract", ContractExpandShift, "worldedit.selection.contract");
            Plugin.RegisterCommand("/expand", ContractExpandShift, "worldedit.selection.expand");
            Plugin.RegisterCommand("/inset", InsetOutset, "worldedit.selection.inset");
            Plugin.RegisterCommand("/outset", InsetOutset, "worldedit.selection.outset");
            Plugin.RegisterCommand("/select", Select, "worldedit.selection.select");
            Plugin.RegisterCommand("/shift", ContractExpandShift, "worldedit.selection.shift");
            var wand = Plugin.RegisterCommand("/wand", Wand, "worldedit.selection.wand");
            wand.AllowServer = false;
        }

        private void ContractExpandShift(CommandArgs args)
        {
            var command = args.Message.Split(' ')[0].Substring(1).ToLowerInvariant();
            var player = args.Player;
            if (args.Parameters.Count != 2)
            {
                player.SendErrorMessage($"Syntax: //{command} <direction> <distance>");
                return;
            }

            var inputDirection = args.Parameters[0];
            if (!_directions.TryGetValue(inputDirection, out var direction))
            {
                player.SendErrorMessage($"Invalid direction '{inputDirection}'.");
                return;
            }

            var inputDistance = args.Parameters[1];
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
            var player = args.Player;
            if (args.Parameters.Count != 1)
            {
                player.SendErrorMessage($"Syntax: //{command} <distance>");
                return;
            }

            var inputDistance = args.Parameters[0];
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

            var position = new Vector(args.X, args.Y);
            var regionSelector = session.RegionSelector;
            if (args.Action == GetDataHandlers.EditAction.PlaceWire)
            {
                session.Selection = regionSelector.SelectPrimary(position);
                player.SendSuccessMessage($"Set primary position at {position}.");
                args.Handled = true;
                player.SendTileSquare(args.X, args.Y, 1);
            }
            else if (args.Action == GetDataHandlers.EditAction.PlaceWire2)
            {
                session.Selection = regionSelector.SelectSecondary(position);
                player.SendSuccessMessage($"Set secondary position at {position}.");
                args.Handled = true;
                player.SendTileSquare(args.X, args.Y, 1);
            }
        }

        private void Select(CommandArgs args)
        {
            var player = args.Player;
            if (args.Parameters.Count != 1)
            {
                player.SendErrorMessage("Syntax: //select <rectangular>.");
                return;
            }

            var inputSelector = args.Parameters[0];
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
