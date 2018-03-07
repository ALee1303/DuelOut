using Breakout.Item;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Blocks
{
    class BabyBlock : Block
    {
        public BabyBlock (Game game) : base (game)
        { }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("block_yellow");
            base.LoadContent();
        }

        /** randomly spawn ZombieBaby or BallonBaby */
        Random r;

        public override IItem SpawnItem()
        {
            IItem spawnedBaby;
            r = new Random();
            int babyCase = r.Next(0, 100) % 3;
            switch (babyCase)
            {
                case 0:
                    spawnedBaby = new ZombieBaby(this.Game, this);
                    return spawnedBaby;
                default:
                    spawnedBaby = new BallonBaby(this.Game, this);
                    return spawnedBaby;
            }
        }
    }
}
