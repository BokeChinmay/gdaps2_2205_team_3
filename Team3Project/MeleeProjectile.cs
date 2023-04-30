using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Name: Melee Projectile
//Purpose: A projectile for the player's melee attack. Does not move in a straight line like a bullet
//Restrictions: Inherits from Projectile

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
        /// <summary>
        /// Overriden draw method
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
                texture,
                new Vector2(collision.X, collision.Y),
                null,
                Color.White,
                rotation,
                new Vector2(20, 50),
                0.5f,
                spriteEffects,
                0
                );
        }

        /// <summary>
        /// Overriden update method
        /// </summary>
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
