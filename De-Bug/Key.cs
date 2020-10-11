using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class Key
    {
        Texture2D texture;
        public Rectangle rectangle;
        static List<Rectangle> sourceRectangles = new List<Rectangle>();
        static int sourceCol = 0;
        static float frameDuration = 0.3f;
        static float frameCountdown = 0f;


        public Key(Texture2D _texture, Rectangle _rectangle)
        {
            texture = _texture;
            rectangle = _rectangle;
            for (int col = rectangle.X / Map.tileSize; col < rectangle.X / Map.tileSize + 2; col++)
            {
                Map.map[rectangle.Y / Map.tileSize, col].tileTypeGetSet = Tile.TileType.key;
                Map.map[rectangle.Y / Map.tileSize + 1, col].tileTypeGetSet = Tile.TileType.key;
            }
            populateSourceList();
        }

        //specify the source rectangle for each frame
        void populateSourceList()
        {
            for (int j = 0; j < 4; j++)
            {
                sourceRectangles.Add(new Rectangle(40 * j, 0, 40, 40));
            }
        }

        public void Update(GameTime gameTime)
        {
            frameCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (frameCountdown < 0)
            {//move to next frame
                sourceCol = (sourceCol + 1) % sourceRectangles.Count;
                frameCountdown = frameDuration;
            }
        }

        //set the tile with the key to empty
        public void deleteKey(Rectangle _playerRect)
        {
            if (_playerRect.Intersects(rectangle))
            {
                for (int col=rectangle.X/Map.tileSize; col <rectangle.X / Map.tileSize+2; col++)
                {
                    Map.map[rectangle.Y / Map.tileSize, col].tileTypeGetSet = Tile.TileType.empty;
                    Map.map[rectangle.Y / Map.tileSize+1, col].tileTypeGetSet = Tile.TileType.empty;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle(rectangle), sourceRectangles[sourceCol],Color.White);
        }
    }
}
