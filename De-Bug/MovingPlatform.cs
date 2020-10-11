using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class MovingPlatform
    {
        Texture2D texture;
        public Rectangle rectangle;
        public Vector2 speed;
        int maxPositionX; //the platform moves between (minPositionX, minPositionY) and (maxPositionX, maxPositionY)
        int maxPositionY;
        int minPositionX;
        int minPositionY;
        float restTime;
        float timeBeforeMoving;
        public bool playerOnTop = false;
        public MovingPlatform(Texture2D _texture, Vector2 _speed, int _maxPositionX, 
            int _maxPositionY, int _minPositionX, int _minPositionY, int width, int height, float _restTime)
        {
            texture = _texture;
            speed = _speed;
            restTime = _restTime;
            maxPositionX = _maxPositionX;
            maxPositionY = _maxPositionY;
            minPositionX = _minPositionX;
            minPositionY = _minPositionY;
            rectangle = new Rectangle(minPositionX, minPositionY, width, height);
            restTime = _restTime;
            timeBeforeMoving = 0;
        }
        public void Update(GameTime gameTime)
        {
            int offset = Player.rectangle.X - rectangle.X;
            float elapsedTime =(float)gameTime.ElapsedGameTime.TotalSeconds;
            timeBeforeMoving -= elapsedTime;
            //wait for time before moving when th eplatform reach eithe rends of its path
            if (timeBeforeMoving > 0)
                return;
            //update platform position
            rectangle.X += (int)(speed.X*elapsedTime);
            rectangle.Y += (int)(speed.Y*elapsedTime);

            //check whether the platform has reached one of the ends of its path
            //if yes, change the sign of the movement speed
            if (rectangle.X > maxPositionX)
            {
                rectangle.X = maxPositionX;
                speed.X *= -1;
                timeBeforeMoving = restTime;
            }
            else if (rectangle.X < minPositionX)
            {
                rectangle.X = minPositionX;
                speed.X *= -1;
                timeBeforeMoving = restTime;

            }
            if (rectangle.Y > maxPositionY)
            {
                rectangle.Y = maxPositionY;
                speed.Y *= -1;
                timeBeforeMoving = restTime;

            }
            else if (rectangle.Y < minPositionY)
            {
                rectangle.Y = minPositionY;
                speed.Y *= -1;
                timeBeforeMoving = restTime;
            }
            //move the player with the platform
            if (playerOnTop)
            {
                Player.rectangle.X = rectangle.X + offset;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Camera.relativeRectangle( new Rectangle(rectangle.X, rectangle.Y-4, rectangle.Width, rectangle.Height)), Color.White);
        }
    }
}
