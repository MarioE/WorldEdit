using System;
using NUnit.Framework;

namespace WorldEditTests.History
{
    [TestFixture]
    public class ChangeTests
    {
        [Test]
        public void Redo_NullExtent_ThrowsArgumentNullException()
        {
            var change = new MockChange();

            Assert.Throws<ArgumentNullException>(() => change.Redo(null));
        }

        [Test]
        public void Undo_NullExtent_ThrowsArgumentNullException()
        {
            var change = new MockChange();

            Assert.Throws<ArgumentNullException>(() => change.Undo(null));
        }
    }
}
