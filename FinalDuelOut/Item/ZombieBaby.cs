using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Item
{
    class ZombieBaby : Baby
    {
        public ZombieBaby(Game game, Block block)
            : base(game, block)
        {
            changeInLife = -1;
            changeInScore = 0;
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ZombieBaby");
        }

        public override void DestroyEffect()
        {
            ScoreManager.Score[(int)SpawnedTeam] += 2;
        }
    }
}
