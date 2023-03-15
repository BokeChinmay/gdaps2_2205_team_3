using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    internal class Projectile : Entity
    {
        // A vector for the projectile's direction
        private Vector2 direction;

        // The targets that a projectile is able to damage
        private List<Entity> validTargets;

        // Parameterized constructor to establish a projectile's speed and velocity
        public Projectile(int speed, int xDirection, Rectangle collision, int yDirection) : base(1, speed, collision)
        {
            direction = new Vector2(xDirection * speed, yDirection * speed);
            validTargets = new List<Entity>();
        }

        protected override void Move()
        {

        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new NotImplementedException();
        }
    }
}
