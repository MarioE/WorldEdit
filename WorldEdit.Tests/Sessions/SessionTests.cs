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
        public void ClearHistory()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 1);
            session.CreateEditSession(true);

            session.ClearHistory();

            Assert.That(!session.CanRedo);
            Assert.That(!session.CanUndo);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CreateEditSession(bool remember)
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 1);
            session.CreateEditSession(remember);

            Assert.That(session.CanUndo, Is.EqualTo(remember));
        }

        [Test]
        public void Ctor_HistoryLimitNegative_ThrowsArgumentOutOfRangeException()
        {
            var world = Mock.Of<World>();

            Assert.That(() => new Session(world, -1), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
            var session = new Session(world, 0);
            var clipboard = new Clipboard(new Tile?[0, 0]);

            session.Clipboard = clipboard;

            Assert.That(session.Clipboard, Is.EqualTo(clipboard));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetSetIsWandMode(bool isWandMode)
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0) {IsWandMode = isWandMode};

            Assert.That(session.IsWandMode, Is.EqualTo(isWandMode));
        }

        [Test]
        public void GetSetMask()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);
            var mask = Mock.Of<Mask>();

            session.Mask = mask;

            Assert.That(session.Mask, Is.EqualTo(mask));
        }

        [Test]
        public void GetSetRegionSelector()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);
            var regionSelector = Mock.Of<RegionSelector>(r => r.GetRegion() == Mock.Of<Region>());

            session.RegionSelector = regionSelector;

            Assert.That(session.RegionSelector, Is.EqualTo(regionSelector));
            Assert.That(session.Selection, Is.InstanceOf<Region>());
        }

        [Test]
        public void GetSetSelection()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);
            var selection = Mock.Of<Region>();

            session.Selection = selection;

            Assert.That(session.Selection, Is.EqualTo(selection));
        }

        [Test]
        public void GetSetTool()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);
            var tool = Mock.Of<ITool>();

            session.Tool = tool;

            Assert.That(session.Tool, Is.EqualTo(tool));
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
            var session = new Session(world, 1);
            var editSession = session.CreateEditSession(true);
            var tile = new Tile {Wall = 1};
            editSession.SetTile(Vector.Zero, tile);
            session.Undo();

            Assert.That(session.Redo(), Is.EqualTo(1));
            Assert.That(editSession.GetTile(Vector.Zero), Is.EqualTo(tile));
        }

        [Test]
        public void Redo_CannotRedo_ThrowsInvalidOperationException()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);

            Assert.That(() => session.Redo(), Throws.InvalidOperationException);
        }

        [Test]
        public void SetMask_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => session.Mask = null, Throws.ArgumentNullException);
        }

        [Test]
        public void SetRegionSelector_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => session.RegionSelector = null, Throws.ArgumentNullException);
        }

        [Test]
        public void SetSelection_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => session.Selection = null, Throws.ArgumentNullException);
        }

        [Test]
        public void SetTool_NullValue_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => session.Tool = null, Throws.ArgumentNullException);
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
            var session = new Session(world, 1);
            var editSession = session.CreateEditSession(true);
            var tile = new Tile {Wall = 1};
            editSession.SetTile(Vector.Zero, tile);

            Assert.That(session.Undo(), Is.EqualTo(1));
            Assert.That(editSession.GetTile(Vector.Zero), Is.Not.EqualTo(tile));
        }

        [Test]
        public void Undo_CannotUndo_ThrowsInvalidOperationException()
        {
            var world = Mock.Of<World>();
            var session = new Session(world, 0);

            Assert.That(() => session.Undo(), Throws.InvalidOperationException);
        }
    }
}
