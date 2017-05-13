using Terraria;
using TShockAPI;
using WorldEdit.Templates;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates region-related functionality.
    /// </summary>
    public class RegionModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegionModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        public RegionModule(WorldEditPlugin plugin) : base(plugin)
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
            Plugin.RegisterCommand("/color", Set<Color>, "worldedit.region.color");
            Plugin.RegisterCommand("/colorwall", Set<WallColor>, "worldedit.region.colorwall");
            Plugin.RegisterCommand("/set", Set<Block>, "worldedit.region.set");
            Plugin.RegisterCommand("/setwall", Set<Wall>, "worldedit.region.setwall");
            Plugin.RegisterCommand("/shape", Set<Shape>, "worldedit.region.shape");
        }

        private void Set<T>(CommandArgs args) where T : class, ITemplate
        {
            var player = args.Player;
            if (args.Parameters.Count != 1)
            {
                var command = args.Message.Split(' ')[0].Substring(1).ToLowerInvariant();
                player.SendErrorMessage($"Syntax: //{command} <pattern>");
                return;
            }

            var inputPattern = args.Parameters[0];
            var result = Pattern<T>.Parse(inputPattern);
            if (!result.WasSuccessful)
            {
                player.SendErrorMessage(result.ErrorMessage);
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            var count = editSession.ApplyTemplate(result.Value, session.Selection);
            Netplay.ResetSections();
            player.SendSuccessMessage($"Modified {count} tiles.");
        }
    }
}
