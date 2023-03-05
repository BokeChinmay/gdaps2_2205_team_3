using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal class RangedEnemy : Enemy, IDamageable
    {
        // Class-specific fields
        int attackDelay;
        int projectileSpeed;

        // Parameterized constructor
        public RangedEnemy(int health, int moveSpeed, int attackDelay, int projectileSpeed) : base(health, moveSpeed)
        {
            this.attackDelay = attackDelay;
            this.projectileSpeed = projectileSpeed;
        }

        protected override void Move()
        {
            
        }

        public void TakeDamage(int amount)
        {

        }
    }
}
