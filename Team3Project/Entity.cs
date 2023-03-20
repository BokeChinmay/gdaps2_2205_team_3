using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        protected Rectangle collision;

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

        public bool Active 
        { 
            get { return active; } 
            set { active = value; } 
        }

        public Rectangle Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        // Parameterized constructor
        public Entity(int health, int moveSpeed, Rectangle collision)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            this.Collision = collision;
            active = true;
        }

        /// <summary>
        /// All entities should have the ability to move
        /// </summary>
        public abstract void Move();

        /// <summary>
        /// All entities must be updated.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// All entitites must be drawn.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public abstract void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects);

    }
}
