using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

//Name: BlockedTile
//Purpose: Class for tiles that can't be moved across by players, enemies, or projectiles
//Restrictions: Inherits from StageObject

namespace Team3Project.Stage_Stuff
{
    internal class BlockedTile : StageObject
    {
        // Field declaration
        private Texture2D topTexture;
        private Texture2D bottomTexture;
        private Texture2D basicTexture;
        private bool basic;

        // Property for whether the tile is basic
        public bool Basic
        {
            get { return basic; }
        }

        // Parameterized constructor
        public BlockedTile(int xPos, int yPos, Texture2D topTexture, Texture2D bottomTexture, Texture2D basicTexture, bool basic) : base(114, 100, xPos, yPos)
        {
            this.topTexture = topTexture;
            this.bottomTexture = bottomTexture;
            this.basicTexture = basicTexture;
            this.basic = basic;
            isObstruction = true;
        }

        // Draw method override
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(basicTexture, new Rectangle(dimensions.X, dimensions.Y - 20, dimensions.Width, dimensions.Height - 10), Color.White);
        }

        // Draws a lower layer
        public void DrawBottom(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(bottomTexture, new Rectangle(dimensions.X, dimensions.Y + 30, bottomTexture.Width, bottomTexture.Height), Color.White);
        }

        // Draws a second layer
        public void DrawTop(SpriteBatch spriteBatch, SpriteEffects spriteEffects) 
        {
            spriteBatch.Draw(basicTexture, new Rectangle(dimensions.X, dimensions.Y - 30, dimensions.Width, dimensions.Height - 10), Color.White);
        }
    }
}
