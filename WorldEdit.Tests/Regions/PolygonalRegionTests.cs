using System.Collections.Generic;
using NUnit.Framework;
using WorldEdit.Regions;

namespace WorldEdit.Tests.Regions
{
    [TestFixture]
    public class PolygonalRegionTests
    {
        [TestCase(0, 0, true)]
        [TestCase(0, -1, false)]
        [TestCase(5, 1, true)]
        [TestCase(5, 5, true)]
        [TestCase(5, 6, false)]
        [TestCase(10, 10, true)]
        public void Contains(int testX, int testY, bool expected)
        {
            var vertices = new List<Vector>
            {
                Vector.Zero,
                new Vector(10, 0),
                new Vector(10, 10)
            };
            var region = new PolygonalRegion(vertices);

            Assert.That(region.Contains(new Vector(testX, testY)), Is.EqualTo(expected));
        }

        [Test]
        public void Ctor_NullVertices_ThrowsArgumentNullException()
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => new PolygonalRegion(null), Throws.ArgumentNullException);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Ctor_VerticesLessThanThreeItems_ThrowsArgumentException(int count)
        {
            var vertices = new Vector[count];

            Assert.That(() => new PolygonalRegion(vertices), Throws.ArgumentException);
        }

        [Test]
        public void LowerBound()
        {
            var vertices = new List<Vector>
            {
                new Vector(0, 0),
                new Vector(3, 19),
                new Vector(-1, 52),
                new Vector(17, -2)
            };
            var region = new PolygonalRegion(vertices);

            Assert.That(region.LowerBound, Is.EqualTo(new Vector(-1, -2)));
        }

        [TestCase(1, 5)]
        public void Shift(int deltaX, int deltaY)
        {
            var vertices = new List<Vector>
            {
                new Vector(0, 0),
                new Vector(3, 19),
                new Vector(-1, 52),
                new Vector(17, -2)
            };
            var region = new PolygonalRegion(vertices);

            var region2 = (PolygonalRegion)region.Shift(new Vector(deltaX, deltaY));

            Assert.That(region2.Vertices, Has.Count.EqualTo(4));
            Assert.That(region2.Vertices[0], Is.EqualTo(new Vector(deltaX, deltaY)));
            Assert.That(region2.Vertices[1], Is.EqualTo(new Vector(3 + deltaX, 19 + deltaY)));
            Assert.That(region2.Vertices[2], Is.EqualTo(new Vector(-1 + deltaX, 52 + deltaY)));
            Assert.That(region2.Vertices[3], Is.EqualTo(new Vector(17 + deltaX, -2 + deltaY)));
        }

        [Test]
        public void UpperBound()
        {
            var vertices = new List<Vector>
            {
                new Vector(0, 0),
                new Vector(3, 19),
                new Vector(-1, 52),
                new Vector(17, -2)
            };
            var region = new PolygonalRegion(vertices);

            Assert.That(region.UpperBound, Is.EqualTo(new Vector(18, 53)));
        }

        [Test]
        public void Vertices()
        {
            var vertices = new List<Vector>
            {
                new Vector(0, 0),
                new Vector(3, 19),
                new Vector(-1, 52),
                new Vector(17, -2)
            };
            var region = new PolygonalRegion(vertices);

            Assert.That(region.Vertices, Has.Count.EqualTo(4));
            Assert.That(region.Vertices[0], Is.EqualTo(new Vector(0, 0)));
            Assert.That(region.Vertices[1], Is.EqualTo(new Vector(3, 19)));
            Assert.That(region.Vertices[2], Is.EqualTo(new Vector(-1, 52)));
            Assert.That(region.Vertices[3], Is.EqualTo(new Vector(17, -2)));
        }
    }
}
