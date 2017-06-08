using System;
using Moq;
using NUnit.Framework;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Regions.Selectors;
using WorldEdit.Sessions;
using WorldEdit.Tools;

namespace WorldEdit.Tests.Sessions
{
    [TestFixture]
    public class SessionTests
    {
        [Test]
        public void CanRedo()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                session.CreateEditSession();

                session.Undo();

                Assert.That(session.CanRedo);
            }
        }

        [Test]
        public void ClearHistory()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                session.CreateEditSession();

                session.ClearHistory();

                Assert.That(!session.CanRedo);
                Assert.That(!session.CanUndo);
            }
        }

        [Test]
        public void CreateEditSession_KeepsLimit()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 3))
            {
                session.CreateEditSession();
                session.CreateEditSession();
                session.CreateEditSession();
                session.CreateEditSession();
                session.Undo();
                session.Undo();
                session.Undo();

                Assert.That(!session.CanUndo);
            }
        }

        [Test]
        public void CreateEditSession_OverwritesUndone()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 5))
            {
                session.CreateEditSession();
                session.CreateEditSession();
                session.Undo();

                session.CreateEditSession();

                Assert.That(!session.CanRedo);
            }
        }

        [Test]
        public void CreateEditSessionCanUndo()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                session.CreateEditSession();

                Assert.That(session.CanUndo);
            }
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void Ctor_NonPositiveHistoryLimit_ThrowsArgumentOutOfRangeException(int historyLimit)
        {
            var world = Mock.Of<World>();

            Assert.That(() => new Session(world, historyLimit), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Ctor_NullWorld_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new Session(null, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void GetSetClipboard()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var clipboard = new Clipboard(new Tile?[0, 0]);

                session.Clipboard = clipboard;

                Assert.That(session.Clipboard, Is.EqualTo(clipboard));
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetSetIsWandMode(bool isWandMode)
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                session.IsWandMode = isWandMode;

                Assert.That(session.IsWandMode, Is.EqualTo(isWandMode));
            }
        }

        [Test]
        public void GetSetLastToolUse()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var lastToolUse = DateTime.UtcNow;

                session.LastToolUse = lastToolUse;

                Assert.That(session.LastToolUse, Is.EqualTo(lastToolUse));
            }
        }

        [TestCase(37)]
        public void GetSetLimit(int limit)
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                session.Limit = limit;

                Assert.That(session.Limit, Is.EqualTo(limit));
            }
        }

        [Test]
        public void GetSetMask()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var mask = Mock.Of<Mask>();

                session.Mask = mask;

                Assert.That(session.Mask, Is.EqualTo(mask));
            }
        }

        [Test]
        public void GetSetSelection()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var selection = Mock.Of<Region>();

                session.Selection = selection;

                Assert.That(session.Selection, Is.EqualTo(selection));
            }
        }

        [Test]
        public void GetSetSelector()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var selection = Mock.Of<Region>();
                var selector = Mock.Of<RegionSelector>(r => r.GetRegion() == selection);

                session.Selector = selector;

                Assert.That(session.Selector, Is.EqualTo(selector));
                Assert.That(session.Selection, Is.EqualTo(selection));
            }
        }

        [Test]
        public void GetSetTool()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var tool = Mock.Of<ITool>();

                session.Tool = tool;

                Assert.That(session.Tool, Is.EqualTo(tool));
            }
        }

        [Test]
        public void GetSetToolSession()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var editSession = session.CreateEditSession();

                session.ToolSession = editSession;

                Assert.That(session.ToolSession, Is.EqualTo(editSession));
            }
        }

        [Test]
        public void Redo()
        {
            var tileAtZeroZero = new Tile();
            var world = Mock.Of<World>();
            Mock.Get(world)
                .Setup(w => w.SetTile(Vector.Zero, It.IsAny<Tile>()))
                .Callback((Vector v, Tile t) => tileAtZeroZero = t)
                .Returns(true);
            Mock.Get(world)
                .Setup(w => w.GetTile(Vector.Zero))
                .Returns((Vector v) => tileAtZeroZero);
            using (var session = new Session(world, 1))
            {
                var editSession = session.CreateEditSession();
                var tile = new Tile {WallId = 1};
                editSession.SetTile(Vector.Zero, tile);
                session.Undo();

                Assert.That(session.Redo(), Is.EqualTo(1));
                Assert.That(editSession.GetTile(Vector.Zero), Is.EqualTo(tile));
            }
        }

        [Test]
        public void Redo_CannotRedo_ThrowsInvalidOperationException()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                Assert.That(() => session.Redo(), Throws.InvalidOperationException);
            }
        }

        [Test]
        public void SetMask_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => session.Mask = null, Throws.ArgumentNullException);
            }
        }

        [Test]
        public void SetSelection_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => session.Selection = null, Throws.ArgumentNullException);
            }
        }

        [Test]
        public void SetSelector_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => session.Selector = null, Throws.ArgumentNullException);
            }
        }

        [Test]
        public void SetTool_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => session.Tool = null, Throws.ArgumentNullException);
            }
        }

        [Test]
        public void Submit()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                var x = 0;

                session.Submit(() => x = 1);

                Assert.That(x, Is.EqualTo(1));
            }
        }

        [Test]
        public void Submit_NullAction_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Assert.That(() => session.Submit(null), Throws.ArgumentNullException);
            }
        }

        [Test]
        public void Undo()
        {
            var tileAtZeroZero = new Tile();
            var world = Mock.Of<World>();
            Mock.Get(world)
                .Setup(w => w.SetTile(Vector.Zero, It.IsAny<Tile>()))
                .Callback((Vector v, Tile t) => tileAtZeroZero = t)
                .Returns(true);
            Mock.Get(world)
                .Setup(w => w.GetTile(Vector.Zero))
                .Returns((Vector v) => tileAtZeroZero);
            using (var session = new Session(world, 1))
            {
                var editSession = session.CreateEditSession();
                var tile = new Tile {WallId = 1};
                editSession.SetTile(Vector.Zero, tile);

                Assert.That(session.Undo(), Is.EqualTo(1));
                Assert.That(editSession.GetTile(Vector.Zero), Is.Not.EqualTo(tile));
            }
        }

        [Test]
        public void Undo_CannotUndo_ThrowsInvalidOperationException()
        {
            var world = Mock.Of<World>();
            using (var session = new Session(world, 1))
            {
                Assert.That(() => session.Undo(), Throws.InvalidOperationException);
            }
        }
    }
}
