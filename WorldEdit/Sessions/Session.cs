using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;
using WorldEdit.Tools;

namespace WorldEdit.Sessions
{
    /// <summary>
    /// Holds session information.
    /// </summary>
    public sealed class Session
    {
        private readonly int _historyLimit;
        private readonly World _world;
        private List<EditSession> _history = new List<EditSession>();
        private int _historyIndex;
        private Mask _mask = new EmptyMask();
        private RegionSelector _regionSelector = new RectangularRegionSelector();
        private Region _selection = new EmptyRegion();
        private ITool _tool = new BlankTool();

        /// <summary>
        /// Initializes a new instance of the <see cref="Session" /> class with the specified world and history limit.
        /// </summary>
        /// <param name="world">The world to modify.</param>
        /// <param name="historyLimit">The history limit.</param>
        /// <exception cref="ArgumentNullException"><paramref name="world" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="historyLimit" /> is negative.</exception>
        public Session([NotNull] World world, int historyLimit)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
            _historyLimit = historyLimit >= 0
                ? historyLimit
                : throw new ArgumentOutOfRangeException(nameof(historyLimit), "Number must be non-negative.");
        }

        /// <summary>
        /// Gets a value indicating whether an edit session can be redone for this <see cref="Session" /> instance.
        /// </summary>
        public bool CanRedo => _historyIndex != _history.Count;

        /// <summary>
        /// Gets a value indicating whether an edit session can be undone for this <see cref="Session" /> instance.
        /// </summary>
        public bool CanUndo => _historyIndex != 0;

        /// <summary>
        /// Gets or sets the clipboard for this <see cref="Session" /> instance, where <c>null</c> means no clipboard.
        /// </summary>
        [CanBeNull]
        public Clipboard Clipboard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating wand mode for this <see cref="Session" /> instance.
        /// </summary>
        public bool IsWandMode { get; set; }

        /// <summary>
        /// Gets or sets the limit on the number of tiles that can be set for this <see cref="Session" /> instance.
        /// </summary>
        public int Limit { get; set; } = -1;

        /// <summary>
        /// Gets or sets the mask for this <see cref="Session" /> instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public Mask Mask
        {
            get => _mask;
            set => _mask = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the region selector for this <see cref="Session" /> instance. Setting the region selector will update the
        /// selection.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
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
        /// Gets or sets the selection for this <see cref="Session" /> instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public Region Selection
        {
            get => _selection;
            set => _selection = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Gets or sets the tool for this <see cref="Session" /> instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public ITool Tool
        {
            get => _tool;
            set => _tool = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Clears the history for this <see cref="Session" /> instance.
        /// </summary>
        public void ClearHistory()
        {
            _history.Clear();
            _historyIndex = 0;
        }

        /// <summary>
        /// Creates a new edit session from this <see cref="Session" /> instance, optionally remembering its history.
        /// </summary>
        /// <param name="remember"><c>true</c> to remember the edit session; otherwise, <c>false</c>.</param>
        /// <returns>The edit session.</returns>
        [NotNull]
        public EditSession CreateEditSession(bool remember = false)
        {
            var editSession = EditSession.Create(_world, Limit, _mask);
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
                throw new InvalidOperationException("No edit sessions to redo.");
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
                throw new InvalidOperationException("No edit sessions to undo.");
            }

            return _history[--_historyIndex].Undo();
        }
    }
}
