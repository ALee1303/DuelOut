using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace Breakout
{

    class ScoreManager : DrawableGameComponent
    {

        SpriteFont font;

        public static int[] Lives, Score, Saved;

        Texture2D paddle;

        SpriteBatch sb;
        Vector2 scoreLoc, scoreLocOffset, livesLoc, livesLocOffset, saveLoc, saveLocOffset,
            stateLoc, blueStateLoc, redStateLoc;
        
        
        public ScoreManager(Game game)
            : base(game)
        {
            SetupNewGame();
        }

        public static void SetupNewGame()
        {
            Lives = new int[2] { 5, 5 };
            Score = new int[2] { 0, 0 };
            Saved = new int[2] { 0, 0 };
        }

        public override void Update(GameTime gameTime)
        {
            if (ScoreManager.Lives[(int)TeamColor.Blue] == 0 || ScoreManager.Lives[(int)TeamColor.Red] == 0)
                Game1.GameStatus = GameState.GameOver;
            else if (BlockManager.currentScore == BlockManager.winScore)
                Game1.GameStatus = GameState.Won;
            KeepBoundary();
            base.Update(gameTime);
        }

        private void KeepBoundary()
        {
            for (int i = 0; i < 2; i++)
            {
                if (Lives[i] < 0)
                    Lives[i] = 0;
                else if (Lives[i] > 5)
                    Lives[i] = 5;
                if (Score[i] < 0)
                    Score[i] = 0;
            }
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("Arial");
            paddle = this.Game.Content.Load<Texture2D>("paddleSmall");
            livesLoc = new Vector2(10, 20);
            livesLocOffset = livesLoc + new Vector2(0, 400);
            scoreLoc = new Vector2(10, 40);
            scoreLocOffset = scoreLoc + new Vector2(0, 360);
            saveLoc = new Vector2(10, 60);
            saveLocOffset = saveLoc + new Vector2(0, 320);
            stateLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            blueStateLoc = stateLoc + new Vector2(0, 100);
            redStateLoc = stateLoc - new Vector2(0, 100);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            sb.DrawString(font, "Lives: " + Lives[1], livesLoc, Color.White);
            sb.DrawString(font, "Lives: " + Lives[0], livesLocOffset, Color.White);
            sb.DrawString(font, "Score: " + Score[1], scoreLoc, Color.White);
            sb.DrawString(font, "Score: " + Score[0], scoreLocOffset, Color.White);
            sb.DrawString(font, "Saved: " + Saved[1], saveLoc, Color.White);
            sb.DrawString(font, "Saved: " + Saved[0], saveLocOffset, Color.White);
            /** All Blocks Destroyed */
            if (Game1.GameStatus == GameState.Won)
            {
                if (Score[0] > Score[1])
                    sb.DrawString(font, "Blue Won!", stateLoc, Color.Blue);
                else if (Score[0] < Score[1])
                    sb.DrawString(font, "Red Won!", stateLoc, Color.Red);
                /** When Tie, Compare Saved Babies */
                else
                {
                    if (Saved[0] > Saved[1])
                        sb.DrawString(font, "Blue Won!", stateLoc, Color.Blue);
                    else if (Saved[0] < Saved[1])
                        sb.DrawString(font, "Red Won!", stateLoc, Color.Red);
                }
            }
            /** Someone has died */
            else if (Game1.GameStatus == GameState.GameOver)
            {
                if (Lives[0] <= 0)
                {
                    sb.DrawString(font, "Game Over!", blueStateLoc, Color.Blue);
                    sb.DrawString(font, "Red Won!", redStateLoc, Color.Red);
                }
                else if (Lives[1] <= 0)
                {
                    sb.DrawString(font, "Game Over!", redStateLoc, Color.Red);
                    sb.DrawString(font, "Blue Won!", blueStateLoc, Color.Blue);
                }
            }
            sb.End();
            base.Draw(gameTime);
        }
    }
}
