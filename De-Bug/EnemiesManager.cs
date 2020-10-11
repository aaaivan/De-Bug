using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace De_Bug
{
    static class EnemiesManager
    {
        //lists of all enemies of each type 
        static List<BasicEnemy> basicEnemies = new List<BasicEnemy>();
        static List<FallingEnemy> fallingEnemies = new List<FallingEnemy>();
        static List<RotatingEnemy> rotatingEnemies = new List<RotatingEnemy>();
        static List<DashingEnemy> dashingEnemies = new List<DashingEnemy>();
        static List<JumpingEnemy> jumpingEnemies = new List<JumpingEnemy>();

        public static void Inizialize(Texture2D _basic, Texture2D _falling, Texture2D _jumping, Texture2D _dashing)
        {
            basicEnemies.Add(new BasicEnemy(380, 220, 160, _basic, new Rectangle(220, 2360, 40, 40), 1));
            basicEnemies.Add(new BasicEnemy(900, 620, 180, _basic, new Rectangle(620, 1800, 40, 40), 1));
            basicEnemies.Add(new BasicEnemy(720, 160, 200, _basic, new Rectangle(160, 920, 40, 40), 1));
            fallingEnemies.Add(new FallingEnemy(3200, 2860, 1500, 0.6f, _falling, new Rectangle(380, 2860, 40, 40), 1));
            fallingEnemies.Add(new FallingEnemy(3200, 2860, 1500, 0.8f, _falling, new Rectangle(760, 2860, 40, 40), 1));
            fallingEnemies.Add(new FallingEnemy(3560, 3200, 1500, 0.8f, _falling, new Rectangle(480, 3200, 40, 40), 1));
            fallingEnemies.Add(new FallingEnemy(3560, 3200, 1500, 0.6f, _falling, new Rectangle(740, 3200, 40, 40), 1));
            fallingEnemies.Add(new FallingEnemy(3560, 3200, 1500, 0.4f, _falling, new Rectangle(1000, 3200, 40, 40), 1));
            rotatingEnemies.Add(new RotatingEnemy(1, _basic, new Rectangle(700, 2240, 40, 40), 700, 660, 2240, 2080, 100));
            dashingEnemies.Add(new DashingEnemy(31, _dashing, new Rectangle(140, 1080, 60, 60), 100, 200, 140, 500));
            jumpingEnemies.Add(new JumpingEnemy(31, _jumping, new Rectangle(1000, 300, 80, 80), 1000, 240, new Vector2(-100, -750), -750));
        }
        public static void Update(GameTime gameTime)
        {
            foreach (BasicEnemy enemy in basicEnemies)
                enemy.Update(gameTime);
            foreach (FallingEnemy enemy in fallingEnemies)
                enemy.Update(gameTime);
            foreach (RotatingEnemy enemy in rotatingEnemies)
                enemy.Update(gameTime);
            foreach (DashingEnemy enemy in dashingEnemies)
                enemy.Update(gameTime);
            foreach (JumpingEnemy enemy in jumpingEnemies)
                enemy.Update(gameTime);
        }

        //check whether an enemy intersect th erectangle passed as parameter
        //if yes reduce th eenemy's health.
        //If an enemy's health reaches 0, remove the enemy from the list
        public static void applyDamage(Rectangle attackRect)
        {
            List<int> indexes = new List<int>();
            int counter=0;
            foreach (BasicEnemy enemy in basicEnemies)
            {
                if (attackRect.Intersects(enemy.rectangle))
                {
                    enemy.healthPoints--;
                    if (enemy.healthPoints == 0)
                        indexes.Add(counter);
                }
                counter++;
            }
            for (int x = indexes.Count-1; x>=0; x--)
            {
                basicEnemies.RemoveAt(indexes[x]);
            }
            indexes = new List<int>();
            foreach (FallingEnemy enemy in fallingEnemies)
            {
                if (attackRect.Intersects(enemy.rectangle))
                {
                    enemy.healthPoints--;
                }
            }
            indexes = new List<int>();
            counter = 0;
            foreach (RotatingEnemy enemy in rotatingEnemies)
            {
                if (attackRect.Intersects(enemy.rectangle))
                {
                    enemy.healthPoints--;
                    if (enemy.healthPoints == 0)
                        indexes.Add(counter);
                }
                counter++;
            }
            for (int x = indexes.Count - 1; x >= 0; x--)
            {
                rotatingEnemies.RemoveAt(indexes[x]);
            }
            indexes = new List<int>();
            counter = 0;
            foreach (DashingEnemy enemy in dashingEnemies)
            {
                if (attackRect.Intersects(enemy.rectangle))
                {
                    enemy.healthPoints--;
                    if (enemy.healthPoints==0)
                        indexes.Add(counter);
                }
                counter++;
            }
            for (int x = indexes.Count - 1; x >= 0; x--)
            {
                dashingEnemies.RemoveAt(indexes[x]);
            }
            indexes = new List<int>();
            counter = 0;
            foreach (JumpingEnemy enemy in jumpingEnemies)
            {
                if (attackRect.Intersects(enemy.rectangle))
                {
                    enemy.healthPoints--;
                    if (enemy.healthPoints == 0)
                    {
                        indexes.Add(counter);
                        KeysManager.AddKey(enemy.rectangle.X, enemy.rectangle.Y);
                    }
                }
                counter++;
            }
            for (int x = indexes.Count - 1; x >= 0; x--)
            {
                jumpingEnemies.RemoveAt(indexes[x]);
            }
        }

        //check whether an enemy intersect the player
        public static bool intersectPlayer(Rectangle playerRect)
        {
            foreach (BasicEnemy enemy in basicEnemies)
            {
                if (playerRect.Intersects(enemy.rectangle))
                    return true;
            }
            foreach (FallingEnemy enemy in fallingEnemies)
            {
                if (playerRect.Intersects(enemy.rectangle) && enemy.healthPoints>0)
                    return true;
            }
            foreach (RotatingEnemy enemy in rotatingEnemies)
            {
                if (playerRect.Intersects(enemy.rectangle))
                    return true;
            }
            foreach (DashingEnemy enemy in dashingEnemies)
            {
                if (playerRect.Intersects(enemy.rectangle))
                    return true;
            }
            foreach (JumpingEnemy enemy in jumpingEnemies)
            {
                if (playerRect.Intersects(enemy.rectangle))
                    return true;
            }
            return false;
        }
        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (BasicEnemy enemy in basicEnemies)
                enemy.Draw(spriteBatch);
            foreach (FallingEnemy enemy in fallingEnemies)
                enemy.Draw(spriteBatch);
            foreach (RotatingEnemy enemy in rotatingEnemies)
                enemy.Draw(spriteBatch);
            foreach (DashingEnemy enemy in dashingEnemies)
                enemy.Draw(spriteBatch);
            foreach (JumpingEnemy enemy in jumpingEnemies)
                enemy.Draw(spriteBatch);
        }
    }
}
