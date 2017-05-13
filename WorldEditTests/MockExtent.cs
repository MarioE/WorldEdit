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

        public override Tile GetTile(int x, int y) => new Tile(Tiles[x, y]);

        public override bool SetTile(int x, int y, Tile tile)
        {
            Tiles[x, y] = tile.ToITile();
            return true;
        }
    }
}
