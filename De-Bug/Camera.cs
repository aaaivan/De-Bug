using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace De_Bug
{
    static class Camera
    {
        public static int positionY;
        static int width;
        static int height;

        public static void Inizialize()
        {
            width = Game1.screenWidth;
            height= Game1.screenHeight;
            positionY = Game1.bgHeight - Game1.screenHeight;
        }
        public static void Update()
        {
            //center player in the camera's frame
            positionY = Player.rectangle.Center.Y - height / 2;
            if (positionY < 0)
                positionY = 0;
            if (positionY > Game1.bgHeight - Game1.screenHeight)
                positionY = Game1.bgHeight - Game1.screenHeight;
        }
        public static Rectangle relativeRectangle(Rectangle rect)
        {
            return new Rectangle(rect.X, rect.Y - positionY, rect.Width, rect.Height);
        }
    }
}
