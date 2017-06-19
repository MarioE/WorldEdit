using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TShockAPI;
using WorldEdit.Schematics;

namespace WorldEdit.Modules
{
    /// <summary>
    ///     Represents a module that encapsulates the schematic functionality.
    /// </summary>
    [UsedImplicitly]
    public sealed class SchematicModule : Module
    {
        private const int SchematicsPerPage = 5;

        private static readonly Dictionary<string, SchematicFormat> SchematicFormats =
            new Dictionary<string, SchematicFormat>(StringComparer.OrdinalIgnoreCase)
            {
                ["default"] = new DefaultSchematicFormat(),
                ["tedit"] = new TeditSchematicFormat()
            };

        private static readonly string SchematicPathFormat = Path.Combine("worldedit", "schematics", "{0}.schematic");
        private static readonly string SchematicsPath = Path.Combine("worldedit", "schematics.json");

        private Dictionary<string, SchematicInfo> _schematicInfos = new Dictionary<string, SchematicInfo>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="SchematicModule" /> class with the specified WorldEdit plugin.
        /// </summary>
        /// <param name="plugin">The WorldEdit plugin, which must not be <c>null</c>.</param>
        public SchematicModule([NotNull] WorldEditPlugin plugin) : base(plugin)
        {
        }

        /// <inheritdoc />
        public override void Deregister()
        {
            File.WriteAllText(SchematicsPath, JsonConvert.SerializeObject(_schematicInfos, Formatting.Indented));
        }

        /// <inheritdoc />
        public override void Register()
        {
            Directory.CreateDirectory(Path.Combine("worldedit", "schematics"));
            if (File.Exists(SchematicsPath))
            {
                _schematicInfos =
                    JsonConvert.DeserializeObject<Dictionary<string, SchematicInfo>>(File.ReadAllText(SchematicsPath));
            }

            var command = Plugin.RegisterCommand("/schematic", Schematic, "worldedit.clipboard.schematic");
            command.HelpText = "Syntax: //schematic delete <name>\n" +
                               "Syntax: //schematic list [page]\n" +
                               "Syntax: //schematic load <name>\n" +
                               "Syntax: //schematic save <name> <format> [description]\n" +
                               "Manages schematics. Valid formats are:\n" +
                               "- default: The default schematic format. Fast and compatible with all features.\n" +
                               "- tedit: The TEdit schematic format. Slower, but must be used for compatibility with TEdit.";
            command.Names.Add("/schem");
        }

        private async void Schematic(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            var subcommandName = parameters.Count > 0 ? parameters[0] : "";
            if (subcommandName.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                await SchematicDelete(args).SendExceptions(player);
            }
            else if (subcommandName.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                await SchematicList(args).SendExceptions(player);
            }
            else if (subcommandName.Equals("load", StringComparison.OrdinalIgnoreCase))
            {
                await SchematicLoad(args).SendExceptions(player);
            }
            else if (subcommandName.Equals("save", StringComparison.OrdinalIgnoreCase))
            {
                await SchematicSave(args).SendExceptions(player);
            }
            else
            {
                player.SendErrorMessage("Syntax: //schematic delete <name>");
                player.SendErrorMessage("Syntax: //schematic list [page]");
                player.SendErrorMessage("Syntax: //schematic load <name>");
                player.SendErrorMessage("Syntax: //schematic save <name> <format> [description]");
            }
        }

        private async Task SchematicDelete(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 2)
            {
                player.SendErrorMessage("Syntax: //schematic delete <name>");
                return;
            }
            var session = Plugin.GetOrCreateSession(player);
            await Task.Run(() => session.Submit(() =>
            {
                var inputName = parameters[1];
                if (!_schematicInfos.ContainsKey(inputName))
                {
                    player.SendErrorMessage($"Invalid schematic '{inputName}'.");
                    return;
                }

                _schematicInfos.Remove(inputName);
                var schematicPath = string.Format(SchematicPathFormat, inputName);
                File.Delete(schematicPath);
                player.SendSuccessMessage("Deleted schematic.");
            }));
        }

        private async Task SchematicList(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count > 2)
            {
                player.SendErrorMessage("Syntax: //schematic list [page]");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            await Task.Run(() => session.Submit(() =>
            {
                var maxPage = (_schematicInfos.Count - 1) / SchematicsPerPage + 1;
                var inputPageNumber = parameters.Count == 1 ? "1" : parameters[1];
                if (!int.TryParse(inputPageNumber, out var pageNumber) || pageNumber <= 0 || pageNumber > maxPage)
                {
                    player.SendErrorMessage($"Invalid page number '{inputPageNumber}'.");
                    return;
                }

                if (_schematicInfos.Count == 0)
                {
                    player.SendErrorMessage("No schematics found.");
                    return;
                }

                player.SendSuccessMessage($"Schematics (page {pageNumber} out of {maxPage}):");
                var kvps = _schematicInfos.ToList();
                var offset = (pageNumber - 1) * SchematicsPerPage;
                for (var i = 0; i < 5 && i < _schematicInfos.Count - offset; ++i)
                {
                    var kvp = kvps[i + offset];
                    var schematicInfo = kvp.Value;
                    player.SendInfoMessage(
                        $"{kvp.Key}, created by {schematicInfo.Author}: {schematicInfo.Description}");
                }
            }));
        }

        private async Task SchematicLoad(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 2)
            {
                player.SendErrorMessage("Syntax: //schematic load <name>");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            await Task.Run(() => session.Submit(() =>
            {
                var inputName = parameters[1];
                if (!_schematicInfos.TryGetValue(inputName, out var schematicInfo))
                {
                    player.SendErrorMessage($"Invalid schematic '{inputName}'.");
                    return;
                }

                if (!SchematicFormats.TryGetValue(schematicInfo.Format, out var schematicFormat))
                {
                    player.SendErrorMessage("Schematic is malformed.");
                    return;
                }

                var schematicPath = string.Format(SchematicPathFormat, inputName);
                using (var stream = File.OpenRead(schematicPath))
                {
                    var clipboard = schematicFormat.Read(stream);
                    if (clipboard == null)
                    {
                        player.SendErrorMessage("Schematic is malformed.");
                        return;
                    }

                    session.Clipboard = clipboard;
                    player.SendSuccessMessage("Loaded schematic.");
                }
            }));
        }

        private async Task SchematicSave(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count < 3)
            {
                player.SendErrorMessage("Syntax: //schematic save <name> <format> [description]");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
            await Task.Run(() => session.Submit(() =>
            {
                var clipboard = session.Clipboard;
                if (clipboard == null)
                {
                    player.SendErrorMessage("Invalid clipboard.");
                    return;
                }

                var inputName = parameters[1];
                if (inputName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                {
                    player.SendErrorMessage($"Invalid name '{inputName}'.");
                    return;
                }
                if (_schematicInfos.ContainsKey(inputName))
                {
                    player.SendErrorMessage("A schematic with that name already exists.");
                    return;
                }

                var inputSchematicFormat = parameters[2];
                if (!SchematicFormats.TryGetValue(inputSchematicFormat, out var schematicFormat))
                {
                    player.SendErrorMessage($"Invalid schematic format '{inputSchematicFormat}'.");
                    return;
                }

                var inputDescription = string.Join(" ", parameters.Skip(3));
                if (string.IsNullOrWhiteSpace(inputDescription))
                {
                    inputDescription = "N/A";
                }

                var schematicPath = string.Format(SchematicPathFormat, inputName);
                using (var stream = File.OpenWrite(schematicPath))
                {
                    schematicFormat.Write(clipboard, stream);
                }
                _schematicInfos[inputName] = new SchematicInfo
                {
                    Author = player.Name,
                    Description = inputDescription,
                    Format = inputSchematicFormat.ToLowerInvariant()
                };

                player.SendSuccessMessage("Saved schematic.");
            }));
        }

        private class SchematicInfo
        {
            public string Author { get; set; }
            public string Description { get; set; }
            public string Format { get; set; }
        }
    }
}
