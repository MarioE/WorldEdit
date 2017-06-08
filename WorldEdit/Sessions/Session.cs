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
    ///     Holds session information.
    /// </summary>
    public sealed class Session : IDisposable
    {
        private readonly object _actionLock = new object();
        private readonly List<EditSession> _history = new List<EditSession>();
        private readonly int _historyLimit;

        private int _historyIndex;
        private Mask _mask = new EmptyMask();
        private Region _selection = new EmptyRegion();
        private RegionSelector _selector = new RectangularRegionSelector();
        private ITool _tool = new BlankTool();

        /// <summary>
        ///     Initializes a new instance of the <see cref="Session" /> class with the specified world and history limit.
        /// </summary>
        /// <param name="world">The world to modify.</param>
        /// <param name="historyLimit">The history limit.</param>
        /// <exception cref="ArgumentNullException"><paramref name="world" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="historyLimit" /> is not positive.</exception>
        public Session([NotNull] World world, int historyLimit)
        {
            World = world ?? throw new ArgumentNullException(nameof(world));
            _historyLimit = historyLimit > 0
                ? historyLimit
                : throw new ArgumentOutOfRangeException(nameof(historyLimit), "History limit must be positive.");
        }

        /// <summary>
        ///     Gets a value indicating whether an edit session can be redone.
        /// </summary>
        public bool CanRedo => _historyIndex != _history.Count;

        /// <summary>
        ///     Gets a value indicating whether an edit session can be undone.
        /// </summary>
        public bool CanUndo => _historyIndex != 0;

        /// <summary>
        ///     Gets or sets the clipboard.
        /// </summary>
        [CanBeNull]
        public Clipboard Clipboard { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating wand mode.
        /// </summary>
        public bool IsWandMode { get; set; }

        /// <summary>
        ///     Gets or sets the time of the last tool use.
        /// </summary>
        public DateTime LastToolUse { get; set; }

        /// <summary>
        ///     Gets or sets the limit on the number of tiles that can be set in a single edit session. A negative value indicates
        ///     no limit.
        /// </summary>
        public int Limit { get; set; } = -1;

        /// <summary>
        ///     Gets or sets the mask, which must not be <c>null</c>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public Mask Mask
        {
            get => _mask;
            set => _mask = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///     Gets or sets the selection, which must not be <c>null</c>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public Region Selection
        {
            get => _selection;
            set => _selection = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///     Gets or sets the region selector, which must not be <c>null</c>.
        /// </summary>
        /// <remarks>
        ///     Setting the region selector will set the region selection.
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public RegionSelector Selector
        {
            get => _selector;
            set
            {
                _selector = value ?? throw new ArgumentNullException(nameof(value));
                Selection = _selector.GetRegion();
            }
        }

        /// <summary>
        ///     Gets or sets the tool.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <c>null</c>.</exception>
        [NotNull]
        public ITool Tool
        {
            get => _tool;
            set => _tool = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///     Gets or sets the tool edit session.
        /// </summary>
        [CanBeNull]
        public EditSession ToolSession { get; set; }

        /// <summary>
        ///     Gets the world.
        /// </summary>
        public World World { get; }

        /// <summary>
        ///     Disposes the session.
        /// </summary>
        public void Dispose()
        {
            foreach (var editSession in _history)
            {
                editSession.Dispose();
            }
        }

        /// <summary>
        ///     Clears the history.
        /// </summary>
        public void ClearHistory()
        {
            foreach (var editSession in _history)
            {
                editSession.Dispose();
            }
            _historyIndex = 0;
            _history.Clear();
        }

        /// <summary>
        ///     Creates a new edit session, remembering its history.
        /// </summary>
        /// <returns>The edit session.</returns>
        [NotNull]
        public EditSession CreateEditSession()
        {
            void RemoveAndDispose(int index)
            {
                var editSession2 = _history[index];
                editSession2.Dispose();
                _history.RemoveAt(index);
            }

            while (_history.Count > _historyIndex)
            {
                RemoveAndDispose(_history.Count - 1);
            }

            var editSession = EditSession.Create(World, Limit, Mask);
            _history.Add(editSession);
            ++_historyIndex;

            while (_history.Count > _historyLimit)
            {
                RemoveAndDispose(0);
                --_historyIndex;
            }

            return editSession;
        }

        /// <summary>
        ///     Redoes an edit session.
        /// </summary>
        /// <returns>The number of redone changes.</returns>
        /// <exception cref="InvalidOperationException">No edit session can be redone.</exception>
        public int Redo()
        {
            if (!CanRedo)
            {
                throw new InvalidOperationException("No edit session can be redone.");
            }

            return _history[_historyIndex++].Redo();
        }

        /// <summary>
        ///     Submits a CPU-bound action to be processed. Only one action can be processed at any given time. This method is
        ///     thread-safe.
        /// </summary>
        /// <param name="action">The action, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is <c>null</c>.</exception>
        public void Submit([NotNull] Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            lock (_actionLock)
            {
                action();
            }
        }

        /// <summary>
        ///     Undoes an edit session.
        /// </summary>
        /// <returns>The number of undone changes.</returns>
        /// <exception cref="InvalidOperationException">No edit session can be undone.</exception>
        public int Undo()
        {
            if (!CanUndo)
            {
                throw new InvalidOperationException("No edit session can be undone.");
            }

            return _history[--_historyIndex].Undo();
        }
    }
}
