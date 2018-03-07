using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Item
{
    public class ItemManager : DrawableGameComponent
    {
        private Paddle[] paddle;
        private List<IItem> items;
        private SpriteBatch spriteBatch;

        public ItemManager(Game game, Paddle[] p)
            : base(game)
        {
            this.paddle = p;
            items = new List<IItem>();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float dTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            if (Game1.GameStatus == GameState.Playing)
                foreach (Paddle pad in paddle)
                    for (int i = items.Count() - 1; i >= 0; i--)
                    {
                        IItem item = items[i];
                        item.UpdateItem(dTime);

                        if (item.PerPixelCollision(pad))
                        {
                            item.TriggerEffect();
                            items.RemoveAt(i);
                        }
                        else if (!items[i].IsOnScreen())
                        {
                            item.DestroyEffect();
                            items.RemoveAt(i);
                        }
                    }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach(IItem item in items)
            {
                item.DrawItem(spriteBatch);
            }
            base.Draw(gameTime);
        }

        public void AddItem(IItem newItem)
        {
            items.Add(newItem);
        }

        public void DestroyAllItem()
        {
            items = new List<IItem>();
        }
    }
}
