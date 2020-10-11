using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    class FallingEnemy
    {
        public int healthPoints;
        Texture2D texture;
        public Rectangle rectangle;
        int maxY; //the enemy will fall from minY to maxY
        int minY;
        int acceleration;
        int speedY = 0;
        float delay; //time before th eenemy falls
        float countdown=0;

        public FallingEnemy(int _maxY, int _minY, int _acceleration, float _delay, Texture2D _texture,
    Rectangle _rectangle, int _health)
        {
            healthPoints = _health;
            texture = _texture;
            rectangle = _rectangle;
            maxY = _maxY;
            minY = _minY;
            acceleration = _acceleration;
            delay = _delay;
        }

        public void Update(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (countdown > 0)
                countdown -= elapsedTime;
            else
            {
                if (rectangle.Y >= maxY)
                {//when the enemy reaches maxY, reset its position to minY
                    rectangle.Y = minY;
                    healthPoints = 1;
                    speedY = 0;
                    countdown = delay;
                }
                else
                {//move enemy down
                    speedY += (int)(acceleration * elapsedTime);
                    rectangle.Y += (int)(speedY * elapsedTime);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (healthPoints>0)
                spriteBatch.Draw(texture, Camera.relativeRectangle(rectangle), Color.White);
        }

    }
}
