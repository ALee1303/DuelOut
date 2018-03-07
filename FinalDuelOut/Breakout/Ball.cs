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
    public enum BallState { OnPaddleStart, Playing }
    public enum TeamColor { Blue, Red }

    public class Ball : DrawableSprite
    {
        public BallState State { get; protected set; }
        public TeamColor Team { get; private set; }

        public bool Reflected { get; private set; }
        public static float BallRadius;

        GameConsole console;

        public Ball(Game game, BallState state, TeamColor team)
            : base(game)
        {
            //initial state
            this.State = state;
            this.Team = team;

            //set color based on team
            if (Team == TeamColor.Blue)
                this.color = Color.Blue;
            else
                this.color = Color.Red;


            //console check
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }
#if DEBUG
            this.ShowMarkers = true;
#endif
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            BallRadius = this.SpriteTexture.Width;

            base.LoadContent();
        }

        public void SetBallLocation(Vector2 SpawnLocation)
        {
            this.Location = SpawnLocation;
        }
        public void SetDirection(Vector2 direction)
        {
            this.Speed = 190;
            this.Direction = direction;
        }

        public virtual void LaunchBall()
        {
            Vector2 LaunchDir;

            if (this.State == BallState.OnPaddleStart)
            {
                if (Team == TeamColor.Red)
                    LaunchDir = new Vector2(-1, 1);
                else
                    LaunchDir = new Vector2(1, -1);
                this.Speed = 190;
                this.Direction = LaunchDir;
                this.State = BallState.Playing;
            }
        }

        public override void Update(GameTime gameTime)
        {
            Reflected = false;
            if (State == BallState.Playing)
            {
                this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

                //bounce off wall
                //Left and Right
                if ((this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
                    || (this.Location.X < 0))
                {
                    this.Direction.X *= -1;
                }
            }
            base.Update(gameTime);
        }
        public void DrawBall(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(spriteTexture, Location, color);
            sb.End();
        }

        //checks if ball is on play
        public bool IsOnPlay()
        {
            bool toRet;
            if ((this.Location.Y + this.spriteTexture.Height > this.Game.GraphicsDevice.Viewport.Height) || (this.Location.Y < 0))
                toRet = false;
            else
                toRet = true;
            return toRet;
        }

        public void ReflectFromBlock(Block block)
        {
            if (Math.Abs(this.Location.X - block.Location.X) >= Math.Abs(this.Location.Y - block.Location.Y))
                this.Direction.X *= -1;
            if (Math.Abs(this.Location.X - block.Location.X) <= Math.Abs(this.Location.Y - block.Location.Y))
                this.Direction.Y *= -1;
            Reflected = true;
        }

        public void ChangeTeam()
        {
            if (Team == TeamColor.Blue)
            {
                Team = TeamColor.Red;
                color = Color.Red;
            }
            else
            {
                Team = TeamColor.Blue;
                color = Color.Blue;
            }
        }

    }
}
