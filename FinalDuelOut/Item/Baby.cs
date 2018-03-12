using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breakout.Item
{
    abstract class Baby : Item
    {
        protected int changeInLife;
        protected int changeInScore;

        public Baby(Game game, Block block)
            : base(game, block)
        {
            Gravity = 2.0f;
        }

        public override void TriggerEffect()
        {
            ScoreManager.Lives[(int)SpawnedTeam] += changeInLife;
            ScoreManager.Score[(int)SpawnedTeam] += changeInScore;
            ++ScoreManager.Saved[(int)SpawnedTeam];
        }

        public override void DestroyEffect()
        {
            ScoreManager.Lives[(int)SpawnedTeam] -= changeInLife;
        }
    }
}
