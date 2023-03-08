using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal abstract class Enemy : Entity
    {
        public Enemy(int health, int moveSpeed, Rectangle collision) : base(health, moveSpeed, collision)
        {

        }
    }
}
