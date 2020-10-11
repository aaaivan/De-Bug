using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class JumpingEnemy
    {
        static List<List<Rectangle>> sourceRectangles = new List<List<Rectangle>>();
        static int sourceRow = 1;
        static int sourceCol = 0;
        static float frameDuration=0.2f;
        static float frameCountdown = 0f;
        public int healthPoints;
        Texture2D texture;
        public Rectangle rectangle;
        int padding = 10;
        int groundLevel;
        int maxX; //the enemy moves between minX and maxX
        int minX; //
        float acceleration=1100;
        float initialSpeedY; //initial upwards speed of the enemy when it jumps
        Vector2 speed;
        public JumpingEnemy(int _healthPoints,
                    Texture2D _texture,
                    Rectangle _rectangle,
                    int _maxX,
                    int _minX,
                    Vector2 _speed,
                    float _initialSpeedY)
        {
            healthPoints = _healthPoints;
            texture = _texture;
            rectangle = new Rectangle(_rectangle.X + padding, _rectangle.Y, _rectangle.Width - 2*padding, _rectangle.Height);
            maxX = _maxX;
            minX = _minX;
            speed = _speed;
            groundLevel = rectangle.Bottom;
            initialSpeedY = _initialSpeedY;
            populateSourceList();
        }
        void populateSourceList()
        {
            for (int i = 0; i < 2; i++)
            {
                sourceRectangles.Add(new List<Rectangle>());
                for (int j = 0; j < 5; j++)
                {
                    sourceRectangles[i].Add(new Rectangle(80 * j, 80 * i, 80 , 80));
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            float timeInterval = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCountdown -= timeInterval;
            if (frameCountdown < 0)
            {//move to next frame
                sourceCol = (sourceCol + 1) % sourceRectangles[sourceRow].Count;
                frameCountdown = frameDuration;
            }
            if (rectangle.Bottom>= groundLevel)
            {//when the enemy touches the ground, start a new jump
                rectangle.Y = groundLevel - rectangle.Height;
                speed.Y = initialSpeedY;
            }
            speed.Y += acceleration* timeInterval;
            //when the enemiy reaches either ends of its path,
            //change the sign of the horizontal speed.
            if (rectangle.X >= maxX - rectangle.Width)
            {
                speed.X *= -1;
                rectangle.X = maxX - rectangle.Width;
                sourceRow = (sourceRow + 1) % sourceRectangles.Count;
            }
            else if (rectangle.X <= minX)
            {
                speed.X *= -1;
                rectangle.X = minX;
                sourceRow = (sourceRow + 1) % sourceRectangles.Count;
            }
            rectangle.X += (int)(speed.X * timeInterval);
            rectangle.Y += (int)(speed.Y * timeInterval);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle(new Rectangle(rectangle.X-padding, rectangle.Y, rectangle.Width+2*padding, rectangle.Height)), 
                sourceRectangles[sourceRow][sourceCol], Color.White);
        }

    }
}
