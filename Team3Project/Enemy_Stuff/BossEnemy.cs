using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    internal class BossEnemy : MeleeEnemy
    {
        //Constructor
        public BossEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay, Texture2D texture) : base(health, moveSpeed, collision, attackDelay, texture)
        {

        }

        //Override draw method
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
                texture,
                new Vector2(collision.X - 45, collision.Y - 45),
                new Rectangle(
                    HORIZONTAL_BUFFER * (frame - 1),
                    VERTICAL_BUFFER,
                    RectWidth,
                    RectHeight),
                Color.White,
                0,
                Vector2.Zero,
                9.0f,
                flipsprite,
                0
                );
        }
    }
}
