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
    public class Paddle : DrawableSprite, IBallSpawner
    {
        PaddleController controller;

        public TeamColor Team { get; private set; }
        
        public Paddle(Game game, TeamColor team)
            : base(game)
        {
            this.Speed = 200;
            controller = new PaddleController(game, team);
            this.Team = team;
            //reflection and color based on team

            if (Team == TeamColor.Red)
            {
                this.color = Color.Red;
            }
            else
            {
                this.color = Color.Blue;
            }


        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("paddleSmall");
#if DEBUG
            this.ShowMarkers = true;
#endif
            SetInitialLocation();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //Update Collision Rect
            SetCollisionRectangle();

            if (Game1.GameStatus == GameState.Playing)
            {
                controller.HandleInput();
                this.Direction = controller.Direction;
                this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);
            }
            KeepPaddleOnScreen();
            base.Update(gameTime);
        }

        public void SetInitialLocation()
        {
            if (Team == TeamColor.Blue)
                this.Location = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height - SpriteTexture.Height);
            else
                this.Location = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, 0.0f);
        }

        public Ball SpawnBall()
        {
            Ball BallToLaunch = new Ball(Game, BallState.OnPaddleStart, Team);
            BallToLaunch.Initialize();
            BallToLaunch.SetBallLocation(GetBallFollowingPosition());
            return BallToLaunch;
        }

        public Vector2 GetBallFollowingPosition()
        {
            // Initialize for return value
            Vector2 positionToFollow;
            float radius = Ball.BallRadius;
            //get middle and top
            positionToFollow = new Vector2(this.Location.X + this.SpriteTexture.Width / 2 - radius, this.Location.Y);
            //modify y according to the team
            if (Team == TeamColor.Blue)
                positionToFollow.Y -= radius;
            else
                positionToFollow.Y += (this.SpriteTexture.Height);
            return positionToFollow;
        }

        /** Called by BallManager */
        #region action subscription
        public void SubscribeLauncher(Action launchBall)
        {
            controller.LaunchBall += launchBall;
        }
        public void UnsubscribeLauncer(Action launchBall)
        {
            controller.LaunchBall -= launchBall;
        }
        #endregion

        Rectangle collisionRectangle;  //Rectangle for paddle collision
        public Rectangle CollisionRectangle { get => collisionRectangle; }

        public void SetCollisionRectangle()
        {
            if (Team == TeamColor.Blue) // uses just the top of the paddle
                collisionRectangle = new Rectangle((int)this.Location.X, (int)this.Location.Y, this.spriteTexture.Width, 1);
            else //if red use bottom
                collisionRectangle = new Rectangle((int)this.Location.X, (int)this.Location.Y + this.SpriteTexture.Height, this.spriteTexture.Width, -1);
        }


        
        private void KeepPaddleOnScreen()
        {
            this.Location.X = MathHelper.Clamp(this.Location.X, 0, this.Game.GraphicsDevice.Viewport.Width - this.spriteTexture.Width);
        }

        public bool IsCollidingBall(Ball ball)
        {
            //Ball Collsion
            //Very simple collision with ball only uses rectangles
            if (collisionRectangle.Intersects(ball.LocationRect))
            {
                return true;
            }
            return false;
        }
    }
}
