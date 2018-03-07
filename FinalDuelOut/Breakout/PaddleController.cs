using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using Microsoft.Xna.Framework.Input;

namespace Breakout
{
    public enum KeyTag { Left, Right, Launch };

    class PaddleController
    {
        InputHandler input;
        TeamColor team;
        Keys[] KeyScheme;

        public Action LaunchBall;
        public Vector2 Direction { get; private set; }

        public PaddleController(Game game, TeamColor team)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            this.Direction = Vector2.Zero;
            this.team = team;
            InitializeKeyScheme();
        }

        void InitializeKeyScheme()
        {
            if (team == TeamColor.Blue)
            {
                KeyScheme = new Keys[3] { Keys.Left, Keys.Right, Keys.Up };
            }
            else
            {
                KeyScheme = new Keys[3] { Keys.Z, Keys.C, Keys.X };
            }
        }

        public void HandleInput()
        {
            this.Direction = Vector2.Zero;  //Start with no direction on each new upafet

            //No need to sum input only uses left and right
            if (input.KeyboardState.IsKeyDown(KeyScheme[(int)KeyTag.Left]))
            {
                this.Direction = new Vector2(-1, 0);
            }
            if (input.KeyboardState.IsKeyDown(KeyScheme[(int)KeyTag.Right]))
            {
                this.Direction = new Vector2(1, 0);
            }
            //TODO add mouse controll?

            //Up launches ball
            if (input.KeyboardState.WasKeyPressed(KeyScheme[(int)KeyTag.Launch]) && LaunchBall != null)
            {
                LaunchBall.Invoke();
            }
        }
    }
}
