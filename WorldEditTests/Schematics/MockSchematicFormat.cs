using System.IO;
using WorldEdit;
using WorldEdit.Schematics;

namespace WorldEditTests.Schematics
{
    public class MockSchematicFormat : SchematicFormat
    {
        protected override Result<Clipboard> ReadImpl(Stream stream) => null;

        protected override void WriteImpl(Clipboard clipboard, Stream stream)
        {
        }
    }
}
