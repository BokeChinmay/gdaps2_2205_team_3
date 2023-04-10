using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal class Projectile : Entity
    {
        //Texture
        Texture2D texture;

        //Damage that the projectile deals
        protected int damage;

        //Property for the damage field
        public int Damage
        {
            get { return damage; }
        }

        // Parameterized constructor to establish a projectile's speed and velocity
        public Projectile(int speed, int damage, Rectangle collision, Texture2D texture) : base(1, speed, collision)
        {
            this.damage = damage;
            this.texture = texture;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(texture, collision, Color.White);
        }
    }
} 
