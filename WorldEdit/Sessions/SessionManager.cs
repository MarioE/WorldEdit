using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace WorldEdit.Sessions
{
    /// <summary>
    /// Manages sessions, allowing them to expire automatically.
    /// </summary>
    public class SessionManager : IDisposable
    {
        private readonly TimeSpan _gracePeriod;
        private readonly object _lock = new object();
        private readonly Func<Session> _sessionCreator;
        private readonly Dictionary<string, SessionHolder> _sessionHolders = new Dictionary<string, SessionHolder>();
        private readonly Timer _timer = new Timer(1000);

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionManager" /> class with the specified session creator and grace
        /// period.
        /// </summary>
        /// <param name="sessionCreator">The session creator.</param>
        /// <param name="gracePeriod">The grace period.</param>
        public SessionManager(Func<Session> sessionCreator, TimeSpan gracePeriod)
        {
            _sessionCreator = sessionCreator ?? throw new ArgumentNullException(nameof(sessionCreator));
            _gracePeriod = gracePeriod;
            _timer.Elapsed += (sender, args) => Flush();
            _timer.Start();
        }

        /// <summary>
        /// Disposes the session manager.
        /// </summary>
        public void Dispose()
        {
            _timer.Dispose();
        }

        /// <summary>
        /// Flushes all expired sessions.
        /// </summary>
        public void Flush()
        {
            lock (_lock)
            {
                var expiredUsernames = from s in _sessionHolders
                                       let v = s.Value
                                       where v.IsExpiring && DateTime.UtcNow > v.ExpirationTime
                                       select s.Key;
                foreach (var username in expiredUsernames.ToList())
                {
                    _sessionHolders.Remove(username);
                }
            }
        }

        /// <summary>
        /// Gets the session associated with the specified username, or creates it if it does not exist. This stops the expiration
        /// countdown.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>The session associated with the username.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="username" /> is <c>null</c>.</exception>
        public Session GetOrCreate(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            lock (_lock)
            {
                if (_sessionHolders.TryGetValue(username, out var sessionHolder))
                {
                    sessionHolder.IsExpiring = false;
                    return sessionHolder.Session;
                }

                var session = _sessionCreator();
                _sessionHolders[username] = new SessionHolder {Session = session};
                return session;
            }
        }

        /// <summary>
        /// Starts removing the session associated with the specified username by starting the expiration countdown.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <exception cref="ArgumentNullException"><paramref name="username" /> is <c>null</c>.</exception>
        public void StartRemoving(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            lock (_lock)
            {
                if (_sessionHolders.TryGetValue(username, out var sessionHolder))
                {
                    sessionHolder.IsExpiring = true;
                    sessionHolder.ExpirationTime = DateTime.UtcNow + _gracePeriod;
                }
            }
        }

        private class SessionHolder
        {
            public DateTime ExpirationTime;
            public bool IsExpiring;
            public Session Session;
        }
    }
}
