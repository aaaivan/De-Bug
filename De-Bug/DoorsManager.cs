using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    static class DoorsManager
    {
        static public List<Door> doors = new List<Door>(); //list of all doors in the level

        public static void Inizialize(Texture2D _textureBreak, Texture2D _textureUnbreak)
        {
            doors.Add(new Door(_textureBreak, new Rectangle(320, 1560, Map.tileSize * 2, Map.tileSize * 5), true));
            doors.Add(new Door(_textureBreak, new Rectangle(180, 2300, Map.tileSize * 2, Map.tileSize * 5), true));
            doors.Add(new Door(_textureUnbreak, new Rectangle(320, 1700, Map.tileSize * 2, Map.tileSize * 5), false));
            doors.Add(new Door(_textureUnbreak, new Rectangle(940, 1560, Map.tileSize * 2, Map.tileSize * 5), false));
            doors.Add(new Door(_textureUnbreak, new Rectangle(1160, 40, Map.tileSize * 2, Map.tileSize * 5), false));
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (Door door in doors)
                if (door.rectangle.Bottom>= Camera.positionY && door.rectangle.Y<= Camera.positionY+Game1.screenHeight)
                    door.Draw(spriteBatch);
        }
    }
}
