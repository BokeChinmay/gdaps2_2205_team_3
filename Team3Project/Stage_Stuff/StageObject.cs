﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Name: StageObject
//Purpose: Parent class for objects that make up the layout of the stage

namespace Team3Project.Stage_Stuff
{
    internal abstract class StageObject : IObstruction
    {
        // Field declarations
        protected Rectangle dimensions;
        protected bool isObstruction;

        // Read-only property for dimensions
        public Rectangle Dimensions { get { return dimensions; } }

        // Properties required by IObstruction
        public int TopEdge { get { return dimensions.Top; } }
        public int LeftEdge { get { return dimensions.Left; } }
        public int RightEdge { get { return dimensions.Right; } }
        public virtual int BottomEdge { get { return dimensions.Bottom; } set { } }
        public bool IsObstruction { get { return isObstruction; } }

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
