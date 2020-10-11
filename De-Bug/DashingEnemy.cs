using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class DashingEnemy
    {
        static List<List<Rectangle>> sourceRectangles = new List<List<Rectangle>>();
        static int sourceRow = 0;
        static int sourceCol = 0;
        static float walkFrameDuration= 0.1f;
        static float dashFrameDuration= walkFrameDuration*2/3;
        static float frameDuration;
        static float frameCountdown = 0f;
        public int healthPoints;
        Texture2D texture;
        public Rectangle rectangle;
        int padding = 7;
        int lowSpeed; //speed at which the enemy walks
        int highSpeed; //speed at which the enemy chases the player
        int minX; //the horizontal movement of teh enemy is restricted between minX and maxX
        int maxX; //
        int currentSpeed;
        public DashingEnemy(int _healthPoints,
                            Texture2D _texture,
                            Rectangle _rectangle,
                            int _lowSpeed,
                            int _highSpeed,
                            int _minX,
                            int _maxX)
        {
            healthPoints = _healthPoints;
            texture = _texture;
            rectangle= new Rectangle(_rectangle.X + padding, _rectangle.Y, _rectangle.Width - 2*padding, _rectangle.Height);
            lowSpeed = _lowSpeed;
            highSpeed= _highSpeed;
            minX= _minX;
            maxX= _maxX;
            currentSpeed = lowSpeed;
            populateSourceList();
        }

        //define the sorce rectangle for each frame
        void populateSourceList()
        {
            for (int i=0; i<2; i++)
            {
                sourceRectangles.Add( new List<Rectangle>());
                for (int j=0; j<4; j++)
                {
                    sourceRectangles[i].Add(new Rectangle(60 * j, 60 * i, 60, 60));
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            float timeInterval = (float)gameTime.ElapsedGameTime.TotalSeconds;
            frameCountdown -= timeInterval;
            if (frameCountdown<0)
            {//move to next frame
                sourceCol = (sourceCol + 1) % sourceRectangles[sourceRow].Count;
                frameCountdown = frameDuration;
            }
            if (Player.rectangle.Intersects(new Rectangle(minX, rectangle.Y, maxX - minX, rectangle.Height)))
            {//increase the current speed if the player is close to the enemy and th eenemy is facing the player
                frameDuration = dashFrameDuration;
                if (Player.rectangle.X >= rectangle.Right && currentSpeed > 0)
                    currentSpeed = highSpeed;
                else if (Player.rectangle.Right - 1 <= rectangle.X && currentSpeed < 0)
                    currentSpeed = -highSpeed;
            }
            else
            {
                frameDuration = walkFrameDuration;
            }
            //if the enemy reaches either ends of its path, reset the speed to lowSpeed
            if (rectangle.X >= maxX - rectangle.Width)
            {
                currentSpeed = -lowSpeed;
                rectangle.X = maxX - rectangle.Width;
                sourceRow = (sourceRow + 1) % sourceRectangles.Count;
            }
            else if (rectangle.X <= minX)
            {
                currentSpeed = lowSpeed;
                rectangle.X = minX;
                sourceRow = (sourceRow + 1) % sourceRectangles.Count;
            }
            rectangle.X += (int)(currentSpeed * timeInterval);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle(new Rectangle(rectangle.X-padding, rectangle.Y, rectangle.Width+2*padding, rectangle.Height)), 
                sourceRectangles[sourceRow][sourceCol],Color.White);
        }

    }
}
