using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TShockAPI;
using WorldEdit.Masks;
using WorldEdit.Templates;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the utility functionality.
    /// </summary>
    public sealed class UtilityModule : Module
    {
        private readonly Dictionary<string, Func<string, ITemplate>> _maskParsers =
            new Dictionary<string, Func<string, ITemplate>>(StringComparer.OrdinalIgnoreCase)
            {
                ["block"] = Pattern<Block>.TryParse,
                ["color"] = Pattern<Color>.TryParse,
                ["shape"] = Pattern<Shape>.TryParse,
                ["wall"] = Pattern<Wall>.TryParse,
                ["wallcolor"] = Pattern<WallColor>.TryParse
            };

        /// <summary>
        /// Initializes a new instance of the <see cref="UtilityModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public UtilityModule([NotNull] WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
        }

        /// <inheritdoc />
        public override void Register()
        {
            var limit = Plugin.RegisterCommand("/limit", Limit, "worldedit.utility.limit");
            limit.HelpDesc = new[]
            {
                "Syntax: //limit <limit>",
                "",
                "Limits the number of tiles that you can modify in a single operation.",
                "A negative limit is treated as no limit."
            };

            var mask = Plugin.RegisterCommand("/mask", Mask, "worldedit.utility.mask");
            mask.HelpDesc = new[]
            {
                "Syntax: //mask <mask>",
                "",
                "Sets your mask. Masks are used to restrict what tiles will be affected. Valid masks are:",
                "- #none - No restrictions.",
                "- #selection - Restricts to your current selection.",
                "- (!)<state> - Restricts to tiles with or without certain states.",
                "- <property> (!)= <pattern> - Restricts to tiles with or without certain properties.",
                "  property can be block, color, shape, wall, and wallcolor."
            };
        }

        private void Limit(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 1)
            {
                player.SendErrorMessage("Syntax: //limit <limit>");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            var inputLimit = parameters[0];
            if (!int.TryParse(inputLimit, out var limit))
            {
                player.SendErrorMessage($"Invalid limit '{inputLimit.ToLowerInvariant()}'.");
                return;
            }

            session.Limit = limit;
            player.SendSuccessMessage($"Set limit to {limit}.");
        }

        private void Mask(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count == 0 || parameters.Count == 2)
            {
                player.SendErrorMessage("Syntax: //mask <mask>");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            Mask mask;
            if (parameters.Count == 1)
            {
                var inputMask = parameters[0];
                if (inputMask.Equals("#none", StringComparison.OrdinalIgnoreCase))
                {
                    mask = new EmptyMask();
                }
                else if (inputMask.Equals("#selection", StringComparison.OrdinalIgnoreCase))
                {
                    mask = new RegionMask(session.Selection);
                }
                else
                {
                    var state = State.TryParse(inputMask);
                    if (state == null)
                    {
                        player.SendErrorMessage($"Invalid state '{inputMask}'.");
                        return;
                    }

                    mask = new TemplateMask(state);
                }
            }
            else
            {
                var inputType = parameters[0];
                if (!_maskParsers.TryGetValue(inputType, out var parser))
                {
                    player.SendErrorMessage($"Invalid mask type '{inputType}'.");
                    return;
                }

                var inputComparison = parameters[1];
                var negated = false;
                if (inputComparison.Equals("!=", StringComparison.OrdinalIgnoreCase))
                {
                    negated = true;
                }
                else if (!inputComparison.Equals("=", StringComparison.OrdinalIgnoreCase))
                {
                    player.SendErrorMessage($"Invalid mask comparison '{inputComparison}'.");
                    return;
                }

                var inputPattern = string.Join(" ", parameters.Skip(2));
                var pattern = parser(inputPattern);
                if (pattern == null)
                {
                    player.SendErrorMessage($"Invalid pattern '{inputPattern}'.");
                    return;
                }

                mask = new TemplateMask(pattern);
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
