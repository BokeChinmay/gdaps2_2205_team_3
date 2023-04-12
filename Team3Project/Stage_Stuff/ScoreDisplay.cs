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
        private SpriteFont font;
        private int level;

        // Property for level
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        // Parameterized constructor
        public ScoreDisplay(SpriteFont font) : base (1700, 75, 135, 740)
        {
            this.font = font;
        }

        /// <summary>
        /// Updates the level display based on the value that the player has stored
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            level = player.Level;
        }

        /// <summary>
        /// Draws the display
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.DrawString(font, $"Level: {level}", new Vector2(1355, 25), Color.LightGreen);
        }
    }
}
