using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using WorldEdit.Regions;

namespace WorldEdit.Tests.Regions
{
    [TestFixture]
    public class RegionTests
    {
        [Test]
        public void Dimensions()
        {
            var region = Mock.Of<Region>(r => r.LowerBound == Vector.Zero && r.UpperBound == new Vector(5, 5));

            Assert.That(region.Dimensions, Is.EqualTo(new Vector(5, 5)));
        }

        [Test]
        public void GetEnumerator()
        {
            var region = Mock.Of<Region>(r => r.LowerBound == Vector.Zero && r.UpperBound == new Vector(5, 5));
            Mock.Get(region).Setup(r => r.Contains(It.IsAny<Vector>())).Returns((Vector v) => true);
            Mock.Get(region)
                .As<IEnumerable<Vector>>()
                .Setup(r => r.GetEnumerator())
                .Returns(() => region.GetEnumerator());

            Assert.That(region.ToList(), Has.Count.EqualTo(25));
        }
    }
}
