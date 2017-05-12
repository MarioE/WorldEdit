using Terraria;
using TShockAPI;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the clipboard functionality.
    /// </summary>
    public class ClipboardModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClipboardModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        public ClipboardModule(WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            // TODO: provide more detailed HelpDesc
            Plugin.RegisterCommand("clearclipboard", ClearClipboard, "worldedit.clipboard.clearclipboard");
            Plugin.RegisterCommand("/copy", Copy, "worldedit.clipboard.copy");
            Plugin.RegisterCommand("/paste", Paste, "worldedit.clipboard.paste");
        }

        private void ClearClipboard(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            session.Clipboard = null;
            player.SendSuccessMessage("Cleared clipboard.");
        }

        private void Copy(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession();
            session.Clipboard = Clipboard.CopyFrom(editSession, session.Selection);
            player.SendSuccessMessage("Copied clipboard from selection.");
        }

        private void Paste(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var position = session.RegionSelector.PrimaryPosition;
            if (position == null)
            {
                player.SendErrorMessage("Invalid primary position.");
                return;
            }

            var clipboard = session.Clipboard;
            var editSession = session.CreateEditSession(true);
            clipboard?.PasteTo(editSession, position.Value);
            Netplay.ResetSections();
            player.SendSuccessMessage("Pasted clipboard to primary position.");
        }
    }
}
