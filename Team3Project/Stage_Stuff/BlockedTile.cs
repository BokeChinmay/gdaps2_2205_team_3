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
        private Texture2D texture;

        // Parameterized constructor
        public BlockedTile(int xPos, int yPos, Texture2D texture) : base(114, 100, xPos, yPos)
        {
            this.texture = texture;
            isObstruction = true;
        }

        // Draw method override
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(texture, dimensions, Color.White);
        }
    }
}
