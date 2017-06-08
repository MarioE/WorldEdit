using System.Threading.Tasks;
using JetBrains.Annotations;
using TShockAPI;

namespace WorldEdit.Modules
{
    /// <summary>
    ///     Represents a module that encapsulates the history functionality.
    /// </summary>
    [UsedImplicitly]
    public sealed class HistoryModule : Module
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="HistoryModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public HistoryModule([NotNull] WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            var command = Plugin.RegisterCommand("clearhistory", ClearHistory, "worldedit.history.clear");
            command.HelpText = "Syntax: /clearhistory\n" +
                               "Clears your history.";

            command = Plugin.RegisterCommand("/redo", Redo, "worldedit.history.redo");
            command.HelpText = "Syntax: //redo\n" +
                               "Redoes your most recent action.";

            command = Plugin.RegisterCommand("/undo", Undo, "worldedit.history.undo");
            command.HelpText = "Syntax: //undo\n" +
                               "Undoes your most recent action.";
        }

        private async void ClearHistory(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);

            // This operation must be submitted, as history modifications must be synchronized.
            await Task.Run(() => session.Submit(() =>
            {
                session.ClearHistory();
                player.SendSuccessMessage("Cleared history.");
            })).SendExceptions(player);
        }

        private async void Redo(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);

            await Task.Run(() => session.Submit(() =>
            {
                if (!session.CanRedo)
                {
                    player.SendErrorMessage("Cannot redo anything.");
                    return;
                }

                var count = session.Redo();
                player.SendSuccessMessage($"Redone {count} changes.");
            })).SendExceptions(player);
        }

        private async void Undo(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);

            await Task.Run(() => session.Submit(() =>
            {
                if (!session.CanUndo)
                {
                    player.SendErrorMessage("Cannot undo anything.");
                    return;
                }

                var count = session.Undo();
                player.SendSuccessMessage($"Undone {count} changes.");
            })).SendExceptions(player);
        }
    }
}
