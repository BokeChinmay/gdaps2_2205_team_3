using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Name: UIObject
//Purpose: Parent class for features of the UI, like the health bar and score

namespace Team3Project.Stage_Stuff
{
    internal abstract class UIObject
    {
        // Fields
        protected Rectangle dimensions;

        // Parameterized constructor
        public UIObject(int xPos, int yPos, int width, int height)
        {
            dimensions = new Rectangle(xPos, yPos, width, height);
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public abstract void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects);
    }
}
