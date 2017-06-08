using System.Threading.Tasks;
using JetBrains.Annotations;
using TShockAPI;

namespace WorldEdit.Modules
{
    /// <summary>
    ///     Represents a module that encapsulates the clipboard functionality.
    /// </summary>
    public sealed class ClipboardModule : Module
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClipboardModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public ClipboardModule([NotNull] WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            var command = Plugin.RegisterCommand("clearclipboard", ClearClipboard, "worldedit.clipboard.clear");
            command.HelpText = "Syntax: /clearclipboard\n" +
                               "Clears your clipboard.";

            command = Plugin.RegisterCommand("/copy", Copy, "worldedit.clipboard.copy");
            command.HelpText = "Syntax: //copy\n" +
                               "Copies your selection to your clipboard.";

            command = Plugin.RegisterCommand("/cut", Cut, "worldedit.clipboard.cut");
            command.HelpText = "Syntax: //cut\n" +
                               "Cuts your selection to your clipboard.";

            command = Plugin.RegisterCommand("/paste", Paste, "worldedit.clipboard.paste");
            command.HelpText = "Syntax: //paste\n" +
                               "Pastes your clipboard to your primary position.";
        }

        private async void ClearClipboard(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);

            // This operation must be submitted, as clipboard modifications must be synchronized.
            await Task.Run(() => session.Submit(() =>
            {
                session.Clipboard = null;
                player.SendSuccessMessage("Cleared clipboard.");
            })).SendExceptions(player);
        }

        private async void Copy(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var region = session.Selection;
            await Task.Run(() => session.Submit(() =>
            {
                session.Clipboard = Clipboard.CopyFrom(session.World, region);
                player.SendSuccessMessage("Copied clipboard from selection.");
            })).SendExceptions(player);
        }

        private async void Cut(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession();
            var region = session.Selection;
            await Task.Run(() => session.Submit(() =>
            {
                session.Clipboard = Clipboard.CopyFrom(editSession, region);
                editSession.Clear(region);
                player.SendSuccessMessage("Copied clipboard from selection.");
            })).SendExceptions(player);
        }

        private async void Paste(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var position = session.Selector.PrimaryPosition;
            if (position == null)
            {
                player.SendErrorMessage("Invalid primary position.");
                return;
            }

            var editSession = session.CreateEditSession();
            await Task.Run(() => session.Submit(() =>
            {
                var clipboard = session.Clipboard;
                if (clipboard == null)
                {
                    player.SendErrorMessage("Invalid clipboard.");
                    return;
                }

                clipboard.PasteTo(editSession, position.Value);
                player.SendSuccessMessage("Pasted clipboard to primary position.");
            })).SendExceptions(player);
        }
    }
}
