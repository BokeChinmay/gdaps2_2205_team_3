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
        // The targets that a projectile is able to damage
        protected List<Entity> validTargets;

        //Damage that the projectile deals
        protected int damage;

        //Property for the damage field
        public int Damage
        {
            get { return damage; }
        }

        // Parameterized constructor to establish a projectile's speed and velocity
        public Projectile(int speed, int damage, Rectangle collision) : base(1, speed, collision)
        {
            validTargets = new List<Entity>();
            this.damage = damage;
        }

        //Name: IsColliding
        //Purpose: Checks if the projectile is colliding with any valid target
        //Params: None
        public bool IsColliding()
        {
            for (int i = 0; i < validTargets.Count; i++)
            {
                if (collision.Intersects(validTargets[i].Collision))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects, Texture2D texture)
        {
            spriteBatch.Draw(texture, collision, Color.White);
        }
    }
}
