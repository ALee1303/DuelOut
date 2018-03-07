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
    class PoopBlock : Block
    {
        public PoopBlock (Game game) : base(game)
        { }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("block_red");
            base.LoadContent();
        }

        public override IItem SpawnItem()
        {
            return new Poop(this.Game, this);
        }
    }
}
