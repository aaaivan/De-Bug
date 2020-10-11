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
    class Tile
    {
        public enum TileType
        {
            empty, key, levelEnd, door, grass, grassR, grassL, dirt, dirtBottom,
            dirtRocks, rocks,rocksR, rocksL,  rocksBottom, rocksTop, halfRocksBottom,
            rocksBottomR, rocksTopR, rocksBottomL, rocksTopL, waterBottom, waterTop,
            lavaBottom, lavaTop, powerUp, total
        }

        TileType tileType;
        Texture2D texture;
        Rectangle rectangle;
        Rectangle sourceRectangle;

        public Tile(TileType _tyleType, Texture2D _texture, Rectangle _rectangle)
        {
            tileType = _tyleType;
            texture = _texture;
            sourceRectangle = new Rectangle (Map.sourceRectangles[(int)tileType].X,
                Map.sourceRectangles[(int)tileType].Y,
                Map.sourceRectangles[(int)tileType].Width,
                Map.sourceRectangles[(int)tileType].Height);
            rectangle = _rectangle;
        }
        public Tile(Texture2D _texture, Rectangle _rectangle)
        {
            tileType = TileType.empty;
            texture = _texture;
            rectangle = _rectangle;
            sourceRectangle = new Rectangle(Map.sourceRectangles[(int)tileType].X,
                Map.sourceRectangles[(int)tileType].Y,
                Map.sourceRectangles[(int)tileType].Width,
                Map.sourceRectangles[(int)tileType].Height);
        }
        public TileType tileTypeGetSet
        {
            get { return tileType; }
            set { tileType = value; sourceRectangle.X = Map.sourceRectangles[(int)tileType].X; }
        }
        public bool isSolid()
        {
            if ((int)tileType > 2 && (int)tileType < 20)
                return true;
            else
                return false;
        }
        public bool isDeadly()
        {
            if ((int)tileType== 20 ||((int)tileType > 21 && (int)tileType < 24))
                return true;
            else
                return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle(rectangle), sourceRectangle, Color.White);
        }
    }
}
