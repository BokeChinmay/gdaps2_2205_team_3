using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal class MeleeEnemy : Enemy, IDamageable
    {
        public MeleeEnemy(int health, int moveSpeed) : base (health, moveSpeed)
        {
            
        }

        protected override void Move()
        {
            
        }

        public void TakeDamage(int amount)
        {

        }
    }
}
