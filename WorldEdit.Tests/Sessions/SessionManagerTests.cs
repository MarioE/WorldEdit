using System;
using System.Threading;
using Moq;
using NUnit.Framework;
using WorldEdit.Sessions;

namespace WorldEdit.Tests.Sessions
{
    [TestFixture]
    public class SessionManagerTests
    {
        [Test]
        public void Ctor_NullSessionCreator_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new SessionManager(null, TimeSpan.Zero));
        }

        [Test]
        public void GetOrCreate_GetSession()
        {
            var world = Mock.Of<World>();
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);
            var session = sessionManager.GetOrCreate("test");

            var session2 = sessionManager.GetOrCreate("test");

            Assert.AreEqual(session, session2);
        }

        [Test]
        public void GetOrCreate_NullUsername_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => sessionManager.GetOrCreate(null));
        }

        [Test]
        public void GetOrCreate_StopsExpiration()
        {
            var world = Mock.Of<World>();
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
            var world = Mock.Of<World>();
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.FromSeconds(1));
            var session = sessionManager.GetOrCreate("test");

            sessionManager.StartRemoving("test");

            Thread.Sleep(3000);
            var session2 = sessionManager.GetOrCreate("test");
            Assert.AreNotEqual(session, session2);
        }


        [Test]
        public void StartRemoving_NullUsername_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            var sessionManager = new SessionManager(() => new Session(world, 0), TimeSpan.Zero);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => sessionManager.StartRemoving(null));
        }
    }
}
