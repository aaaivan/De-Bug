using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace De_Bug
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum GameStates
        {
            title, instructions, playing, dead, jump, attack, powerJump, credits
        }
        public static GameStates gameState;
        Dictionary<GameStates, Texture2D> screens = new Dictionary<GameStates, Texture2D>();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D bg, fg;
        Song song;
        public static List<SoundEffect> soundEffects;
        public static int tileSize = 20;
        public static int bgHeight = 3600;
        public static int bgWidth = 1280;
        public static Rectangle bgRect = new Rectangle(0, 0, bgWidth, bgHeight);
        public static int screenHeight = 720;
        public static int screenWidth = 1280;
        public static Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
        public static KeyboardState previousState;
        public static KeyboardState currentState;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //initialization logic here
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            soundEffects = new List<SoundEffect>();
            Camera.Inizialize();
            gameState = GameStates.title;
            previousState = Keyboard.GetState();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load your game content here
            soundEffects.Add(Content.Load<SoundEffect>("jump"));
            soundEffects.Add(Content.Load<SoundEffect>("attack"));
            soundEffects.Add(Content.Load<SoundEffect>("coin"));
            soundEffects.Add(Content.Load<SoundEffect>("keySFX"));
            soundEffects.Add(Content.Load<SoundEffect>("evolve"));
            this.song = Content.Load<Song>("night_wildlife");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume -= 0.5f;
            bg = Content.Load<Texture2D>("bg");
            fg = Content.Load<Texture2D>("fg");
            screens.Add(GameStates.title, Content.Load<Texture2D>("title"));
            screens.Add(GameStates.instructions, Content.Load<Texture2D>("instructions"));
            screens.Add(GameStates.dead, Content.Load<Texture2D>("deathScreen"));
            screens.Add(GameStates.jump, Content.Load<Texture2D>("jumpScreen"));
            screens.Add(GameStates.attack, Content.Load<Texture2D>("attackScreen"));
            screens.Add(GameStates.powerJump, Content.Load<Texture2D>("powerJumpScreen"));
            screens.Add(GameStates.credits, Content.Load<Texture2D>("credits"));
            Map.Inizialize(tileSize, bgHeight, bgWidth, Content.Load<Texture2D>("tiles"));
            PlatformsManager.Inizialize(Content.Load<Texture2D>("platform"), Content.Load<Texture2D>("log"));
            DoorsManager.Inizialize(Content.Load<Texture2D>("breakableDoor"), Content.Load<Texture2D>("lockedDoor"));
            KeysManager.Inizialize(Content.Load<Texture2D>("key"));
            EnemiesManager.Inizialize(Content.Load<Texture2D>("basicEnemy"), Content.Load<Texture2D>("spike"), Content.Load<Texture2D>("jumpingEnemy"), Content.Load<Texture2D>("dashingEnemy"));
            Player.Inizialize(Content.Load<Texture2D>("player"), new Vector2(60, 2460), new Vector2(160, 0));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentState = Keyboard.GetState();
            //update logic here
            switch (gameState)
            {
                case GameStates.title:
                    {
                        if (currentState.IsKeyDown(Keys.Space))
                            gameState = GameStates.instructions;
                        else if (currentState.IsKeyDown(Keys.Enter))
                            gameState = GameStates.playing;
                        break;
                    }
                case GameStates.instructions:
                    {
                        if (currentState.IsKeyDown(Keys.Back))
                            gameState = GameStates.title;
                        break;
                    }
                case GameStates.playing:
                    {
                        PlatformsManager.Update(gameTime);
                        EnemiesManager.Update(gameTime);
                        KeysManager.Update(gameTime);
                        Player.Update(gameTime);
                        Camera.Update();
                        break;
                    }
                case GameStates.dead:
                    {
                        if (currentState.IsKeyDown(Keys.Space) || currentState.IsKeyDown(Keys.Enter))
                            gameState = GameStates.playing;
                        break;
                    }
                case GameStates.jump:
                    {
                        if (currentState.IsKeyDown(Keys.Space) || currentState.IsKeyDown(Keys.W) || currentState.IsKeyDown(Keys.Enter))
                            gameState = GameStates.playing;
                        break;
                    }
                case GameStates.attack:
                    {
                        if (currentState.IsKeyDown(Keys.Space) || currentState.IsKeyDown(Keys.Enter))
                            gameState = GameStates.playing;
                        break;
                    }
                case GameStates.powerJump:
                    {
                        if (currentState.IsKeyDown(Keys.Space) || currentState.IsKeyDown(Keys.Space) || currentState.IsKeyDown(Keys.Enter))
                            gameState = GameStates.playing;
                        break;
                    }
            }


            previousState = currentState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (gameState)
            {
                case GameStates.title:
                    spriteBatch.Draw(screens[GameStates.title], screenRectangle, Color.White);
                    break;
                case GameStates.instructions:
                    spriteBatch.Draw(screens[GameStates.instructions], screenRectangle, Color.White);
                    break;
                case GameStates.playing:
                    spriteBatch.Draw(bg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    EnemiesManager.Draw(spriteBatch);
                    Map.Draw(spriteBatch);
                    PlatformsManager.Draw(spriteBatch);
                    KeysManager.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    DoorsManager.Draw(spriteBatch);
                    spriteBatch.Draw(fg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    break;
                case GameStates.dead:
                    spriteBatch.Draw(bg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    EnemiesManager.Draw(spriteBatch);
                    Map.Draw(spriteBatch);
                    PlatformsManager.Draw(spriteBatch);
                    KeysManager.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    DoorsManager.Draw(spriteBatch);
                    spriteBatch.Draw(fg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(screens[GameStates.dead], screenRectangle, Color.White);
                    break;
                case GameStates.jump:
                    spriteBatch.Draw(bg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    EnemiesManager.Draw(spriteBatch);
                    Map.Draw(spriteBatch);
                    PlatformsManager.Draw(spriteBatch);
                    KeysManager.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    DoorsManager.Draw(spriteBatch);
                    spriteBatch.Draw(fg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(screens[GameStates.jump], screenRectangle, Color.White);
                    break;
                case GameStates.attack:
                    spriteBatch.Draw(bg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    EnemiesManager.Draw(spriteBatch);
                    Map.Draw(spriteBatch);
                    PlatformsManager.Draw(spriteBatch);
                    KeysManager.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    DoorsManager.Draw(spriteBatch);
                    spriteBatch.Draw(fg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(screens[GameStates.attack], screenRectangle, Color.White);
                    break;
                case GameStates.powerJump:
                    spriteBatch.Draw(bg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    EnemiesManager.Draw(spriteBatch);
                    Map.Draw(spriteBatch);
                    PlatformsManager.Draw(spriteBatch);
                    KeysManager.Draw(spriteBatch);
                    Player.Draw(spriteBatch);
                    DoorsManager.Draw(spriteBatch);
                    spriteBatch.Draw(fg, screenRectangle, new Rectangle(0, Camera.positionY, screenWidth, screenHeight), Color.White);
                    spriteBatch.Draw(screens[GameStates.powerJump], screenRectangle, Color.White);
                    break;
                case GameStates.credits:
                    spriteBatch.Draw(screens[GameStates.credits], screenRectangle, Color.White);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
