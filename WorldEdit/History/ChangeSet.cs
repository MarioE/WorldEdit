using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using WorldEdit.Extents;

namespace WorldEdit.History
{
    /// <summary>
    ///     Specifies a set of changes that can be undone and redone collectively.
    /// </summary>
    // TODO: Optimize this
    public sealed class ChangeSet : IDisposable
    {
        private const int TileSize = 11;
        private const int TileUpdateSize = VectorSize + 2 * TileSize;
        private const int VectorSize = 8;

        private readonly List<FileStream> _files = new List<FileStream>();
        private readonly List<IChange> _nonTileUpdates = new List<IChange>();
        private readonly MemoryStream _stream;
        private readonly BinaryWriter _writer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChangeSet" /> class with the specified tile update capacity.
        /// </summary>
        /// <param name="tileUpdateCapacity">The tile update capacity, which must be positive.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="tileUpdateCapacity" /> is not positive.</exception>
        public ChangeSet(int tileUpdateCapacity = 100000)
        {
            if (tileUpdateCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tileUpdateCapacity), "Capacity must be positive.");
            }

            _stream = new MemoryStream(tileUpdateCapacity * TileUpdateSize);
            _writer = new BinaryWriter(_stream);
        }

        /// <summary>
        ///     Disposes the change set.
        /// </summary>
        public void Dispose()
        {
            foreach (var file in _files)
            {
                file.Dispose();
            }
            _stream.Dispose();
            _writer.Dispose();
        }

        /// <summary>
        ///     Adds the specified change to the change set.
        /// </summary>
        /// <param name="change">The change to add, which must not be <c>null</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="change" /> is <c>null</c>.</exception>
        /// <remarks>
        ///     Initially, tile updates will be stored in memory. If the capacity is reached, then temporary files will be used for
        ///     storing more tile updates.
        /// </remarks>
        public void Add([NotNull] IChange change)
        {
            if (change == null)
            {
                throw new ArgumentNullException(nameof(change));
            }

            if (change is TileUpdate tileUpdate)
            {
                if (_stream.Position == _stream.Capacity)
                {
                    var file = File.Create(Path.GetTempFileName(), 40960, FileOptions.DeleteOnClose);
                    file.Write(_stream.GetBuffer(), 0, _stream.Capacity);
                    _files.Add(file);
                    _stream.Position = 0;
                    _stream.SetLength(0);
                }

                _writer.Write(tileUpdate.Position);
                _writer.Write(tileUpdate.OldTile);
                _writer.Write(tileUpdate.NewTile);
            }
            else
            {
                _nonTileUpdates.Add(change);
            }
        }

        /// <summary>
        ///     Redoes the changes on the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify, which must not be <c>null</c>.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public int Redo([NotNull] Extent extent)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            var count = 0;

            void RedoTileUpdates(Stream stream)
            {
                using (var reader = new BinaryReader(stream, Encoding.Default, true))
                {
                    stream.Position = 0;
                    while (stream.Position != stream.Length)
                    {
                        var position = reader.ReadVector();
                        stream.Position += TileSize;
                        var newTile = reader.ReadTile();

                        if (extent.SetTile(position, newTile))
                        {
                            ++count;
                        }
                    }
                }
            }

            RedoTileUpdates(_stream);
            foreach (var file in Enumerable.Reverse(_files))
            {
                RedoTileUpdates(file);
            }

            count += _nonTileUpdates.Count(change => change.Redo(extent));
            return count;
        }

        /// <summary>
        ///     Undoes the changes on the specified extent.
        /// </summary>
        /// <param name="extent">The extent to modify, which must not be <c>null</c>.</param>
        /// <returns>The number of modifications.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extent" /> is <c>null</c>.</exception>
        public int Undo([NotNull] Extent extent)
        {
            if (extent == null)
            {
                throw new ArgumentNullException(nameof(extent));
            }

            var count = 0;

            void UndoTileUpdates(Stream stream)
            {
                using (var reader = new BinaryReader(stream, Encoding.Default, true))
                {
                    stream.Position = stream.Length;
                    while (stream.Position != 0)
                    {
                        stream.Position -= TileUpdateSize;

                        var position = reader.ReadVector();
                        var oldTile = reader.ReadTile();
                        if (extent.SetTile(position, oldTile))
                        {
                            ++count;
                        }

                        stream.Position = Math.Max(0, stream.Position - VectorSize - TileSize);
                    }
                }
            }

            UndoTileUpdates(_stream);
            foreach (var file in Enumerable.Reverse(_files))
            {
                UndoTileUpdates(file);
            }

            count += Enumerable.Reverse(_nonTileUpdates).Count(change => change.Redo(extent));
            return count;
        }
    }
}
