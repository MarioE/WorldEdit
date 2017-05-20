using System;
using System.Collections.Generic;
using NUnit.Framework;
using WorldEdit.Core.Regions;

namespace WorldEdit.Core.Tests.Regions
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

            Assert.AreEqual(expected, region.Contains(new Vector(testX, testY)));
        }

        [TestCase(1, 1)]
        public void Contract_ThrowsInvalidOperationException(int deltaX, int deltaY)
        {
            var region = new PolygonalRegion(new[] {Vector.Zero, Vector.Zero, Vector.Zero});

            Assert.Throws<InvalidOperationException>(() => region.Contract(new Vector(deltaX, deltaY)));
        }

        [Test]
        public void Ctor_NullVertices_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PolygonalRegion(null));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Ctor_VerticesLessThanThreeItems_ThrowsArgumentException(int count)
        {
            var vertices = new Vector[count];

            Assert.Throws<ArgumentException>(() => new PolygonalRegion(vertices));
        }

        [TestCase(1, 1)]
        public void Expand_ThrowsInvalidOperationException(int deltaX, int deltaY)
        {
            var region = new PolygonalRegion(new[] {Vector.Zero, Vector.Zero, Vector.Zero});

            Assert.Throws<InvalidOperationException>(() => region.Expand(new Vector(deltaX, deltaY)));
        }

        [Test]
        public void GetCanContract()
        {
            var region = new PolygonalRegion(new[] {Vector.Zero, Vector.Zero, Vector.Zero});

            Assert.IsFalse(region.CanContract);
        }

        [Test]
        public void GetCanExpand()
        {
            var region = new PolygonalRegion(new[] {Vector.Zero, Vector.Zero, Vector.Zero});

            Assert.IsFalse(region.CanExpand);
        }

        [Test]
        public void GetCanShift()
        {
            var region = new PolygonalRegion(new[] {Vector.Zero, Vector.Zero, Vector.Zero});

            Assert.IsTrue(region.CanShift);
        }

        [TestCase(1)]
        public void Inset_ThrowsInvalidOperationException(int delta)
        {
            var region = new PolygonalRegion(new[] {Vector.Zero, Vector.Zero, Vector.Zero});

            Assert.Throws<InvalidOperationException>(() => region.Inset(delta));
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

            Assert.AreEqual(new Vector(-1, -2), region.LowerBound);
        }

        [TestCase(1)]
        public void Outset_ThrowsInvalidOperationException(int delta)
        {
            var region = new PolygonalRegion(new[] {Vector.Zero, Vector.Zero, Vector.Zero});

            Assert.Throws<InvalidOperationException>(() => region.Outset(delta));
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

            Assert.AreEqual(4, region2.Vertices.Count);
            Assert.AreEqual(new Vector(deltaX, deltaY), region2.Vertices[0]);
            Assert.AreEqual(new Vector(3 + deltaX, 19 + deltaY), region2.Vertices[1]);
            Assert.AreEqual(new Vector(-1 + deltaX, 52 + deltaY), region2.Vertices[2]);
            Assert.AreEqual(new Vector(17 + deltaX, -2 + deltaY), region2.Vertices[3]);
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

            Assert.AreEqual(new Vector(18, 53), region.UpperBound);
        }
    }
}
