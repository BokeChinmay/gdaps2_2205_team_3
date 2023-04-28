using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal class MeleeProjectile : Projectile
    {
        //Fields
        float rotation;
        int timer;

        //Constructor
        public MeleeProjectile(int speed, int damage, Rectangle collision, Texture2D texture, bool friendly, float rotation) : base(speed, damage, collision, texture, friendly)
        {
            this.rotation = rotation;
            timer = 15;
        }

        //Methods
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
                texture,
                new Vector2(collision.X, collision.Y),
                collision,
                Color.White,
                rotation,
                new Vector2(collision.X - (collision.Width/2), collision.Y - (collision.Y/2)),
                1.0f,
                spriteEffects,
                0
                );
        }

        public override void Update()
        {
            if (timer < 0)
            {
                active = false;
            }
            timer--;
        }
    }
}
