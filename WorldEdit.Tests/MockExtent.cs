using OTAPI.Tile;
using WorldEdit.Core;
using WorldEdit.Core.Extents;

namespace WorldEdit.Tests
{
    public class MockExtent : Extent
    {
        public ITile[,] Tiles;

        public override Vector Dimensions => new Vector(Tiles.GetLength(0), Tiles.GetLength(1));

        public override Tile GetTile(int x, int y) => new Tile(Tiles[x, y]);

        public override bool SetTile(int x, int y, Tile tile)
        {
            Tiles[x, y] = new Terraria.Tile
            {
                bTileHeader = tile.BTileHeader,
                frameX = tile.FrameX,
                frameY = tile.FrameY,
                liquid = tile.Liquid,
                sTileHeader = tile.STileHeader,
                type = tile.Type,
                wall = tile.Wall
            };
            return true;
        }
    }
}
