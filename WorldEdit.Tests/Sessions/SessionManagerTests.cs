using System;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using WorldEdit.Sessions;

namespace WorldEdit.Tests.Sessions
{
    [TestFixture]
    public class SessionManagerTests
    {
        [Parallelizable]
        [Test]
        public async Task GetOrCreateSession_CancelsRemove()
        {
            var world = Mock.Of<World>();
            var sessionManager = new SessionManager(() => new Session(world, 1), TimeSpan.FromSeconds(1));
            var session1 = sessionManager.GetOrCreate("test");
            var task = sessionManager.RemoveAsync("test");

            sessionManager.GetOrCreate("test");

            await task;
            var session2 = sessionManager.GetOrCreate("test");
            Assert.That(session1, Is.EqualTo(session2));
        }

        [Test]
        public void GetOrCreateSession_NullUsername_ThrowsArgumentNullException()
        {
            var sessionManager = new SessionManager(() => null, TimeSpan.FromSeconds(1));

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => sessionManager.GetOrCreate(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GetOrCreateSession_ReturnsSameSession()
        {
            var world = Mock.Of<World>();
            var sessionManager = new SessionManager(() => new Session(world, 1), TimeSpan.FromSeconds(1));
            var session1 = sessionManager.GetOrCreate("test");

            var session2 = sessionManager.GetOrCreate("test");

            Assert.That(session1, Is.EqualTo(session2));
        }

        [Parallelizable]
        [Test]
        public async Task RemoveAsync()
        {
            var world = Mock.Of<World>();
            var sessionManager = new SessionManager(() => new Session(world, 1), TimeSpan.FromSeconds(1));
            var session1 = sessionManager.GetOrCreate("test");

            await sessionManager.RemoveAsync("test");

            var session2 = sessionManager.GetOrCreate("test");
            Assert.That(session1, Is.Not.EqualTo(session2));
        }

        [Test]
        public void RemoveAsync_NullUsername_ThrowsArgumentNullException()
        {
            var sessionManager = new SessionManager(() => null, TimeSpan.FromSeconds(1));

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(async () => await sessionManager.RemoveAsync(null), Throws.ArgumentNullException);
        }
    }
}
