﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Stage_Stuff
{
    internal class VisualBarrier : StageObject
    {
        // Field declarations
        private Texture2D texture;

        // This will be used exclusively for the back wall
        private int bottomEdgeOverride;

        public override int BottomEdge
        {
            get { return bottomEdgeOverride; }
            set { bottomEdgeOverride = value; }
        }

        // Parameterized constructor
        public VisualBarrier(int xPos, Texture2D bufferTexture, bool backWall)
            : base(bufferTexture.Width, bufferTexture.Height, xPos, 0)
        {
            texture = bufferTexture;
            isObstruction = true;
            bottomEdgeOverride = xPos + bufferTexture.Height;

            if (backWall)
            {
                bottomEdgeOverride = 76;
            }
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