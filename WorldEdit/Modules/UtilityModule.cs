using TShockAPI;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the utility functionality.
    /// </summary>
    public class UtilityModule : Module
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UtilityModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin.</param>
        public UtilityModule(WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            Plugin.RegisterCommand("/limit", Limit, "worldedit.utility.limit");
        }

        private void Limit(CommandArgs args)
        {
            var player = args.Player;
            if (args.Parameters.Count != 1)
            {
                player.SendErrorMessage("Syntax: //limit <limit>");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var inputLimit = args.Parameters[0].ToLowerInvariant();
            if (!int.TryParse(inputLimit, out int limit))
            {
                player.SendErrorMessage($"Invalid limit '{inputLimit}'.");
                return;
            }

            session.Limit = limit;
            player.SendSuccessMessage($"Set limit to {limit}.");
        }
    }
}
