using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    static class PlatformsManager
    {
        static public List<MovingPlatform> platforms = new List<MovingPlatform>();//list of all moving platforms in the level
        static public void Inizialize(Texture2D texture , Texture2D texture2)
        {
            platforms.Add(new MovingPlatform(texture2, new Vector2(160, 0), 660, 2820, 380, 2820,
                Map.tileSize*6, Map.tileSize * 1, 1f));
            platforms.Add(new MovingPlatform(texture, new Vector2(200, 0), 800, 2540, 360, 2540,
                Map.tileSize * 5, Map.tileSize * 2, 0f));
            platforms.Add(new MovingPlatform(texture, new Vector2(0, 200), 560, 1540, 560, 1180,
                Map.tileSize * 5, Map.tileSize * 2, 0f));
            platforms.Add(new MovingPlatform(texture, new Vector2(200, 0), 1100, 1340, 780, 1340,
                Map.tileSize * 5, Map.tileSize * 2, 0f));
            platforms.Add(new MovingPlatform(texture, new Vector2(300, 0), 1100, 1200, 780, 1200,
                Map.tileSize * 5, Map.tileSize * 2, 2f));
            platforms.Add(new MovingPlatform(texture, new Vector2(200, 0), 1100, 1060, 780, 1060,
                Map.tileSize * 5, Map.tileSize * 2, 0.5f));
            platforms.Add(new MovingPlatform(texture, new Vector2(300, 0), 1100, 920, 780, 920,
                Map.tileSize * 5, Map.tileSize * 2, 1f));
            platforms.Add(new MovingPlatform(texture, new Vector2(300, 0), 1100, 640, 700, 640,
                Map.tileSize * 5, Map.tileSize * 2, 0f));
        }
        public static void Update(GameTime gameTime)
        {
            foreach (MovingPlatform p in platforms)
                p.Update(gameTime);
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (MovingPlatform p in platforms)
                if (p.rectangle.Y> Camera.positionY-40 && p.rectangle.Y < Camera.positionY+Game1.screenHeight-1)
                    p.Draw(spriteBatch);
        }
    }
}
