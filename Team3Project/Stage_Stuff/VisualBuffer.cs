using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Stage_Stuff
{
    internal class VisualBuffer : StageObject
    {
        // Field declaration
        private Texture2D texture;
        
        // Parameterized constructor
        public VisualBuffer(int xPos, Texture2D bufferTexture)
            : base(bufferTexture.Width, bufferTexture.Height, xPos, 0)
        { 
            texture = bufferTexture;
            isObstruction = true;
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(texture, dimensions, Color.White);
        }
    }
}
