using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Terraria;
using TShockAPI;
using WorldEdit.Schematics;

namespace WorldEdit.Modules
{
    /// <summary>
    /// Represents a module that encapsulates the clipboard functionality.
    /// </summary>
    public class ClipboardModule : Module
    {
        private const int SchematicsPerPage = 5;

        private static readonly Dictionary<string, SchematicFormat> SchematicFormats =
            new Dictionary<string, SchematicFormat>(StringComparer.OrdinalIgnoreCase)
            {
                ["tedit"] = new TeditSchematicFormat()
            };

        private static readonly string SchematicPathFormat = Path.Combine("worldedit", "schematics", "{0}.schematic");
        private static readonly string SchematicsPath = Path.Combine("worldedit", "schematics.json");

        private Dictionary<string, SchematicInfo> _schematicInfos = new Dictionary<string, SchematicInfo>();

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

            var clearClipboard = Plugin.RegisterCommand("clearclipboard",
                ClearClipboard,
                "worldedit.clipboard.clearclipboard");
            clearClipboard.HelpDesc = new[]
            {
                "Syntax: /clearclipboard",
                "",
                "Clears your clipboard."
            };

            var copy = Plugin.RegisterCommand("/copy", Copy, "worldedit.clipboard.copy");
            copy.HelpDesc = new[]
            {
                "Syntax: //copy",
                "",
                "Copies your selection to your clipboard."
            };

            var cut = Plugin.RegisterCommand("/cut", Cut, "worldedit.clipboard.cut");
            cut.HelpDesc = new[]
            {
                "Syntax: //cut",
                "",
                "Cuts your selection to your clipboard."
            };

            var paste = Plugin.RegisterCommand("/paste", Paste, "worldedit.clipboard.paste");
            paste.HelpDesc = new[]
            {
                "Syntax: //paste",
                "",
                "Pastes your clipboard to your primary position."
            };

            var schematic = Plugin.RegisterCommand("/schematic", Schematic, "worldedit.clipboard.schematic");
            schematic.HelpDesc = new[]
            {
                "Syntax: //schematic delete <name>",
                "Syntax: //schematic list [page]",
                "Syntax: //schematic load <name>",
                "Syntax: //schematic save <name> <format>",
                "",
                "Manages schematics. Currently, the only valid format is tedit."
            };
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

        private void Cut(CommandArgs args)
        {
            var player = args.Player;
            var session = Plugin.GetOrCreateSession(player);
            var editSession = session.CreateEditSession(true);
            session.Clipboard = Clipboard.CopyFrom(editSession, session.Selection);
            editSession.ClearTiles(session.Selection);
            Netplay.ResetSections();
            player.SendSuccessMessage("Cut clipboard from selection.");
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

        private void Schematic(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count < 1)
            {
                player.SendErrorMessage("Syntax: //schematic delete <name>");
                player.SendErrorMessage("Syntax: //schematic list [page]");
                player.SendErrorMessage("Syntax: //schematic load <name>");
                player.SendErrorMessage("Syntax: //schematic save <name> <format> [description]");
                return;
            }

            var subcommandName = parameters[0];
            if (subcommandName.Equals("delete", StringComparison.OrdinalIgnoreCase))
            {
                SchematicDelete(args);
            }
            else if (subcommandName.Equals("list", StringComparison.OrdinalIgnoreCase))
            {
                SchematicList(args);
            }
            else if (subcommandName.Equals("load", StringComparison.OrdinalIgnoreCase))
            {
                SchematicLoad(args);
            }
            else if (subcommandName.Equals("save", StringComparison.OrdinalIgnoreCase))
            {
                SchematicSave(args);
            }
            else
            {
                player.SendErrorMessage("Syntax: //schematic delete <name>");
                player.SendErrorMessage("Syntax: //schematic list [page]");
                player.SendErrorMessage("Syntax: //schematic load <name>");
                player.SendErrorMessage("Syntax: //schematic save <name> <format> [description]");
            }
        }

        private void SchematicDelete(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 2)
            {
                player.SendErrorMessage("Syntax: //schematic delete <name>");
                return;
            }

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
        }

        private void SchematicList(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count > 2)
            {
                player.SendErrorMessage("Syntax: //schematic list [page]");
                return;
            }

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
                var kvp = kvps[i];
                var schematicInfo = kvp.Value;
                player.SendInfoMessage($"{kvp.Key}, created by {schematicInfo.Author}: {schematicInfo.Description}");
            }
        }

        private void SchematicLoad(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count != 2)
            {
                player.SendErrorMessage("Syntax: //schematic load <name>");
                return;
            }

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

            var session = Plugin.GetOrCreateSession(player);
            var schematicPath = string.Format(SchematicPathFormat, inputName);
            using (var stream = File.OpenRead(schematicPath))
            {
                var clipboardResult = schematicFormat.Read(stream);
                if (!clipboardResult.WasSuccessful)
                {
                    player.SendErrorMessage(clipboardResult.ErrorMessage);
                    return;
                }

                session.Clipboard = clipboardResult.Value;
                player.SendSuccessMessage("Loaded schematic.");
            }
        }

        private void SchematicSave(CommandArgs args)
        {
            var parameters = args.Parameters;
            var player = args.Player;
            if (parameters.Count < 3)
            {
                player.SendErrorMessage("Syntax: //schematic save <name> <format> [description]");
                return;
            }

            var session = Plugin.GetOrCreateSession(player);
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
        }

        private class SchematicInfo
        {
            public string Author { get; set; }
            public string Description { get; set; }
            public string Format { get; set; }
        }
    }
}
