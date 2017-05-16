using System;
using NUnit.Framework;
using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Masks;
using WorldEdit.Regions;
using WorldEdit.Sessions;

namespace WorldEditTests.Sessions
{
    [TestFixture]
    public class SessionTests
    {
        [Test]
        public void ClearHistory()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);
            session.CreateEditSession(true);

            session.ClearHistory();

            Assert.IsFalse(session.CanUndo);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CreateEditSession(bool remember)
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 1);

            session.CreateEditSession(remember);

            Assert.AreEqual(remember, session.CanUndo);
        }

        [Test]
        public void Ctor_NegativeHistoryLimit_ThrowsArgumentOutOfRangeException()
        {
            var world = new World(new MockTileCollection());

            Assert.Throws<ArgumentOutOfRangeException>(() => new Session(world, -1));
        }

        [Test]
        public void Ctor_NullWorld_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Session(null, 0));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetSetIsWandMode(bool value)
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);

            session.IsWandMode = value;

            Assert.AreEqual(value, session.IsWandMode);
        }

        [Test]
        public void GetSetMask()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);
            var mask = new NullMask();

            session.Mask = mask;

            Assert.AreEqual(mask, session.Mask);
        }

        [Test]
        public void GetSetRegionSelector()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);
            var regionSelector = new RegionSelector();

            session.RegionSelector = regionSelector;

            Assert.AreEqual(regionSelector, session.RegionSelector);
        }

        [Test]
        public void GetSetSelection()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);
            var selection = new NullRegion();

            session.Selection = selection;

            Assert.AreEqual(selection, session.Selection);
        }

        [Test]
        public void Redo()
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var session = new Session(world, 1);
            var editSession = session.CreateEditSession(true);
            editSession.SetTile(0, 0, new Tile {Wall = 1});
            session.Undo();

            Assert.AreEqual(1, session.Redo());
            Assert.AreEqual(1, world.GetTile(0, 0).Wall);
        }

        [Test]
        public void Redo_CannotRedo_ThrowsInvalidOperationException()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);

            Assert.Throws<InvalidOperationException>(() => session.Redo());
        }

        [Test]
        public void SetMask_NullValue_ThrowsArgumentNullException()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);

            Assert.Throws<ArgumentNullException>(() => session.Mask = null);
        }

        [Test]
        public void SetRegionSelector_NullValue_ThrowsArgumentNullException()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);

            Assert.Throws<ArgumentNullException>(() => session.RegionSelector = null);
        }

        [Test]
        public void SetSelection_NullValue_ThrowsArgumentNullException()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);

            Assert.Throws<ArgumentNullException>(() => session.Selection = null);
        }

        [Test]
        public void Undo()
        {
            var world = new World(new MockTileCollection {Tiles = new ITile[20, 10]});
            var session = new Session(world, 1);
            var editSession = session.CreateEditSession(true);
            editSession.SetTile(0, 0, new Tile {Wall = 1});

            Assert.AreEqual(1, session.Undo());
            Assert.AreNotEqual(1, world.GetTile(0, 0).Wall);
        }

        [Test]
        public void Undo_CannotUndo_ThrowsInvalidOperationException()
        {
            var world = new World(new MockTileCollection());
            var session = new Session(world, 0);

            Assert.Throws<InvalidOperationException>(() => session.Undo());
        }
    }
}
