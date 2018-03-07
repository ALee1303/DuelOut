using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Item
{
    class BallonBaby : Baby
    {
        public BallonBaby(Game game, Block block)
            : base(game, block)
        {
            changeInLife = 1;
            changeInScore = 2;
            this.spriteTexture = this.Game.Content.Load<Texture2D>("BallonBaby");
        }
    }
}
