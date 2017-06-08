using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace WorldEdit.Sessions
{
    /// <summary>
    ///     Manages sessions, allowing them to expire automatically. This class is thread-safe.
    /// </summary>
    public sealed class SessionManager
    {
        private readonly Dictionary<string, CancellationTokenSource> _cancels =
            new Dictionary<string, CancellationTokenSource>();

        private readonly TimeSpan _gracePeriod;
        private readonly object _lock = new object();
        private readonly Func<Session> _sessionCreator;
        private readonly Dictionary<string, Session> _sessions = new Dictionary<string, Session>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="SessionManager" /> class with the specified session creator and grace
        ///     period.
        /// </summary>
        /// <param name="sessionCreator">The session creator, which must not be <c>null</c>.</param>
        /// <param name="gracePeriod">The grace period.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sessionCreator" /> is <c>null</c>.</exception>
        public SessionManager([NotNull] Func<Session> sessionCreator, TimeSpan gracePeriod)
        {
            _sessionCreator = sessionCreator ?? throw new ArgumentNullException(nameof(sessionCreator));
            _gracePeriod = gracePeriod;
        }

        /// <summary>
        ///     Gets the session associated with the specified username, or creates it if it does not exist. This will prevent the
        ///     session from being removed.
        /// </summary>
        /// <param name="username">The username, which must not be <c>null</c>.</param>
        /// <returns>The session associated with the username.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="username" /> is <c>null</c>.</exception>
        [NotNull]
        public Session GetOrCreate([NotNull] string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            lock (_lock)
            {
                if (!_sessions.TryGetValue(username, out var session))
                {
                    session = _sessionCreator();
                    _sessions[username] = session;
                }
                else if (_cancels.TryGetValue(username, out var cancel))
                {
                    cancel.Cancel();
                    cancel.Dispose();
                    _cancels.Remove(username);
                }
                return session;
            }
        }

        /// <summary>
        ///     Removes the session associated with the specified username after the grace period has expired.
        /// </summary>
        /// <param name="username">The username, which must not be <c>null</c>.</param>
        /// <returns>A task that will complete when the session is removed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="username" /> is <c>null</c>.</exception>
        public async Task RemoveAsync([NotNull] string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            CancellationTokenSource cancel;
            lock (_lock)
            {
                // Check to make sure that there even is a session associated with the username.
                if (!_sessions.ContainsKey(username))
                {
                    return;
                }

                cancel = new CancellationTokenSource();
                _cancels[username] = cancel;
            }

            try
            {
                await Task.Delay(_gracePeriod, cancel.Token);
            }
            catch (TaskCanceledException)
            {
                return;
            }

            lock (_lock)
            {
                // A cancel may have occurred after the delay task finished but before the lock was taken.
                if (cancel.IsCancellationRequested)
                {
                    return;
                }

                cancel.Dispose();
                _cancels.Remove(username);

                var session = _sessions[username];
                session.Dispose();
                _sessions.Remove(username);
            }
        }
    }
}
