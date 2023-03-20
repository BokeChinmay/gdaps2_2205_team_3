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
            //Update position by direction vector
            collision.X += (int)direction.X;
            collision.Y += (int)direction.Y;
        }

        //Name: IsColliding
        //Purpose: Checks if the projectile is colliding with any valid target
        //Params: None
        //*****Note: I am not sure how the projectile will actually get this list every frame,
        //           maybe this could be done a different way?
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
    }
}
