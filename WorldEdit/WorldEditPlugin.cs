using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;
using WorldEdit.Sessions;
using Module = WorldEdit.Modules.Module;

namespace WorldEdit
{
    /// <summary>
    /// Represents the WorldEdit plugin.
    /// </summary>
    [ApiVersion(2, 1)]
    public class WorldEditPlugin : TerrariaPlugin
    {
        private static readonly string ConfigPath = Path.Combine("worldedit", "config.json");

        private readonly List<Command> _commands = new List<Command>();
        private readonly List<Module> _modules = new List<Module>();
        private Config _config = new Config();
        private SessionManager _sessionManager;
        private World _world;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorldEditPlugin" /> class with the specified Main instance.
        /// </summary>
        /// <param name="game">The Main instance.</param>
        public WorldEditPlugin(Main game) : base(game)
        {
        }

        /// <summary>
        /// Gets the plugin's author.
        /// </summary>
        public override string Author => "MarioE";

        /// <summary>
        /// Gets the plugin's description.
        /// </summary>
        public override string Description => "Provides mass editing of tiles.";

        /// <summary>
        /// Gets the plugin's name.
        /// </summary>
        public override string Name => "WorldEdit";

        /// <summary>
        /// Gets the plugin's version.
        /// </summary>
        public override Version Version => new Version(1, 0);

        /// <summary>
        /// Gets the session associated with a player, or creates it if it does not exist.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <returns>The session associated with the player.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="player" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="player" /> has no username.</exception>
        public Session GetOrCreateSession(TSPlayer player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            var username = player.User?.Name ?? throw new ArgumentException("No username.", nameof(player));
            return _sessionManager.GetOrCreate(username);
        }

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void Initialize()
        {
            Directory.CreateDirectory("worldedit");
            if (File.Exists(ConfigPath))
            {
                _config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
            }

            ServerApi.Hooks.ServerLeave.Register(this, OnLeave);
            _world = new World(Main.tile);
            _sessionManager = new SessionManager(() => new Session(_world, _config.SessionHistoryLimit),
                _config.SessionGracePeriod);

            var modules = from t in Assembly.GetExecutingAssembly().GetTypes()
                          where !t.IsAbstract && typeof(Module).IsAssignableFrom(t)
                          select (Module)Activator.CreateInstance(t, this);
            foreach (var module in modules)
            {
                module.Register();
                _modules.Add(module);
            }
        }

        /// <summary>
        /// Registers and returns a command with the specified attributes. The command will automatically be deregistered.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <param name="callback">The command callback.</param>
        /// <param name="permission">The command permission.</param>
        /// <returns>The resulting command.</returns>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="name" /> or <paramref name="callback" /> is <c>null</c>.
        /// </exception>
        public Command RegisterCommand(string name, CommandDelegate callback, string permission)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var command = new Command(permission, callback, name);
            _commands.Add(command);
            Commands.ChatCommands.Add(command);
            return command;
        }

        /// <summary>
        /// Disposes the plugin.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release managed resources; otherwise, <c>false</c>.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(_config, Formatting.Indented));

                ServerApi.Hooks.ServerLeave.Deregister(this, OnLeave);
                foreach (var command in _commands)
                {
                    Commands.ChatCommands.Remove(command);
                }
                foreach (var module in _modules)
                {
                    module.Deregister();
                }
            }
            base.Dispose(disposing);
        }

        private void OnLeave(LeaveEventArgs args)
        {
            var username = TShock.Players[args.Who]?.User?.Name;
            if (username != null)
            {
                _sessionManager.StartRemoving(username);
            }
        }
    }
}
