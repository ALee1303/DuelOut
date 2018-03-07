using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Item
{
    class Poop : Item
    {
        public Poop(Game game, Block block)
            : base(game, block)
        {
            spriteTexture = Game.Content.Load<Texture2D>("Poop");
            Direction *= -1;
        }

        public override void TriggerEffect()
        {
            TeamColor opponent;
            if (SpawnedTeam == TeamColor.Blue)
                opponent = TeamColor.Red;
            else
                opponent = TeamColor.Blue;
            --ScoreManager.Lives[(int)opponent];
        }
    }
}
