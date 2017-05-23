using Moq;
using NUnit.Framework;
using WorldEdit.Extents;
using WorldEdit.Tools;

namespace WorldEdit.Tests.Tools
{
    [TestFixture]
    public class BlankToolTests
    {
        [Test]
        public void Apply()
        {
            var extent = Mock.Of<Extent>();
            var tool = new BlankTool();

            Assert.That(tool.Apply(extent, Vector.Zero), Is.Zero);
        }
    }
}
