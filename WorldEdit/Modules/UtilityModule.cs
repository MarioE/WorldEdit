using System;
using System.Collections.Generic;
using TShockAPI;
using WorldEdit.Masks;
using WorldEdit.Templates;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the utility functionality.
    /// </summary>
    public class UtilityModule : Module
    {
        private readonly Dictionary<string, Func<string, ParsingResult>> _maskParsers =
            new Dictionary<string, Func<string, ParsingResult>>(StringComparer.OrdinalIgnoreCase)
            {
                ["block"] = Pattern<Block>.Parse,
                ["color"] = Pattern<Color>.Parse,
                ["shape"] = Pattern<Shape>.Parse,
                ["wall"] = Pattern<Wall>.Parse,
                ["wallcolor"] = Pattern<WallColor>.Parse
            };

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
            // TODO: provide more detailed HelpDesc
            Plugin.RegisterCommand("/limit", Limit, "worldedit.utility.limit");
            Plugin.RegisterCommand("/mask", Mask, "worldedit.utility.mask");
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

        private void Mask(CommandArgs args)
        {
            var player = args.Player;
            if (args.Parameters.Count != 1 && args.Parameters.Count != 3)
            {
                player.SendErrorMessage("Syntax: //mask <mask>");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            Mask mask;
            if (args.Parameters.Count == 1)
            {
                var inputMask = args.Parameters[0];
                if (inputMask.Equals("#none", StringComparison.OrdinalIgnoreCase))
                {
                    mask = new NullMask();
                }
                else if (inputMask.Equals("#selection", StringComparison.OrdinalIgnoreCase))
                {
                    mask = new RegionMask(session.Selection);
                }
                else
                {
                    player.SendErrorMessage($"Invalid mask '{inputMask}'.");
                    return;
                }
            }
            else
            {
                var inputPattern = args.Parameters[2];
                var inputComparison = args.Parameters[1];
                var negated = false;
                if (inputComparison.Equals("!=", StringComparison.OrdinalIgnoreCase))
                {
                    negated = true;
                }
                else if (!inputComparison.Equals("==", StringComparison.OrdinalIgnoreCase))
                {
                    player.SendErrorMessage($"Invalid mask comparison '{inputComparison}'.");
                    return;
                }

                var inputType = args.Parameters[0];
                if (!_maskParsers.TryGetValue(inputType, out var parser))
                {
                    player.SendErrorMessage($"Invalid mask type '{inputType}'.");
                    return;
                }

                var result = parser(inputPattern);
                if (!result.WasSuccessful)
                {
                    player.SendErrorMessage(result.ErrorMessage);
                    return;
                }

                mask = new TemplateMask((ITemplate)result.Value);
                if (negated)
                {
                    mask = new NegatedMask(mask);
                }
            }

            session.Mask = mask;
            player.SendSuccessMessage("Set mask.");
        }
    }
}
