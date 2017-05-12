using OTAPI.Tile;

namespace WorldEditTests
{
    public class MockTileCollection : ITileCollection
    {
        public ITile[,] Tiles;
        public int Height => Tiles.GetLength(1);

        public int Width => Tiles.GetLength(0);

        public ITile this[int x, int y]
        {
            get => Tiles[x, y];
            set => Tiles[x, y] = value;
        }
    }
}
