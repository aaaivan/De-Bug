using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    static class KeysManager
    {
        public static List<Key> keys = new List<Key>();//list of all the keys in the level
        static Texture2D texture;
        public static void Inizialize(Texture2D _texture)
        {
            texture = _texture;
            keys.Add(new Key(_texture, new Rectangle(240, 2120, Map.tileSize * 2, Map.tileSize * 2)));
            keys.Add(new Key(_texture, new Rectangle(1180, 1580, Map.tileSize * 2, Map.tileSize * 2)));
        }
        public static void Update(GameTime gameTime)
        {
            foreach (Key key in keys)
            {
                key.Update(gameTime);
            }
        }
        public static void AddKey(int x, int y)
        {
            keys.Add(new Key(texture, new Rectangle(x - (x % Map.tileSize), y - (y % Map.tileSize), Map.tileSize * 2, Map.tileSize * 2)));
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(Key key in keys)
            {
                if (key.rectangle.Bottom >= Camera.positionY && key.rectangle.Y <= Camera.positionY + Game1.screenHeight)
                    key.Draw(spriteBatch);
            }
        }
    }
}
