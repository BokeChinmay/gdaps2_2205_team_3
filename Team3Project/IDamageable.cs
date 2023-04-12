using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Name: IDamageable
// Purpose: defines a health property and takedamage method necessary for an object to be able
//          to be damaged and destroyed

namespace Team3Project
{
    internal interface IDamageable
    {
        // Allows health to be viewed by something else, if necessary
        public int Health { get; }

        // Will allow health to be reduced, and will make an object inactive when it hits 0
        public void TakeDamage(int amount);
    }
}
