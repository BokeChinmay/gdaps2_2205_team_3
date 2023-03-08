using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal abstract class Entity
    {
        // Field declarations
        protected int health;
        protected int moveSpeed;
        protected bool active;

        // Read-and-write properties for health and move speed
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public int MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        // Parameterized constructor
        public Entity(int health, int moveSpeed)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            active = true;
        }

        /// <summary>
        /// All entities should have the ability to move
        /// </summary>
        protected abstract void Move();

        /// <summary>
        /// All entities should be drawn.
        /// </summary>
        protected abstract void Draw();
    }
}
