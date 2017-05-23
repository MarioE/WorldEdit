using System.Linq;
using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.History;

namespace WorldEdit.Tests.History
{
    [TestFixture]
    public class ChangeSetTests
    {
        [Test]
        public void AddChange()
        {
            var change = Mock.Of<IChange>();
            var changeSet = new ChangeSet();

            changeSet.Add(change);

            var changes = changeSet.ToList();
            Assert.That(changes, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddChange_NullChange_ThrowsArgumentNullException()
        {
            var changeSet = new ChangeSet();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => changeSet.Add(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Redo()
        {
            var test = 0;
            var extent = Mock.Of<Extent>();
            var change = Mock.Of<IChange>();
            Mock.Get(change).Setup(c => c.Redo(extent)).Callback((Extent e) => test = 1).Returns(true);
            var change2 = Mock.Of<IChange>();
            Mock.Get(change2).Setup(c => c.Redo(extent)).Callback((Extent e) => test = 2).Returns(true);
            var changeSet = new ChangeSet {change, change2};

            Assert.That(changeSet.Redo(extent), Is.EqualTo(2));
            Assert.That(test, Is.EqualTo(2));
        }

        [Test]
        public void Redo_NullExtent_ThrowsArgumentNullException()
        {
            var changeSet = new ChangeSet();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => changeSet.Redo(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Undo()
        {
            var test = 0;
            var extent = Mock.Of<Extent>();
            var change = Mock.Of<IChange>();
            Mock.Get(change).Setup(c => c.Undo(extent)).Callback((Extent e) => test = 1).Returns(true);
            var change2 = Mock.Of<IChange>();
            Mock.Get(change2).Setup(c => c.Undo(extent)).Callback((Extent e) => test = 2).Returns(true);
            var changeSet = new ChangeSet {change, change2};

            Assert.That(changeSet.Undo(extent), Is.EqualTo(2));
            Assert.That(test, Is.EqualTo(1));
        }

        [Test]
        public void Undo_NullExtent_ThrowsArgumentNullException()
        {
            var changeSet = new ChangeSet();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => changeSet.Undo(null), Throws.ArgumentNullException);
        }
    }
}
