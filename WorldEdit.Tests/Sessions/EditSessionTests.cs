using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Masks;
using WorldEdit.Sessions;

namespace WorldEdit.Tests.Sessions
{
    [TestFixture]
    public class EditSessionTests
    {
        [Test]
        public void Create_NullMask_ThrowsArgumentNullException()
        {
            var world = Mock.Of<World>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => EditSession.Create(world, -1, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Create_NullWorld_ThrowsArgumentNullException()
        {
            var mask = Mock.Of<Mask>();

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => EditSession.Create(null, -1, mask), Throws.ArgumentNullException);
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
            var mask = Mock.Of<Mask>(m => m.Test(It.IsAny<Extent>(), It.IsAny<Vector>()));
            using (var editSession = EditSession.Create(world, -1, mask))
            {
                var tile = new Tile {WallId = 1};
                editSession.SetTile(Vector.Zero, tile);
                editSession.Undo();

                Assert.That(editSession.Redo(), Is.EqualTo(1));
                Assert.That(editSession.GetTile(Vector.Zero), Is.EqualTo(tile));
            }
        }

        [TestCase(3, 4)]
        public void SetTile_LimitObeyed(int x, int y)
        {
            var tileAtXy = new Tile();
            var position = new Vector(x, y);
            var world = Mock.Of<World>();
            Mock.Get(world).Setup(w => w.GetTile(position)).Returns((Vector v) => tileAtXy);
            Mock.Get(world)
                .Setup(w => w.SetTile(position, It.IsAny<Tile>()))
                .Callback((Vector v, Tile t) => tileAtXy = t)
                .Returns(true);
            var mask = Mock.Of<Mask>(m => m.Test(It.IsAny<Extent>(), It.IsAny<Vector>()));
            using (var editSession = EditSession.Create(world, 0, mask))
            {
                var tile = new Tile {WallId = 1};

                Assert.That(!editSession.SetTile(position, tile));
                Assert.That(editSession.GetTile(position) != tile);
            }
        }

        [TestCase(3, 4)]
        public void SetTile_MaskObeyed(int x, int y)
        {
            var tileAtXy = new Tile();
            var position = new Vector(x, y);
            var world = Mock.Of<World>();
            Mock.Get(world).Setup(w => w.GetTile(position)).Returns((Vector v) => tileAtXy);
            Mock.Get(world)
                .Setup(w => w.SetTile(position, It.IsAny<Tile>()))
                .Callback((Vector v, Tile t) => tileAtXy = t)
                .Returns(true);
            var mask = Mock.Of<Mask>(m => !m.Test(It.IsAny<Extent>(), It.IsAny<Vector>()));
            using (var editSession = EditSession.Create(world, -1, mask))
            {
                var tile = new Tile {WallId = 1};

                Assert.That(!editSession.SetTile(position, tile));
                Assert.That(editSession.GetTile(position) != tile);
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
            var tile = new Tile {WallId = 1};
            var mask = Mock.Of<Mask>(m => m.Test(It.IsAny<Extent>(), It.IsAny<Vector>()));
            using (var editSession = EditSession.Create(world, -1, mask))
            {
                editSession.SetTile(Vector.Zero, tile);

                Assert.That(editSession.Undo(), Is.EqualTo(1));
                Assert.That(editSession.GetTile(Vector.Zero), Is.Not.EqualTo(tile));
            }
        }
    }
}
