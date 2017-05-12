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
            Plugin.RegisterCommand("/setblock", Set<Block>, "worldedit.region.setblock");
            Plugin.RegisterCommand("/setpaint", Set<Paint>, "worldedit.region.setpaint");
            Plugin.RegisterCommand("/setshape", Set<Shape>, "worldedit.region.setshape");
            Plugin.RegisterCommand("/setwall", Set<Wall>, "worldedit.region.setwall");
            Plugin.RegisterCommand("/setwire", Set<Wire>, "worldedit.region.setwire");
        }

        private void Set<T>(CommandArgs args) where T : class, ITemplate
        {
            var typeName = typeof(T).Name.ToLowerInvariant();
            var player = args.Player;
            if (args.Parameters.Count != 1)
            {
                player.SendErrorMessage($"Syntax: //set{typeName} <pattern>.");
                return;
            }

            var inputPattern = args.Parameters[1];
            var result = Pattern<T>.Parse(inputPattern);
            if (!result.WasSuccessful)
            {
                player.SendErrorMessage(result.ErrorMessage);
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            var count = editSession.SetTiles(session.Selection, result.Value);
            Netplay.ResetSections();
            player.SendSuccessMessage($"Set {count} tiles.");
        }
    }
}
