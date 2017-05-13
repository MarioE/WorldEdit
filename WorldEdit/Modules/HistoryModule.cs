﻿using Terraria;
using TShockAPI;
using WorldEdit.Sessions;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the history functionality.
    /// </summary>
    public class HistoryModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        public HistoryModule(WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            var clearHistory = Plugin.RegisterCommand("clearhistory", ClearHistory, "worldedit.history.clearhistory");
            clearHistory.HelpDesc = new[]
            {
                "Syntax: /clearhistory",
                "",
                "Clears your history."
            };

            var redo = Plugin.RegisterCommand("/redo", RedoUndo, "worldedit.history.redo");
            redo.HelpDesc = new[]
            {
                "Syntax: //redo",
                "",
                "Redoes your most recent action."
            };

            var undo = Plugin.RegisterCommand("/undo", RedoUndo, "worldedit.history.undo");
            undo.HelpDesc = new[]
            {
                "Syntax: //undo",
                "",
                "Undoes your most recent action."
            };
        }

        private void ClearHistory(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            session.ClearHistory();
            player.SendSuccessMessage("Cleared history.");
        }

        private void RedoUndo(CommandArgs args)
        {
            var command = args.Message.Split(' ')[0].Substring(1).ToLowerInvariant();
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            // ReSharper disable once PossibleNullReferenceException
            if (!(bool)typeof(Session).GetProperty($"Can{command.ToTitleCase()}").GetValue(session))
            {
                player.SendErrorMessage($"Cannot {command} anything.");
                return;
            }

            var count = (int)typeof(Session).GetMethod(command.ToTitleCase()).Invoke(session, new object[0]);
            Netplay.ResetSections();
            player.SendSuccessMessage($"{command.ToTitleCase()}ne {count} changes.");
        }
    }
}
