using WorldEdit.Core.Extents;
using WorldEdit.Core.History;

namespace WorldEdit.Core.Tests.History
{
    public class MockChange : Change
    {
        public override bool Redo(Extent extent) => true;

        public override bool Undo(Extent extent) => true;
    }
}
