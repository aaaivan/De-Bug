using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace De_Bug
{
    static class Map
    {
        static public int tileSize;
        static public int bgHeightTiles;
        static public int bgWidthTiles;
        static public Tile[,] map;
        static public Texture2D texture;
        static public List<Rectangle> sourceRectangles = new List<Rectangle>();

        static public void Inizialize(int _tileSize, int _bgHeightPx, int _bgWidthPx, Texture2D _texture)
        {
            tileSize = _tileSize;
            bgHeightTiles = _bgHeightPx / _tileSize;
            bgWidthTiles = _bgWidthPx / _tileSize;
            texture = _texture;
            for (int count=0; count<(int)Tile.TileType.total; count++)
            {
                sourceRectangles.Add(new Rectangle(count * tileSize, 0, tileSize, tileSize));
            }
            map = new Tile[bgHeightTiles, bgWidthTiles];
            for (int row=0; row<bgHeightTiles; row++)
            {
                for (int col = 0; col< bgWidthTiles; col++)
                {
                    map[row, col] = new Tile(texture, new Rectangle(col*tileSize, row*tileSize, tileSize, tileSize));
                }
            }
            createLevelMap();
            insertCollectables();
        }

        //build the tilemap: define the type of tile for each tile in the level
        static private void createLevelMap()
        {
            for (int col = 0; col < bgWidthTiles; col++)
            {
                map[0, col].tileTypeGetSet = Tile.TileType.dirt;
                map[1, col].tileTypeGetSet = Tile.TileType.dirtBottom;
                map[bgHeightTiles - 1, col].tileTypeGetSet = Tile.TileType.rocks;
                map[bgHeightTiles - 2, col].tileTypeGetSet = Tile.TileType.rocksTop;
            }
            map[1, 58].tileTypeGetSet = Tile.TileType.dirt;
            map[1, 59].tileTypeGetSet = Tile.TileType.dirt;
            map[bgHeightTiles - 2, 62].tileTypeGetSet = Tile.TileType.rocks;
            map[bgHeightTiles - 2, 63].tileTypeGetSet = Tile.TileType.rocks;

            for (int row = 1; row < 144; row++)
            {
                map[row, 0].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 1].tileTypeGetSet = Tile.TileType.dirt;
                if (row < 9)
                    continue;
                map[row, bgWidthTiles - 1].tileTypeGetSet = Tile.TileType.dirt;
                map[row, bgWidthTiles - 2].tileTypeGetSet = Tile.TileType.dirt;
            }
            map[144, 0].tileTypeGetSet = Tile.TileType.dirtRocks;
            map[144, 1].tileTypeGetSet = Tile.TileType.dirtRocks;
            map[144, bgWidthTiles - 1].tileTypeGetSet = Tile.TileType.dirtRocks;
            map[144, bgWidthTiles - 2].tileTypeGetSet = Tile.TileType.dirtRocks;

            for (int row = 145; row < 178; row++)
            {
                map[row, 0].tileTypeGetSet = Tile.TileType.rocks;
                map[row, 1].tileTypeGetSet = Tile.TileType.rocksR;
                map[row, bgWidthTiles - 1].tileTypeGetSet = Tile.TileType.rocks;
                map[row, bgWidthTiles - 2].tileTypeGetSet = Tile.TileType.rocksL;
            }
            for (int col = 58; col < bgWidthTiles; col++)
            { 
                map[7, col].tileTypeGetSet = Tile.TileType.grass;
                map[8, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 12; col < bgWidthTiles-2; col++)
            {
                map[19, col].tileTypeGetSet = Tile.TileType.grass;
                map[20, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 4; col++)
            {
                map[25, col].tileTypeGetSet = Tile.TileType.grass;
                map[26, col].tileTypeGetSet = Tile.TileType.dirt;
                map[108, col].tileTypeGetSet = Tile.TileType.grass;
                map[109, col].tileTypeGetSet = Tile.TileType.dirt;
                map[164, col].tileTypeGetSet = Tile.TileType.rocksTop;
                map[165, col].tileTypeGetSet = Tile.TileType.rocksBottom;
            }
            map[164, 1].tileTypeGetSet = Tile.TileType.rocks;
            map[165, 1].tileTypeGetSet = Tile.TileType.rocks;
            for (int col = 30; col < 34; col++)
            {
                map[32, col].tileTypeGetSet = Tile.TileType.grass;
                map[33, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 34; row <43; row++)
            {
                map[row, 30].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 31].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 47; col < 49; col++)
            {
                map[76, col].tileTypeGetSet = Tile.TileType.dirt;
                map[77, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 30; col++)
            {
                if (col>7 && col<24)
                {
                    if (col>14 && col<17)
                    {
                        map[39, col].tileTypeGetSet = Tile.TileType.grass;
                        map[40, col].tileTypeGetSet = Tile.TileType.dirt;
                        continue;
                    }
                    map[39, col].tileTypeGetSet = Tile.TileType.waterTop;
                    map[40, col].tileTypeGetSet = Tile.TileType.waterBottom;
                    continue;
                }
                map[39, col].tileTypeGetSet = Tile.TileType.grass;
                map[40, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 30; col++)
            {
                map[41, col].tileTypeGetSet = Tile.TileType.dirt;
                map[42, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 38; col++)
            {
                map[48, col].tileTypeGetSet = Tile.TileType.grass;
                map[49, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 28; col++)
            {
                map[57, col].tileTypeGetSet = Tile.TileType.grass;
                map[58, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 50; row < 76; row++)
            {
                map[row, 36].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 37].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 59; row < 84; row++)
            {
                map[row, 26].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 27].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 38; col < 49; col++)
            {
                map[74, col].tileTypeGetSet = Tile.TileType.grass;
                map[75, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 56; col < 62; col++)
            {
                map[76, col].tileTypeGetSet = Tile.TileType.grass;
                map[77, col].tileTypeGetSet = Tile.TileType.dirt;
                map[99, col].tileTypeGetSet = Tile.TileType.lavaTop;
                map[100, col].tileTypeGetSet = Tile.TileType.lavaBottom;
                map[101, col].tileTypeGetSet = Tile.TileType.dirt;
                map[102, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 7; col++)
            {
                map[69, col].tileTypeGetSet = Tile.TileType.grass;
                map[70, col].tileTypeGetSet = Tile.TileType.dirt;
                map[94, col].tileTypeGetSet = Tile.TileType.grass;
                map[95, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 12; col < 18; col++)
            {
                map[66, col].tileTypeGetSet = Tile.TileType.grass;
                map[67, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 11; col < 16; col++)
            {
                map[76, col].tileTypeGetSet = Tile.TileType.grass;
                map[77, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 68; row < 78; row++)
            {
                map[row, 16].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 17].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 53; col++)
            {
                if (col > 6 && col < 16)
                    continue;
                map[83, col].tileTypeGetSet = Tile.TileType.grass;
                map[84, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 12; col < 56; col++)
            {
                if (col > 30 && col < 47)
                    continue;
                map[90, col].tileTypeGetSet = Tile.TileType.grass;
                map[91, col].tileTypeGetSet = Tile.TileType.dirt;
            }

            for (int col = 29; col < 49; col++)
            {
                map[93, col].tileTypeGetSet = Tile.TileType.dirt;
                if (col < 31 || col > 46)
                {
                    map[92, col].tileTypeGetSet = Tile.TileType.dirt;
                    continue;
                }
                map[92, col].tileTypeGetSet = Tile.TileType.grass;
            }
            for (int row = 92; row < 103; row++)
            {
                map[row, 54].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 55].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 9; col < 23; col++)
            {
                if (col > 10 && col < 21)
                    continue;
                map[102, col].tileTypeGetSet = Tile.TileType.grass;
                map[103, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 11; col < 21; col++)
            {
                map[100, col].tileTypeGetSet = Tile.TileType.grass;
                map[101, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 21; col++)
            {
                map[120, col].tileTypeGetSet = Tile.TileType.grass;
                map[121, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 7; col < 21; col++)
            {
                if (col>8 && col<19)
                    continue;
                map[113, col].tileTypeGetSet = Tile.TileType.grass;
                map[114, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 11; col < 55; col++)
            {
                if (col>13 && col<45)
                    continue;
                map[109, col].tileTypeGetSet = Tile.TileType.grass;
                map[110, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 23; col <46; col++)
            {
                map[114, col].tileTypeGetSet = Tile.TileType.lavaTop;
                map[115, col].tileTypeGetSet = Tile.TileType.lavaBottom;
                map[116, col].tileTypeGetSet = Tile.TileType.dirt;
                map[117, col].tileTypeGetSet = Tile.TileType.dirtBottom;
            }

            for (int row = 104; row < 115; row++)
            {
                map[row, 9].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 10].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 104; row < 122; row++)
            {
                map[row, 21].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 22].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 102; row < 112; row++)
            {
                map[row, 27].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 28].tileTypeGetSet = Tile.TileType.dirt;
                if (row < 104)
                    continue;
                map[row, 33].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 34].tileTypeGetSet = Tile.TileType.dirt;
                if (row < 106)
                    continue;
                map[row, 39].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 40].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 108; row < 118; row++)
            {
                map[row, 45].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 46].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int row = 111; row < 122; row++)
            {
                map[row, 53].tileTypeGetSet = Tile.TileType.dirt;
                map[row, 54].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 60; col < 62; col++)
            {
                map[114, col].tileTypeGetSet = Tile.TileType.grass;
                map[115, col].tileTypeGetSet = Tile.TileType.dirt;
                map[147, col].tileTypeGetSet = Tile.TileType.rocksTop;
                map[148, col].tileTypeGetSet = Tile.TileType.halfRocksBottom;
            }
            map[147, 62].tileTypeGetSet = Tile.TileType.rocks;
            map[148, 62].tileTypeGetSet = Tile.TileType.rocks;
            for (int col = 55; col < 57; col++)
            {
                map[120, col].tileTypeGetSet = Tile.TileType.grass;
                map[121, col].tileTypeGetSet = Tile.TileType.dirt;
                map[153, col].tileTypeGetSet = Tile.TileType.rocksTop;
                map[154, col].tileTypeGetSet = Tile.TileType.halfRocksBottom;
            }
            for (int col = 47; col < 62; col++)
            {
                map[127, col].tileTypeGetSet = Tile.TileType.grass;
                map[128, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 7; col < 17; col++)
            {
                map[132, col].tileTypeGetSet = Tile.TileType.grass;
                map[133, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 2; col < 9; col++)
            {
                map[139, col].tileTypeGetSet = Tile.TileType.grass;
                map[140, col].tileTypeGetSet = Tile.TileType.dirt;
                map[141, col].tileTypeGetSet = Tile.TileType.dirt;
                map[142, col].tileTypeGetSet = Tile.TileType.dirt;
                map[143, col].tileTypeGetSet = Tile.TileType.dirt;
                map[144, col].tileTypeGetSet = Tile.TileType.dirtRocks;
            }
            for (int col = 9; col < 45; col++)
            {
                map[143, col].tileTypeGetSet = Tile.TileType.dirt;
                map[144, col].tileTypeGetSet = Tile.TileType.dirtRocks;
                if (col>18 && col<39)
                {
                    map[141, col].tileTypeGetSet = Tile.TileType.waterTop;
                    map[142, col].tileTypeGetSet = Tile.TileType.waterBottom;
                    continue;
                }
                map[141, col].tileTypeGetSet = Tile.TileType.grass;
                map[142, col].tileTypeGetSet = Tile.TileType.dirt;
            }
            for (int col = 45; col < 55; col++)
            {
                map[143, col].tileTypeGetSet = Tile.TileType.grass;
                map[144, col].tileTypeGetSet = Tile.TileType.dirtRocks;
            }
            for (int col = 9; col < 62; col++)
            {
                map[160, col].tileTypeGetSet = Tile.TileType.rocksTop;
                map[161, col].tileTypeGetSet = Tile.TileType.rocksBottom;
            }
            map[160, 62].tileTypeGetSet = Tile.TileType.rocks;
            map[161, 62].tileTypeGetSet = Tile.TileType.rocks;
            for (int col = 7; col < 9; col++)
            {
                map[171, col].tileTypeGetSet = Tile.TileType.rocksTop;
                map[172, col].tileTypeGetSet = Tile.TileType.rocksBottom;
            }
            for (int row = 145; row < 155; row++)
            {
                map[row, 53].tileTypeGetSet = Tile.TileType.rocksL;
                map[row, 54].tileTypeGetSet = Tile.TileType.rocksR;
            }
            map[153, 54].tileTypeGetSet = Tile.TileType.rocks;
            map[154, 54].tileTypeGetSet = Tile.TileType.halfRocksBottom;
            map[154, 53].tileTypeGetSet = Tile.TileType.halfRocksBottom;

            for (int row = 161; row < 173; row++)
            {
                map[row, 9].tileTypeGetSet = Tile.TileType.rocksL;
                map[row, 10].tileTypeGetSet = Tile.TileType.rocksR;
            }
            map[160, 9].tileTypeGetSet = Tile.TileType.rocksTopL;
            map[161, 10].tileTypeGetSet = Tile.TileType.rocks;
            for (int row = 2; row < 7; row++)
            {
                map[row, 60].tileTypeGetSet = Tile.TileType.levelEnd;
                map[row, 61].tileTypeGetSet = Tile.TileType.levelEnd;
                map[row, 62].tileTypeGetSet = Tile.TileType.levelEnd;
                map[row, 63].tileTypeGetSet = Tile.TileType.levelEnd;
            }
            map[171, 9].tileTypeGetSet = Tile.TileType.rocksTop;
            map[172, 9].tileTypeGetSet = Tile.TileType.rocksBottom;
            map[83, 26].tileTypeGetSet = Tile.TileType.dirt;
            map[83, 27].tileTypeGetSet = Tile.TileType.dirt;

        }
        static private void insertCollectables()
        {
            map[175, 57].tileTypeGetSet = Tile.TileType.powerUp;
            map[157, 12].tileTypeGetSet = Tile.TileType.powerUp;
            map[138, 42].tileTypeGetSet = Tile.TileType.powerUp;
            map[129, 11].tileTypeGetSet = Tile.TileType.powerUp;
            map[96, 17].tileTypeGetSet = Tile.TileType.powerUp;
            map[86, 52].tileTypeGetSet = Tile.TileType.powerUp;
            map[79, 22].tileTypeGetSet = Tile.TileType.powerUp;
            map[53, 3].tileTypeGetSet = Tile.TileType.powerUp;
            map[55, 58].tileTypeGetSet = Tile.TileType.powerUp;
            map[44, 4].tileTypeGetSet = Tile.TileType.powerUp;
        }
        static public void Draw(SpriteBatch spriteBatch)
        {
            for (int row= Camera.positionY/tileSize; row< (Camera.positionY+Game1.screenHeight-1) / tileSize +1; row++)
            {
                for (int col =0 ; col < bgWidthTiles; col++)
                {
                    map[row, col].Draw(spriteBatch);
                }
            }
        }
    }
}
