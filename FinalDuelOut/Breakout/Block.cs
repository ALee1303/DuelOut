using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Breakout.Item;

namespace Breakout
{
    public enum BlockState { Normal, Broken };

    public abstract class Block : DrawableSprite
    {
        protected BlockState _state;            //Private instance datamenber for block
        public TeamColor ScoredTeam { get; protected set; }

        public BlockState State
        {
            get { return _state; }
            protected set
            {
                if (this._state != value)       //Change state if it is different than previous state                
                {
                    this._state = value;
                }
            }
        }

        public Block(Game game)
            : base(game)
        {
            _state = BlockState.Normal;
        }
        /// <summary>
        /// Checks if ball is hit by block
        /// </summary>
        /// <param name="ball"></param>
        internal virtual void HitByBall(Ball ball)
        {
            this.Enabled = false;
            this.Visible = false;
            this.ScoredTeam = ball.Team;
            this.State = BlockState.Broken;
        }

        public virtual IItem SpawnItem()
        {
            return null;
        }

    }
}
