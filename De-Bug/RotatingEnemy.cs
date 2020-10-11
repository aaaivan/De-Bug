using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class RotatingEnemy
    {
        static List<Rectangle> sourceRectangles = new List<Rectangle>();
        static int sourceCol = 0;
        static float frameDuration = 0.2f;
        static float frameCountdown = 0f;
        public int healthPoints;
        Texture2D texture;
        public Rectangle rectangle;
        int maxX; //the enemies moves between for points:
        int minX;//     (minX,minY)--(maxX,minY)
        int maxY;//          |            |
        int minY;//     (minX,maxY)--(maxX,maxY)
        int speed;
        Vector2 speeds;

        public RotatingEnemy(int _healthPoints,
                            Texture2D _texture,
                            Rectangle _rectangle,
                            int _maxX,
                            int _minX,
                            int _maxY,
                            int _minY,
                            int _speed)
        {
            healthPoints= _healthPoints;
            texture = _texture;
            rectangle =_rectangle;
            maxX =_maxX;
            minX= _minX;
            maxY= _maxY;
            minY=_minY;
            speed= _speed;
            speeds = new Vector2(-speed, 0);
            populateSourceList();
        }
        void populateSourceList()
        {
                for (int j = 0; j < 4; j++)
                {
                    sourceRectangles.Add(new Rectangle(40 * j, 0, 40, 40));
                }
        }
        public void Update(GameTime gameTime)
        {
            float timeInterval = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCountdown -= timeInterval;
            if (frameCountdown < 0)
            {//move to next frame
                sourceCol = (sourceCol + 1) % sourceRectangles.Count;
                frameCountdown = frameDuration;
            }
            //movement logic: when the enemy reaches a corner,
            //change the velocities so that it starts moving to the next corner
            if (rectangle.X<= minX-rectangle.Width && rectangle.Y>=maxY)
            {
                speeds.X = 0;
                speeds.Y = -speed;
                rectangle.X = minX - rectangle.Width;
                rectangle.Y = maxY;
            }
            else if (rectangle.X <= minX - rectangle.Width && rectangle.Y <= minY-rectangle.Height)
            {
                speeds.X = speed;
                speeds.Y = 0;
                rectangle.X = minX - rectangle.Width;
                rectangle.Y = minY - rectangle.Height;
            }
            else if(rectangle.X >= maxX  && rectangle.Y <= minY - rectangle.Height)
            {
                speeds.X = 0;
                speeds.Y = speed;
                rectangle.X = maxX;
                rectangle.Y = minY - rectangle.Height;
            }
            else if (rectangle.X >= maxX && rectangle.Y >= maxY)
            {
                speeds.X = -speed;
                speeds.Y = 0;
                rectangle.X = maxX;
                rectangle.Y = maxY;
            }
            rectangle.X += (int)(speeds.X* timeInterval);
            rectangle.Y += (int)(speeds.Y* timeInterval);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle(rectangle), 
                sourceRectangles[sourceCol], Color.White);
        }

    }
}
