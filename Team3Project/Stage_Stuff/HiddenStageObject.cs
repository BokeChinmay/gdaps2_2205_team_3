using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Name: HiddenStageObject
//Purpose: Create offscreen game objects, in order to keep entities
//         within the bounds of the screen

namespace Team3Project.Stage_Stuff
{
    internal class HiddenStageObject : StageObject
    {       
        // Parameterized constructor
        public HiddenStageObject(int width, int height, int xPos, int yPos) 
            : base (width, height, xPos, yPos)
        {
            isObstruction = true;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects){}
    }
}
