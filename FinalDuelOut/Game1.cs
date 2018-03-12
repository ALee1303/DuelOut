using Breakout;
using Breakout.Item;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace Breakout
{
    public enum GameState { Playing, Won, GameOver };

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static GameState GameStatus;

        //Services
        InputHandler input;

        //Components
        BlockManager bm;
        Paddle redPaddle, bluePaddle;
        BallManager balls;
        ItemManager im;

        ScoreManager score;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            GameStatus = GameState.Playing;

            //Services
            input = new InputHandler(this);
            this.Components.Add(input);

            score = new ScoreManager(this);
            this.Components.Add(score);

            /** adds paddles */
            redPaddle = new Paddle(this, TeamColor.Red);
            bluePaddle = new Paddle(this, TeamColor.Blue);
            this.Components.Add(bluePaddle);
            this.Components.Add(redPaddle);
            Paddle[] pads = new Paddle[] { bluePaddle, redPaddle };

            /** adds BallManager */
            balls = new BallManager(this, pads);
            this.Components.Add(balls);

            im = new ItemManager(this, pads);
            this.Components.Add(im);

            bm = new BlockManager(this, balls, im);
            this.Components.Add(bm);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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

            if (GameStatus != GameState.Playing && input.WasKeyPressed(Keys.Space))
                SetNewGame();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        void SetNewGame()
        {
            bm.LoadLevel();
            im.DestroyAllItem();
            balls.LoadNewGame();
            ScoreManager.SetupNewGame();
            redPaddle.SetInitialLocation();
            bluePaddle.SetInitialLocation();
            GameStatus = GameState.Playing;
        }
    }
}
