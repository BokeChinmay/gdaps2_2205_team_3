using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Stage_Stuff
{
    internal abstract class StageObject
    {
        // Field declarations
        protected Rectangle dimensions;

        // Read-only property for dimensions
        public Rectangle Dimensions { get { return dimensions; } }

        // Parameterized constructor
        public StageObject(int width, int height, int xPos, int yPos)
        {
            dimensions = new Rectangle(xPos, yPos, width, height);
        }

        /// <summary>
        /// Stage objects need draw methods
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public abstract void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects);
    }
}
