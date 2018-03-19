using MonoGameLibrary.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Item
{
    abstract class Item : DrawableSprite, IItem
    {
        protected float Gravity;
        protected Vector2 Momentum;
        protected TeamColor SpawnedTeam;

        public Item(Game game,Block SpawnedBy)
            : base(game)
        {
            Gravity = 9.8f;
            Momentum = new Vector2();
            Location = SpawnedBy.Location;
            SpawnedTeam = SpawnedBy.ScoredTeam;
            if (SpawnedTeam == TeamColor.Blue)
                Direction = new Vector2(0, 1);
            else
                Direction = new Vector2(0, -1);
        }

        public virtual void TriggerEffect()
        { }
        public virtual void DestroyEffect()
        { }

        public void UpdateItem(float dt)
        {
            Location += new Vector2(0.0f, (Momentum.Y + Gravity * dt * Direction.Y / 2));
            Momentum.Y += Gravity * dt * Direction.Y;
            SetTranformAndRect();
        }
        public void DrawItem(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(spriteTexture, Location, Color.White);
            sb.End();
        }
        public bool IsOnScreen()
        {
            //Y bottom
            if (Location.Y >
                    GraphicsDevice.Viewport.Height || Location.Y < 0-this.Rectagle.Height)
                return false;
            return true;
        }
    }
}
