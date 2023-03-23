using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    enum RangedEnemyState
    {
        Idle,
        Move,
        Attack,
        Recovery
    }

    internal class RangedEnemy : Enemy, IDamageable
    {
        // Class-specific fields
        int attackDelay;
        int projectileSpeed;

        // Parameterized constructor
        public RangedEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay, int projectileSpeed) : base(health, moveSpeed, collision)
        {
            this.attackDelay = attackDelay;
            this.projectileSpeed = projectileSpeed;
        }

        public override void Move()
        {

        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void Update(Rectangle playerCollision, List<Projectile> projectileList)
        {
            //Check if in range of player

            //If too close to the player, move away

            //Check attack delay, attack

            //Check for damage

        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new NotImplementedException();
        }
    }
}
