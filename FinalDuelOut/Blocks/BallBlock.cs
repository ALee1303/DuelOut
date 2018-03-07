using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Blocks
{
    class BallBlock : Block
    {
        private Action<Block> ballBlockBroken;

        public BallBlock(Game game, Action<Block> whenBroken) : base (game)
        {
            ballBlockBroken = whenBroken;
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("block_blue");
            base.LoadContent();
        }

        internal override void HitByBall(Ball ball)
        {
            base.HitByBall(ball);
            ballBlockBroken.Invoke(this);
        }
    }
}
