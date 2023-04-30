using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Player_Stuff;

//Name: ScoreDisplay
//Purpose: Displays the current level on the top right of the screen
//Inherits from: UIObject

namespace Team3Project.Stage_Stuff
{
    internal class ScoreDisplay : UIObject
    {
        // Field declarations
        private int value;
        private bool level;

        // Property for level
        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        // Parameterized constructor
        public ScoreDisplay(bool level) : base (1372, 35, 76, 32)
        {
            this.level = level;

            if (!level)
            {
                dimensions.X = 1352;
                dimensions.Y = 120;
                dimensions.Width = 116;
                dimensions.Height = 24;
            }
        }

        /// <summary>
        /// Updates the level display based on the value that the player has stored
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            if (level)
            {
                value = player.Level;
            }
            else
            {
                value = player.Score;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Draws the display
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, StageObjectManager stageObjectManager, Texture2D label)
        {
            spriteBatch.Draw(label, new Vector2(dimensions.X, dimensions.Y), Color.White);

            if (level)
            {
                if (value <= 999)
                {
                    stageObjectManager.DrawUINumber(value / 100, new Vector2(dimensions.X + 4, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber((value % 100) / 10, new Vector2(dimensions.X + 28, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber(value % 10, new Vector2(dimensions.X + 52, dimensions.Y + dimensions.Height + 8), spriteBatch);
                }
                else
                {
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X + 4, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X + 28, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X + 52, dimensions.Y + dimensions.Height + 8), spriteBatch);
                }
            }
            else
            {
                if (value <= 99999)
                {
                    if (value >= 10000)
                    {
                        stageObjectManager.DrawUINumber(value / 10000, new Vector2(dimensions.X, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }
                    else
                    {
                        stageObjectManager.DrawUINumber(0, new Vector2(dimensions.X, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }

                    if (value >= 1000)
                    {
                        stageObjectManager.DrawUINumber((value % 10000) / 1000, new Vector2(dimensions.X + 24, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }
                    else
                    {
                        stageObjectManager.DrawUINumber(0, new Vector2(dimensions.X + 24, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }

                    if (value >= 100)
                    {
                        stageObjectManager.DrawUINumber((value % 1000) / 100, new Vector2(dimensions.X + 48, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }
                    else
                    {
                        stageObjectManager.DrawUINumber(0, new Vector2(dimensions.X + 48, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }

                    if (value >= 10)
                    {
                        stageObjectManager.DrawUINumber((value % 100) / 10, new Vector2(dimensions.X + 72, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }
                    else
                    {
                        stageObjectManager.DrawUINumber(0, new Vector2(dimensions.X + 72, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    }

                    stageObjectManager.DrawUINumber(value % 10, new Vector2(dimensions.X + 96, dimensions.Y + dimensions.Height + 8), spriteBatch);
                }
                else
                {
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X + 24, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X + 48, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X + 72, dimensions.Y + dimensions.Height + 8), spriteBatch);
                    stageObjectManager.DrawUINumber(9, new Vector2(dimensions.X + 96, dimensions.Y + dimensions.Height + 8), spriteBatch);
                }
            }
        }
    }
}
