using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class Door
    {
        Texture2D texture;
        public Rectangle rectangle;
        public bool breakable;
        public Door(Texture2D _texture, Rectangle _rectangle, bool _breakable)
        {
            texture = _texture;
            rectangle = _rectangle;
            breakable = _breakable;
            for(int row= rectangle.Y/Map.tileSize; row< rectangle.Y / Map.tileSize+5; row++ )
            {
                Map.map[row, rectangle.X / Map.tileSize].tileTypeGetSet = Tile.TileType.door;
                Map.map[row, rectangle.X / Map.tileSize+1].tileTypeGetSet = Tile.TileType.door;
            }
        }

        //set the tiles overlapping the door to the empty type
        public void deleteDoor()
        {
            for (int row = rectangle.Y / Map.tileSize; row < rectangle.Y / Map.tileSize + 5; row++)
            {
                Map.map[row, rectangle.X / Map.tileSize].tileTypeGetSet = Tile.TileType.empty;
                Map.map[row, rectangle.X / Map.tileSize + 1].tileTypeGetSet = Tile.TileType.empty;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle(rectangle), Color.White);
        }
    }
}
