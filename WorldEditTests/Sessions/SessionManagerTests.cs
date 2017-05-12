﻿using System;
using System.Threading;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Sessions;

namespace WorldEditTests.Sessions
{
    [TestFixture]
    public class SessionManagerTests
    {
        [Test]
        public void Ctor_NullSessionCreator_ThrowsArgumentNullException()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new SessionManager(null, TimeSpan.Zero));
        }

        [Test]
        public void GetOrCreate_GetSession()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            var world = new World(extent);
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);
            var session = sessionManager.GetOrCreate("test");

            var session2 = sessionManager.GetOrCreate("test");

            Assert.AreEqual(session, session2);
        }

        [Test]
        public void GetOrCreate_NullUsername_ThrowsArgumentNullException()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            var world = new World(extent);
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);

            Assert.Throws<ArgumentNullException>(() => sessionManager.GetOrCreate(null));
        }

        [Test]
        public void GetOrCreate_StopsExpiration()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            var world = new World(extent);
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.FromSeconds(1));
            var session = sessionManager.GetOrCreate("test");
            sessionManager.StartRemoving("test");

            var session2 = sessionManager.GetOrCreate("test");

            Thread.Sleep(2000);
            var session3 = sessionManager.GetOrCreate("test");
            Assert.AreEqual(session, session2);
            Assert.AreEqual(session2, session3);
        }

        [Test]
        public void StartRemoving()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            var world = new World(extent);
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.FromSeconds(1));
            var session = sessionManager.GetOrCreate("test");
            sessionManager.StartRemoving("test");

            Thread.Sleep(2000);
            var session2 = sessionManager.GetOrCreate("test");
            Assert.AreNotEqual(session, session2);
        }

        [Test]
        public void StartRemoving_NullUsername_ThrowsArgumentNullException()
        {
            var extent = new MockTileCollection {Tiles = new ITile[20, 10]};
            var world = new World(extent);
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);

            Assert.Throws<ArgumentNullException>(() => sessionManager.StartRemoving(null));
        }
    }
}