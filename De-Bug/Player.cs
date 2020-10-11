using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace De_Bug
{
    static class Player
    {
        public enum PlayerStates
        {
            crawl = 0, jump = 1, attack = 2, fly = 3, total=4
        }

        static Texture2D texture;
        static List<List<Rectangle>> sourceRectangles;
        static int sourceRow = 0;
        static int sourceCol = 0;
        public static Rectangle rectangle;
        static int padding = 10; 
        static Rectangle sourceRectangle;
        static PlayerStates playerState= PlayerStates.crawl;
        static Vector2 speed;
        static float initialJumpSpeedY=-900;
        static float acceleration = 2100;
        static public int pUpsCollected = 0;
        static public int keyCollected = 0;
        static bool jumpAllowed = false;
        static bool isFalling = true;
        static bool isOnPlatform = false;
        static MovingPlatform platformWithPlayer; //index of platform with player on top
        public static bool isAlive = true;
        static bool isFacingRight = true;
        static bool isAttacking = false;
        static float attackFrameDuration = 0.1f;
        static float attackFrameCountdown = 0f;
        static float walkFrameDuration= 0.05f;
        static float walkFrameCountdown= walkFrameDuration;

        static  Vector2 checkpoint; //osition of the last power up collected
        static public void Inizialize(Texture2D _texture, Vector2 _position, Vector2 _speed)
        {
            texture = _texture;
            sourceRectangles = new List<List<Rectangle>>();
            populateSourceList();
            sourceRectangle = sourceRectangles[sourceRow][sourceCol];
            rectangle = new Rectangle((int)_position.X+padding, (int)_position.Y, 
                40-padding*2, 60);
            speed = _speed;
            checkpoint = new Vector2(_position.X, _position.Y);
        }

        //define the source rectangle for each frame
        static private void populateSourceList()
        {
                for (int i=0; i< 12; i++)
                {
                    sourceRectangles.Add(new List<Rectangle>());
                    for (int j=0; j<8; j++)
                    {
                        if (i == 6 || i == 7 || i == 10 || i == 11 )
                        {
                            if (j > 4)
                                break;
                            else
                                sourceRectangles[i].Add(new Rectangle(110 * j, 60 * i, 110, 60));
                        }
                        else
                            sourceRectangles[i].Add( new Rectangle(50*j, 60*i, 50,60));
                    }
                }
        }
        static public void Update(GameTime gameTime)
        {
            float timeInterval = (float)gameTime.ElapsedGameTime.TotalSeconds;
            checkPlayerStatus();
            if (!isAlive)
            {//if the player dies, reposition it at the checkpoint
                rectangle.X = (int)checkpoint.X;
                rectangle.Y = (int)checkpoint.Y;
                if (Game1.currentState.IsKeyDown(Keys.Space) || Game1.currentState.IsKeyDown(Keys.Enter))
                    isAlive = true;
                return;
            }
            if(!isAttacking)
                walkAnimation(timeInterval);
            switch(playerState)
            {
                case PlayerStates.crawl:
                    checkPlatforms();
                    moveHorizontal(timeInterval);
                    moveVertical(timeInterval);
                    break;
                case PlayerStates.jump:
                    checkPlatforms();
                    moveHorizontal(timeInterval);
                    moveVertical(timeInterval);
                    break;
                default:
                    checkAttackInput(gameTime);
                    checkPlatforms();
                    moveHorizontal(timeInterval);
                    moveVertical(timeInterval);
                    break;
            }
        }

        //play walk animation
        static void walkAnimation(float timeInterval)
        {
            sourceRow = isFacingRight ? (sourceRow / 2) * 2 : (sourceRow / 2) * 2 + 1;
            walkFrameCountdown -= timeInterval;
            if (walkFrameCountdown < 0)
            {//go to next frame
                walkFrameCountdown = walkFrameDuration;
                sourceCol = (sourceCol + 1) % sourceRectangles[sourceRow].Count;
            }
        }
        static void checkAttackInput(GameTime gameTime)
        {
            if (isAttacking)
            {//play attack animation
                attackFrameCountdown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                checkTargets();
                if (attackFrameCountdown <= 0)
                {
                    attackFrameCountdown = attackFrameDuration;
                    sourceCol = sourceCol + 1;
                    if (sourceCol== sourceRectangles[sourceRow].Count)
                    {//revert to the walking animation
                        isAttacking = false;
                        sourceCol = 0;
                        sourceRow -= 2;
                    }
                }
            }
            else
            { 
                if (Game1.currentState.IsKeyDown(Keys.Space) && !Game1.previousState.IsKeyDown(Keys.Space))
                {//play attacking sound
                    Game1.soundEffects[1].CreateInstance().Play();
                    isAttacking = true;
                    attackFrameCountdown = attackFrameDuration;
                    sourceCol = 0;
                    sourceRow += 2; 
                }
            }
        }

        //check whether an attack has hit a door or an anemy
        private static void checkTargets()
        {
            Rectangle attackRect = getAttackRectangle();
            if (DoorsManager.doors.Count > 0)
            {
                if (DoorsManager.doors[0].breakable && DoorsManager.doors[0].rectangle.Intersects(attackRect))
                {
                    DoorsManager.doors[0].deleteDoor();
                    DoorsManager.doors.RemoveAt(0);
                }
            }
            EnemiesManager.applyDamage(attackRect);
        }

        static void checkPlayerStatus()
        {
            if (EnemiesManager.intersectPlayer(rectangle))
            {//an enemy has hit the player
                isAlive = false;
                Game1.gameState = Game1.GameStates.dead;
                return;
            }
            for (int row = (rectangle.Y / Map.tileSize); row <= (rectangle.Bottom - 1) / Map.tileSize; row++)
            {
                for (int col = (rectangle.X / Map.tileSize); col <= (rectangle.Right - 1) / Map.tileSize; col++)
                {
                    if (Map.map[row, col].isDeadly())
                    {//the player has touched a deadly tile
                        isAlive = false;
                        Game1.gameState = Game1.GameStates.dead;
                        return;
                    }
                    else if (Map.map[row, col].tileTypeGetSet == Tile.TileType.powerUp)
                    {//the player has touched a power up tile
                        Game1.soundEffects[2].CreateInstance().Play();
                        pUpsCollected += 1;
                        Map.map[row, col].tileTypeGetSet = Tile.TileType.empty;
                        checkpoint.X = col * Map.tileSize;
                        checkpoint.Y = row * Map.tileSize;
                        //upgrade player abilities based on number of power ups collected
                        if (pUpsCollected == 3)
                        {
                            Game1.soundEffects[4].CreateInstance().Play();
                            playerState = PlayerStates.jump;
                            Game1.gameState = Game1.GameStates.jump;
                            jumpAllowed = true;
                            sourceRow = isFacingRight ? 2 : 3;
                            speed.X = 200;
                        }
                        else if (pUpsCollected == 6)
                        {
                            Game1.soundEffects[4].CreateInstance().Play();
                            Game1.gameState = Game1.GameStates.attack;
                            playerState = PlayerStates.attack;
                            sourceRow = isFacingRight ? 4 : 5;
                        }
                        else if (pUpsCollected == 10)
                        {
                            Game1.soundEffects[4].CreateInstance().Play();
                            Game1.gameState = Game1.GameStates.powerJump;
                            playerState = PlayerStates.fly;
                            initialJumpSpeedY = -1300;
                            sourceRow = isFacingRight ? 8 : 9;
                        }
                    }
                    else if (Map.map[row, col].tileTypeGetSet == Tile.TileType.key)
                    {//the player has touched a key tile
                        Game1.soundEffects[3].CreateInstance().Play();
                        keyCollected += 1;
                        if (KeysManager.keys.Count > 0)
                        {
                            KeysManager.keys[0].deleteKey(rectangle);
                            KeysManager.keys.RemoveAt(0);
                        }
                    }
                    else if (Map.map[row, col].tileTypeGetSet == Tile.TileType.levelEnd)
                    {//the player has touched a levelEnd tile-> go to credits
                        Game1.gameState = Game1.GameStates.credits;
                    }
                    if (DoorsManager.doors.Count > 0)
                    {
                        if (!DoorsManager.doors[0].breakable && DoorsManager.doors[0].rectangle.Intersects(getDrawRectangle()))
                        {//if the player idìs by a door and has a key, open the door
                            if (keyCollected > 0)
                            {
                                keyCollected--;
                                DoorsManager.doors[0].deleteDoor();
                                DoorsManager.doors.RemoveAt(0);
                            }
                        }
                    }
                }
            }
        }
        static void moveHorizontal(float timeInterval)
        {   Rectangle tempRect = new Rectangle(rectangle.X-padding, rectangle.Y, rectangle.Width+2*padding, rectangle.Height); ;
            if (!isAttacking)
            {//move player rectangle to new position based on the user input
                bool walking = false;
                if ((Game1.currentState.IsKeyDown(Keys.A) || Game1.currentState.IsKeyDown(Keys.Left)))
                {
                    tempRect.X -= (int)(speed.X * timeInterval);
                    isFacingRight = false;
                    walking = !walking;
                }
                if ((Game1.currentState.IsKeyDown(Keys.D) || Game1.currentState.IsKeyDown(Keys.Right)))
                {
                    tempRect.X += (int)(speed.X * timeInterval);
                    isFacingRight = true;
                    walking = !walking;
                }
                if (!walking)
                {
                    sourceCol = 0;//idle frame
                }
            }
            //check whether the new player rectangle is intersecting a solid tile
            //if yes reposition the rectangle so that no intersection occur
            //left hand side
            if (tempRect.X < 0)//levele boundaries
                tempRect.X = 0;
            else if (Map.map[tempRect.Y / Map.tileSize, tempRect.X / Map.tileSize].isSolid() ||
                    Map.map[tempRect.Y / Map.tileSize + 1, tempRect.X / Map.tileSize].isSolid() ||
                    Map.map[tempRect.Y / Map.tileSize + 2, tempRect.X / Map.tileSize].isSolid() ||
                    Map.map[(tempRect.Bottom - 1) / Map.tileSize, tempRect.X / Map.tileSize].isSolid())
                tempRect.X = (tempRect.X / Map.tileSize + 1) * Map.tileSize;
            //right-hand side
            if (tempRect.Right > Game1.bgRect.Right)//level boundaries
                tempRect.X = Game1.bgWidth - rectangle.Width;
            else if (Map.map[tempRect.Y / Map.tileSize, (tempRect.Right - 1) / Map.tileSize].isSolid() ||
                    Map.map[tempRect.Y / Map.tileSize + 1, (tempRect.Right - 1) / Map.tileSize].isSolid() ||
                    Map.map[tempRect.Y / Map.tileSize + 2, (tempRect.Right - 1) / Map.tileSize].isSolid() ||
                    Map.map[(tempRect.Bottom - 1) / Map.tileSize, (tempRect.Right - 1) / Map.tileSize].isSolid())
                tempRect.X = (tempRect.X / Map.tileSize) * Map.tileSize;

            rectangle.X = tempRect.X+padding;
        }
        static void moveVertical(float timeInterval)
        {
            Rectangle tempRect = new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            if (isFalling)
            {
                speed.Y += acceleration * timeInterval;
                if (speed.Y > 600)
                    speed.Y = 600;

                tempRect.Y += (int)(speed.Y*timeInterval);
                //check whether the new player rectangle is intersecting a solid tile
                //if yes reposition the rectangle so that no intersection occur
                //bottom
                if (tempRect.Bottom > Game1.bgRect.Bottom)//level boundaries
                {
                    speed.Y = 0;
                    isFalling = false;
                    rectangle.Y = Game1.bgRect.Bottom - rectangle.Height;
                }
                else if (Map.map[(tempRect.Bottom - 1) / Map.tileSize, tempRect.X / Map.tileSize].isSolid() ||
                    Map.map[(tempRect.Bottom - 1) / Map.tileSize, tempRect.X / Map.tileSize + 1].isSolid() ||
                    Map.map[(tempRect.Bottom - 1) / Map.tileSize, (tempRect.Right - 1) / Map.tileSize].isSolid())
                {
                    speed.Y = 0;
                    isFalling = false;
                    rectangle.Y = (tempRect.Y / Map.tileSize) * Map.tileSize;
                }
                //top
                else if (Map.map[tempRect.Y / Map.tileSize, tempRect.X / Map.tileSize].isSolid() ||
                        Map.map[tempRect.Y / Map.tileSize, tempRect.X / Map.tileSize + 1].isSolid() ||
                        Map.map[tempRect.Y / Map.tileSize, (tempRect.Right - 1) / Map.tileSize].isSolid())
                {
                    speed.Y = 0;
                    rectangle.Y = (tempRect.Y / Map.tileSize + 1) * Map.tileSize;
                    return;
                }
                else
                    rectangle.Y = tempRect.Y;
            }
            //jump input while on a moving platform
            else if (isOnPlatform)
            {
                rectangle.Y = platformWithPlayer.rectangle.Y - rectangle.Height;
                if ((Game1.currentState.IsKeyDown(Keys.W) || Game1.currentState.IsKeyDown(Keys.Up)) 
                    && !(Game1.previousState.IsKeyDown(Keys.W) || Game1.previousState.IsKeyDown(Keys.Up)) && jumpAllowed)
                {
                    Game1.soundEffects[0].CreateInstance().Play();
                    speed.Y = initialJumpSpeedY;
                    isFalling = true;
                    isOnPlatform = false;
                    platformWithPlayer.playerOnTop = false;
                    rectangle.Y += (int)(speed.Y*timeInterval);
                }
            }
            //jump input while in the ground
            else
            {
                if ((Game1.currentState.IsKeyDown(Keys.W) || Game1.currentState.IsKeyDown(Keys.Up))
                    && !(Game1.previousState.IsKeyDown(Keys.W) || Game1.previousState.IsKeyDown(Keys.Up)) && jumpAllowed)
                {
                    Game1.soundEffects[0].CreateInstance().Play();
                    speed.Y = initialJumpSpeedY;
                    isFalling = true;
                }
                else if (!(Map.map[rectangle.Bottom / Map.tileSize, rectangle.X / Map.tileSize].isSolid() ||
                        Map.map[rectangle.Bottom / Map.tileSize, rectangle.X / Map.tileSize + 1].isSolid() ||
                        Map.map[rectangle.Bottom / Map.tileSize, (rectangle.Right - 1) / Map.tileSize].isSolid() ||
                        rectangle.Bottom == Game1.bgRect.Bottom))
                    isFalling = true;
            }
        }

        private static void checkPlatforms()
        {
            if (isOnPlatform)
            {//check whether the player has walked out of a platform
                if (rectangle.X >= platformWithPlayer.rectangle.Right || rectangle.Right <= platformWithPlayer.rectangle.X)
                {
                    isOnPlatform = false;
                    isFalling = true;
                    platformWithPlayer.playerOnTop = false;
                }
            }
            else
            {//check whether the new player rectangle is intersecting a moving platform
             //if yes reposition the rectangle so that no intersection occur
                foreach (MovingPlatform platform in PlatformsManager.platforms)
                {
                    if (platform.rectangle.Intersects(rectangle))
                    {
                        if (rectangle.Center.Y < platform.rectangle.Center.Y)
                        {   
                            //check player's bottom right corner
                            if (rectangle.Right - 1 - platform.rectangle.X < rectangle.Bottom - 1 - platform.rectangle.Y)
                            {
                                rectangle.X = platform.rectangle.X - rectangle.Width;
                                isFalling = true;
                            }
                            //check player's bottom left corner
                            else if (platform.rectangle.Right - 1 - rectangle.X < rectangle.Bottom - 1 - platform.rectangle.Y)
                            {
                                rectangle.X = platform.rectangle.Right;
                                isFalling = true;
                            }
                            else
                            {
                                rectangle.Y = platform.rectangle.Y - rectangle.Height;
                                speed.Y = 0;
                                isFalling = false;
                                isOnPlatform = true;
                                platformWithPlayer = platform;
                                platformWithPlayer.playerOnTop = true;
                            }
                        }
                        else
                        {
                            //check player's top right corner
                            if (rectangle.Right - 1 - platform.rectangle.X < platform.rectangle.Bottom - 1 - rectangle.Y)
                                rectangle.X = platform.rectangle.X - rectangle.Width;
                            //check player's bottom left corner
                            else if (platform.rectangle.Right - 1 - rectangle.X < platform.rectangle.Bottom - 1 - rectangle.Y)
                                rectangle.X = platform.rectangle.Right;
                            else
                            {
                                rectangle.Y = platform.rectangle.Bottom;
                                speed.Y = -initialJumpSpeedY/5;
                            }
                        }
                        return;
                    }
                }
            }
        }
        private static Rectangle getAttackRectangle()
        {
            if (isFacingRight)
                return new Rectangle(rectangle.X+ sourceCol*25, rectangle.Y-20, sourceCol * 25, rectangle.Height+20);
            else
                return new Rectangle(rectangle.X- sourceCol * 25, rectangle.Y-20, sourceCol * 25, rectangle.Height+20);
        }
        static public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,
                Camera.relativeRectangle(getDrawRectangle()),
                sourceRectangles[sourceRow][sourceCol],
                Color.White);
        }
        static Rectangle getDrawRectangle()
        {
            if (isFacingRight)
            {
                return new Rectangle(rectangle.X - padding, rectangle.Y,
                    sourceRectangles[sourceRow][sourceCol].Width, sourceRectangles[sourceRow][sourceCol].Height);
            }
            else
            {
                return new Rectangle(rectangle.X +rectangle.Width + padding- sourceRectangles[sourceRow][sourceCol].Width, rectangle.Y,
                    sourceRectangles[sourceRow][sourceCol].Width, sourceRectangles[sourceRow][sourceCol].Height);
            }
        }
    }
}
