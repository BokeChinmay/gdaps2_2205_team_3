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
    internal class EnemyBullet : Projectile
    {
        // A vector for the projectile's direction
        private Vector2 direction;

        public EnemyBullet(int speed, float xDirection, float yDirection, Rectangle collision, int damage) : base(1, speed, collision)
        {
            direction = new Vector2(xDirection * speed, yDirection * speed);
            validTargets = new List<Entity>();
            this.damage = damage;
        }

        public void Move()
        {
            //Update position by direction vector
            collision.X += (int)direction.X;
            collision.Y += (int)direction.Y;
        }

        public override void Update()
        {
            Move();
        }

    }
}
