using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class BasicEnemy
    {
        static List<Rectangle> sourceRectangles = new List<Rectangle>();
        static int sourceCol = 0;
        static float frameDuration = 0.2f;
        static float frameCountdown = 0f;
        public int healthPoints;
        Texture2D texture;
        public Rectangle rectangle;
        int padding = 5;
        int maxX; //this enemy moves back and forth between minX and max X
        int minX; //
        int speed;
        public BasicEnemy(int _maxX, int _minX, int _speed, Texture2D _texture, 
            Rectangle _rectangle, int _health)
        {
            healthPoints = _health;
            texture = _texture;
            rectangle = new Rectangle(_rectangle.X + padding, _rectangle.Y, _rectangle.Width - 2*padding, _rectangle.Height);
            maxX = _maxX;
            minX = _minX;
            speed = _speed;
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
            float timeInterval = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCountdown -= timeInterval;
            if (frameCountdown < 0)
            {//move to next frame
                sourceCol = (sourceCol + 1) % sourceRectangles.Count;
                frameCountdown = frameDuration;
            }

            //if the enemy has reached maxX, chenge the sign of the speed
            if (rectangle.X >= maxX - rectangle.Width)
            {
                speed *= -1;
                rectangle.X = maxX - rectangle.Width;
            }
            else if (rectangle.X <= minX)
            {
                speed *= -1;
                rectangle.X = minX;
            }
            //move enemy
            rectangle.X += (int)(speed * timeInterval);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle( new Rectangle(rectangle.X-padding, rectangle.Y, rectangle.Width+2*padding, rectangle.Height)), 
                sourceRectangles[sourceCol], Color.White);
        }
    }
}
