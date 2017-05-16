using NUnit.Framework;
using WorldEdit.Schematics;

namespace WorldEditTests.Schematics
{
    [TestFixture]
    public class SchematicInfoTests
    {
        [TestCase("A")]
        public void GetSetAuthor(string author)
        {
            var schematicInfo = new SchematicInfo();

            schematicInfo.Author = author;

            Assert.AreEqual(author, schematicInfo.Author);
        }

        [TestCase("A")]
        public void GetSetDescription(string description)
        {
            var schematicInfo = new SchematicInfo();

            schematicInfo.Description = description;

            Assert.AreEqual(description, schematicInfo.Description);
        }

        [TestCase("A")]
        public void GetSetFormat(string format)
        {
            var schematicInfo = new SchematicInfo();

            schematicInfo.Format = format;

            Assert.AreEqual(format, schematicInfo.Format);
        }
    }
}
