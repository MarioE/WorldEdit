using System;
using System.Threading;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Sessions;

namespace WorldEdit.Tests.Sessions
{
    [TestFixture]
    public class SessionManagerTests
    {
        [Test]
        public void Ctor_NullSessionCreator_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SessionManager(null, TimeSpan.Zero));
        }

        [Test]
        public void GetOrCreate_GetSession()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            using (var world = new World(extent))
            {
                // ReSharper disable once AccessToDisposedClosure
                var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);
                var session = sessionManager.GetOrCreate("test");

                var session2 = sessionManager.GetOrCreate("test");

                Assert.AreEqual(session, session2);
            }
        }

        [Test]
        public void GetOrCreate_NullUsername_ThrowsArgumentNullException()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            using (var world = new World(extent))
            {
                // ReSharper disable once AccessToDisposedClosure
                var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);

                Assert.Throws<ArgumentNullException>(() => sessionManager.GetOrCreate(null));
            }
        }

        [Test]
        public void GetOrCreate_StopsExpiration()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            using (var world = new World(extent))
            {
                // ReSharper disable once AccessToDisposedClosure
                var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.FromSeconds(1));
                var session = sessionManager.GetOrCreate("test");
                sessionManager.StartRemoving("test");

                var session2 = sessionManager.GetOrCreate("test");

                Thread.Sleep(2000);
                var session3 = sessionManager.GetOrCreate("test");
                Assert.AreEqual(session, session2);
                Assert.AreEqual(session2, session3);
            }
        }

        [Test]
        public void StartRemoving()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            using (var world = new World(extent))
            {
                // ReSharper disable once AccessToDisposedClosure
                var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.FromSeconds(1));
                var session = sessionManager.GetOrCreate("test");

                sessionManager.StartRemoving("test");

                Thread.Sleep(3000);
                var session2 = sessionManager.GetOrCreate("test");
                Assert.AreNotEqual(session, session2);
            }
        }

        [Test]
        public void StartRemoving_NullUsername_ThrowsArgumentNullException()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            using (var world = new World(extent))
            {
                // ReSharper disable once AccessToDisposedClosure
                var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);

                Assert.Throws<ArgumentNullException>(() => sessionManager.StartRemoving(null));
            }
        }
    }
}
