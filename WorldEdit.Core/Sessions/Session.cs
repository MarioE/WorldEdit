using System;
using System.Collections.Generic;
using WorldEdit.Core.Masks;
using WorldEdit.Core.Regions;
using WorldEdit.Core.Regions.Selectors;
using WorldEdit.Core.Tools;

namespace WorldEdit.Core.Sessions
{
    /// <summary>
    /// Represents a player's session.
    /// </summary>
    // TODO: consider better encapsulation
    public sealed class Session
    {
        private readonly int _historyLimit;
        private readonly World _world;
        private List<EditSession> _history = new List<EditSession>();
        private int _historyIndex;
        private Mask _mask = new NullMask();
        private RegionSelector _regionSelector = new RectangularRegionSelector();
        private Region _selection = new NullRegion();
        private ITool _tool = new NullTool();

        /// <summary>
        /// Initializes a new instance of the <see cref="Session" /> class with the specified world and history limit.
        /// </summary>
        /// <param name="world">The world to modify.</param>
        /// <param name="historyLimit">The history limit.</param>
        /// <exception cref="ArgumentNullException"><paramref name="world" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="historyLimit" /> is negative.</exception>
        public Session(World world, int historyLimit)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
            _historyLimit = historyLimit >= 0
                ? historyLimit
                : throw new ArgumentOutOfRangeException(nameof(historyLimit), "Number must be non-negative.");
        }

        /// <summary>
        /// Gets a value indicating whether an edit session can be redone.
        /// </summary>
        public bool CanRedo => _historyIndex != _history.Count;

        /// <summary>
        /// Gets a value indicating whether an edit session can be undone.
        /// </summary>
        public bool CanUndo => _historyIndex != 0;

        /// <summary>
        /// Gets or sets the clipboard.
        /// </summary>
        public Clipboard Clipboard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the session is in wand mode.
        /// </summary>
        public bool IsWandMode { get; set; }

        /// <summary>
        /// Gets or sets the limit on the number of tiles that can be set.
        /// </summary>
        public int Limit { get; set; } = -1;

        /// <summary>
        /// Gets or sets the mask.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public Mask Mask
        {
            get => _mask;
            set => _mask = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the region selector. Setting the region selector will update the selection.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public RegionSelector RegionSelector
        {
            get => _regionSelector;
            set
            {
                _regionSelector = value ?? throw new ArgumentNullException(nameof(value));
                _selection = value.GetRegion();
            }
        }

        /// <summary>
        /// Gets or sets the selection.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public Region Selection
        {
            get => _selection;
            set => _selection = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the tool.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        public ITool Tool
        {
            get => _tool;
            set => _tool = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Clears the edit session history.
        /// </summary>
        public void ClearHistory()
        {
            _history.Clear();
            _historyIndex = 0;
        }

        /// <summary>
        /// Creates a new edit session, optionally remembering its history.
        /// </summary>
        /// <param name="remember"><c>true</c> to remember the edit session; otherwise, <c>false</c>.</param>
        /// <returns>The edit session.</returns>
        public EditSession CreateEditSession(bool remember = false)
        {
            var editSession = new EditSession(_world, Limit, _mask);
            if (remember)
            {
                // Overwrite history at the index and limit it.
                _history = _history.GetRange(0, _historyIndex++);
                _history.Add(editSession);
                while (_history.Count > _historyLimit)
                {
                    _history.RemoveAt(0);
                    --_historyIndex;
                }
            }
            return editSession;
        }

        /// <summary>
        /// Redoes an edit session.
        /// </summary>
        /// <returns>The number of redone changes.</returns>
        /// <exception cref="InvalidOperationException">No edit session can be redone.</exception>
        public int Redo()
        {
            if (!CanRedo)
            {
                throw new InvalidOperationException("Cannot redo an edit session.");
            }

            return _history[_historyIndex++].Redo();
        }

        /// <summary>
        /// Undoes an edit session.
        /// </summary>
        /// <returns>The number of undone changes.</returns>
        /// <exception cref="InvalidOperationException">No edit session can be undone.</exception>
        public int Undo()
        {
            if (!CanUndo)
            {
                throw new InvalidOperationException("Cannot undo an edit session.");
            }

            return _history[--_historyIndex].Undo();
        }
    }
}
