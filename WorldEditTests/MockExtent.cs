using OTAPI.Tile;
using WorldEdit;
using WorldEdit.Extents;

namespace WorldEditTests
{
    public class MockExtent : Extent
    {
        public ITile[,] Tiles;

        public override Vector LowerBound => Vector.Zero;
        public override Vector UpperBound => new Vector(Tiles.GetLength(0), Tiles.GetLength(1));

        public override Tile this[int x, int y]
        {
            get => new Tile(Tiles[x, y]);
            set => Tiles[x, y] = value.ToITile();
        }
    }
}
