using WorldEdit.Extents;
using WorldEdit.History;

namespace WorldEditTests.History
{
    public class MockChange : Change
    {
        public override bool Redo(Extent extent) => true;

        public override bool Undo(Extent extent) => true;
    }
}
