using TShockAPI;

namespace WorldEdit.Core.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the history functionality.
    /// </summary>
    public sealed class HistoryModule : Module
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

            var redo = Plugin.RegisterCommand("/redo", Redo, "worldedit.history.redo");
            redo.HelpDesc = new[]
            {
                "Syntax: //redo",
                "",
                "Redoes your most recent action."
            };

            var undo = Plugin.RegisterCommand("/undo", Undo, "worldedit.history.undo");
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

        private void Redo(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            if (!session.CanRedo)
            {
                player.SendErrorMessage("Cannot redo anything.");
                return;
            }

            var count = session.Redo();
            player.SendSuccessMessage($"Redone {count} changes.");
        }

        private void Undo(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            if (!session.CanUndo)
            {
                player.SendErrorMessage("Cannot undo anything.");
                return;
            }

            var count = session.Undo();
            player.SendSuccessMessage($"Undone {count} changes.");
        }
    }
}
