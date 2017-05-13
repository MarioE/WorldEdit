using WorldEdit.Extents;
using WorldEdit.History;

namespace WorldEditTests.History
{
    public class MockChange : Change
    {
        protected override bool RedoImpl(Extent extent) => true;

        protected override bool UndoImpl(Extent extent) => true;
    }
}
