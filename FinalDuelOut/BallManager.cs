using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout
{
    public class BallManager : DrawableGameComponent
    {
        /** collection of Balls */
        public List<Ball> Balls;

        /** private fields */
        Ball redBall, blueBall;
        Paddle redPaddle, bluePaddle;
        Paddle[] pads;
        SpriteBatch spriteBatch;

        Random randomizer;

        public Action<Block> BallBlockBrokenDelegate;

        /** counstructor */
        public BallManager(Game game, Paddle[] paddles)
            : base(game)
        {
            Balls = new List<Ball>();
            redPaddle = paddles[(int)TeamColor.Red];
            bluePaddle = paddles[(int)TeamColor.Blue];
            pads = paddles;
            BallBlockBrokenDelegate = OnBallBlockBroken;
            randomizer = new Random();
        }
        /** Load SpriteBatch*/
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpawnBallOnPads();
            base.LoadContent();
        }

        /** Update all Balls*/
        public override void Update(GameTime gameTime)
        {
            SpawnBasedOnScore();

            MakeBallFollowPads();

            if (Game1.GameStatus == GameState.Playing)
            {
                for (int i = Balls.Count() - 1; i >= 0; i--)
                {
                    //initialize for loop
                    Ball ball = Balls[i];
                    TeamColor ballTeam = ball.Team;
                    //call each update
                    ball.Update(gameTime);
                    //paddle collision
                    foreach (Paddle pad in pads)
                    {
                        if (pad.IsCollidingBall(ball))
                        {
                            ball.Direction = GetBallReflection(ball, pad);
                            if (pad.Team != ballTeam)
                                ball.ChangeTeam();
                        }
                    }
                    //is ball in play?
                    if (!ball.IsOnPlay())
                    {
                        ballTeam = ball.Team;
                        ScoreManager.Score[(int)ballTeam] -= 1;
                        Balls.Remove(ball);
                    }
                }
            }

            base.Update(gameTime);
        }

        /** Draw */
        public override void Draw(GameTime gameTime)
        {
            if (redBall != null)
                redBall.DrawBall(spriteBatch);
            if (blueBall != null)
                blueBall.DrawBall(spriteBatch);
            foreach (Ball ball in Balls)
                ball.DrawBall(spriteBatch);
            base.Draw(gameTime);
        }

        /** Member Functions */
        #region Pad Related
        //lock ball on pads
        private void MakeBallFollowPads()
        {
            if (redBall != null)
                redBall.SetBallLocation(redPaddle.GetBallFollowingPosition());
            if (blueBall != null)
                blueBall.SetBallLocation(bluePaddle.GetBallFollowingPosition());
        }
        //spawning based on color
        private void SpawnBallOnPads(TeamColor team)
        {
            if (team == TeamColor.Red)
            {
                redBall = redPaddle.SpawnBall();
                redPaddle.SubscribeLauncher(LaunchRedBall);
            }
            else
            {
                blueBall = bluePaddle.SpawnBall();
                bluePaddle.SubscribeLauncher(LaunchBlueBall);
            }
        }
        //overload for dual spawn
        private void SpawnBallOnPads()
        {
            SpawnBallOnPads(TeamColor.Red);
            SpawnBallOnPads(TeamColor.Blue);
        }
        #endregion

        #region Ball Spawning Conditions
        /** When No Ball is on play */
        private void SpawnBasedOnScore()
        {
            if (redBall == null && blueBall == null && Balls.Count() == 0)
            {
                if (ScoreManager.Score[(int)TeamColor.Blue] > ScoreManager.Score[(int)TeamColor.Red])
                {
                    SpawnBallOnPads(TeamColor.Red);
                }
                else if (ScoreManager.Score[(int)TeamColor.Blue] < ScoreManager.Score[(int)TeamColor.Red])
                {
                    SpawnBallOnPads(TeamColor.Blue);
                }
                else
                {
                    SpawnBallOnPads();
                }
            }
        }
        /** When ball hits a BallBlock */
        private void OnBallBlockBroken(Block brokenBlock)
        {
            /** based on Team */
            TeamColor scoredTeam = brokenBlock.ScoredTeam;
            Ball spawnedBall;
            // when that team pad is empty
            if (scoredTeam == TeamColor.Blue && blueBall == null)
            {
                SpawnBallOnPads(TeamColor.Blue);
                return;
            }
            else if (scoredTeam == TeamColor.Red && redBall == null)
            {
                SpawnBallOnPads(TeamColor.Red);
                return;
            }
            // when pad is not empty
            //direction based on team
            spawnedBall = new Ball(Game, BallState.Playing, scoredTeam);
            spawnedBall.Initialize();
            float xDir = randomizer.Next(-1, 1);
            float yOffSet = brokenBlock.spriteTexture.Height * 1.5f;
            if (scoredTeam == TeamColor.Blue)
                spawnedBall.SetDirection(new Vector2(xDir, 1));
            else
            {
                spawnedBall.SetDirection(new Vector2(xDir, -1));
                yOffSet *= -1;
            }
            spawnedBall.SetBallLocation(brokenBlock.Location + new Vector2(brokenBlock.spriteTexture.Width / 2, yOffSet));

            Balls.Add(spawnedBall);
        }
        #endregion

        #region Launchers
        private void LaunchRedBall()
        {
            redBall.LaunchBall();
            Balls.Add(redBall);
            redPaddle.UnsubscribeLauncer(LaunchRedBall);
            redBall = null;
        }
        private void LaunchBlueBall()
        {
            blueBall.LaunchBall();
            Balls.Add(blueBall);
            bluePaddle.UnsubscribeLauncer(LaunchBlueBall);
            blueBall = null;
        }
        #endregion

        #region ball reflection
        private Vector2 GetBallReflection(Ball ball, Paddle pad)
        {
            /* Adds a bit of entropy to bounce nothing should be perfect */
            ball.Direction.Y *= -1;
            ball.Direction.X += ModifyXRandomFuness();
            ball.Direction.X += ModifyXBasedOnCollision(ball, pad);

            return ball.Direction;
        }

        private float ModifyXRandomFuness()
        {
            return ((randomizer.Next(0, 3) - 1) * 0.1f); //return -0.1~0.1
        }

        private float ModifyXBasedOnCollision(Ball ball, Paddle pad)
        {
            float toRet = 0.0f;
            //Change angle based on paddle movement
            if (pad.Direction.X > 0)
            {
                toRet += .1f;
            }
            if (pad.Direction.X < 0)
            {
                toRet -= .1f;
            }
            //Change anlge based on side of paddle
            //First Third

            if ((ball.Location.X > pad.Location.X) && (ball.Location.X < pad.Location.X + pad.spriteTexture.Width / 3))
            {
                //console.GameConsoleWrite("1st Third");
                toRet += .1f;
            }
            if ((ball.Location.X > pad.Location.X + (pad.spriteTexture.Width / 3)) && (ball.Location.X < pad.Location.X + (pad.spriteTexture.Width / 3) * 2))
            {
                //console.GameConsoleWrite("2nd third");
            }
            if ((ball.Location.X > (pad.Location.X + (pad.spriteTexture.Width / 3) * 2)) && (ball.Location.X < pad.Location.X + (pad.spriteTexture.Width)))
            {
                //console.GameConsoleWrite("3rd third");
                toRet -= .1f;
            }
            return toRet;
        }
        #endregion

        public void LoadNewGame()
        {
            redBall = null;
            blueBall = null;
            redPaddle.UnsubscribeLauncer(LaunchRedBall);
            bluePaddle.UnsubscribeLauncer(LaunchBlueBall);
            Balls.Clear();
            SpawnBallOnPads();
        }
    }
}
