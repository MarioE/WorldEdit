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

        private static bool TryParseTemplate<T>(string s, out T t) where T : ITemplate
        {
            var parameters = new object[] {s, null};
            var result = (bool)typeof(T).GetMethod("TryParse").Invoke(null, parameters);
            t = (T)parameters[1];
            return result;
        }

        private void Set<T>(CommandArgs args) where T : ITemplate
        {
            var templateType = typeof(T).Name.ToLowerInvariant();
            var player = args.Player;
            if (args.Parameters.Count != 1)
            {
                player.SendErrorMessage($"Syntax: //set{templateType} <{templateType}>.");
                return;
            }

            var templateName = args.Parameters[0];
            if (!TryParseTemplate<T>(templateName, out var t))
            {
                player.SendErrorMessage($"Invalid {templateType} '{templateName}'.");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            var count = editSession.SetTiles(session.Selection, t);
            Netplay.ResetSections();
            player.SendSuccessMessage($"Set {count} tiles.");
        }
    }
}
