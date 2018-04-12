using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Breakout.Item;
using Breakout.Blocks;

namespace Breakout
{
    class BlockManager : DrawableGameComponent
    {

        public List<Block> Blocks { get; private set; } //List of Blocks the are managed by Block Manager
        public static int winScore;
        public static int currentScore;

        //Dependancy on Ball
        BallManager ballManager;
        ItemManager itemManager;

        Queue<int> caseQueue;

        /// <summary>
        /// BlockManager hold a list of blocks and handles updating, drawing a block collision
        /// </summary>
        /// <param name="game">Reference to Game</param>
        /// <param name="ball">Refernce to Ball for collision</param>
        public BlockManager(Game game, BallManager b, ItemManager im)
            : base(game)
        {
            this.Blocks = new List<Block>();
            caseQueue = new Queue<int>();
            this.ballManager = b;
            this.itemManager = im;
        }

        public override void Initialize()
        {
            LoadLevel();
            base.Initialize();
        }

        /// <summary>
        /// Replacable Method to Load a Level by filling the Blocks List with Blocks
        /// </summary>
        public virtual void LoadLevel()
        {
            currentScore = 0;
            Blocks.Clear();
            int width = 24;
            int height = 4;
            Random r = new Random();
            for (int i = 0; i < width * height; i++)
            {
                int blockCase = r.Next(0, 1000) % 10; // 0~9
                caseQueue.Enqueue(blockCase);
            }
            CreateBlockArrayByWidthAndHeight(width, height, 1);
        }

        /// <summary>
        /// Simple Level lays out multiple levels of blocks
        /// </summary>
        /// <param name="width">Number of blocks wide</param>
        /// <param name="height">Number of blocks high</param>
        /// <param name="margin">space between blocks</param>
        private void CreateBlockArrayByWidthAndHeight(int width, int height, int margin)
        {
            Block b;
            winScore = 0;
            //Create Block Array based on with and hieght
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    b = GetRandomBlock();
                    b.Initialize();
                    b.Location = new Vector2(5 + (w * b.SpriteTexture.Width + (w * margin)), Game.GraphicsDevice.Viewport.Height / 2 + b.SpriteTexture.Height - (h * b.SpriteTexture.Height + (h * margin)));
                    Blocks.Add(b);
                    ++winScore;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            CheckBlocksForCollision(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Removes disabled blocks from list
        /// </summary>


        private void CheckBlocksForCollision(GameTime gameTime)
        {
            for (int i = Blocks.Count() - 1; i >= 0; i--)
            {
                Block block = Blocks[i];
                for (int j = ballManager.Balls.Count() - 1; j >= 0; j--)
                {
                    Ball ball = ballManager.Balls[j];
                    if (block.Intersects(ball)) //chek rectagle collision between ball and current block 
                    {
                        //checks hit
                        block.HitByBall(ball);

                        if (!ball.Reflected) //only reflect once
                        {
                            ball.ReflectFromBlock(block);
                        }
                    }
                    if (block.State == BlockState.Broken)
                    {
                        RemoveDisabledBlocks(block);
                        break;
                    }
                }
                block.Update(gameTime);
            }
        }
        /// <summary>
        /// remove if blockstate is broken
        /// </summary>
        /// <param name="ball"></param>
        private void RemoveDisabledBlocks(Block block)
        {
            //remove disabled blocks

            TeamColor team = block.ScoredTeam;
            IItem spawnedItem = block.SpawnItem();
            if (spawnedItem != null)
                itemManager.AddItem(spawnedItem);
            Blocks.Remove(block);
            ScoreManager.Score[(int)team]++;
            currentScore++;

        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var block in this.Blocks)
            {
                block.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        /** TODO: Add Random Block Generation */
        private Block GetRandomBlock()
        {
            int blockCase = caseQueue.Dequeue() % 3;
            switch (blockCase)
            {
                case 0:
                    return new PoopBlock(Game);
                default:
                    if (blockCase % 2 == 0)
                        return new BabyBlock(Game);
                    else
                        return new BallBlock(Game, ballManager.BallBlockBrokenDelegate);
            }
        }
    }
}
