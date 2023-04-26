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
        private int level;

        // Property for level
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        // Parameterized constructor
        public ScoreDisplay() : base (1372, 35, 76, 32)
        {
        }

        /// <summary>
        /// Updates the level display based on the value that the player has stored
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            level = player.Level;
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
            stageObjectManager.DrawUINumber(level / 100, new Vector2(dimensions.X + 4, dimensions.Y + dimensions.Height + 8), spriteBatch);
            stageObjectManager.DrawUINumber(level / 10, new Vector2(dimensions.X + 28, dimensions.Y + dimensions.Height + 8), spriteBatch);
            stageObjectManager.DrawUINumber(level % 10, new Vector2(dimensions.X + 52, dimensions.Y + dimensions.Height + 8), spriteBatch);
        }
    }
}
