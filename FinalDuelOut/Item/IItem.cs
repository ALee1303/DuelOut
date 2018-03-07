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
    public interface IItem
    {
        void TriggerEffect();
        void DestroyEffect();
        void UpdateItem(float gameTime);
        void DrawItem(SpriteBatch spriteBatch);

        bool PerPixelCollision(Sprite OtherSprite);
        bool IsOnScreen();
    }
}
