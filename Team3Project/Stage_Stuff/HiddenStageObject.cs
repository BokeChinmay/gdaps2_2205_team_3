﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Stage_Stuff
{
    internal class HiddenStageObject : StageObject
    {
        // The purpose of this class is to create offscreen game objects, in order to keep entities
        // within the bounds of the screen
        
        public HiddenStageObject(int width, int height, int xPos, int yPos) 
            : base (width, height, xPos, yPos)
        {
            isObstruction = true;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects){}

        // This method will set an entity's position to be within bounds if they somehow clip out
        public void ShoveEntity(Entity entity)
        {
            
        }
    }
}
