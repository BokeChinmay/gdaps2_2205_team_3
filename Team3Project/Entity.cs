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
        protected bool topBlocked;
        protected bool bottomBlocked;
        protected bool leftBlocked;
        protected bool rightBlocked;

        // Read-and-write properties
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

        // Properties related to stage objects
        public bool TopBlocked 
        { 
            get { return topBlocked; }
            set { topBlocked = value; }
        }
        public bool BottomBlocked
        {
            get { return bottomBlocked; }
            set { bottomBlocked = value; }
        }
        public bool LeftBlocked
        {
            get { return leftBlocked; }
            set { leftBlocked = value; }
        }
        public bool RightBlocked
        {
            get { return rightBlocked; }
            set { rightBlocked = value; }
        }

        // Parameterized constructor
        public Entity(int health, int moveSpeed, Rectangle collision)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            this.Collision = collision;
            active = true;
            topBlocked = false;
            bottomBlocked = false;
            leftBlocked = false;
            rightBlocked = false;
        }

        /// <summary>
        /// All entities should have the ability to move
        /// </summary>
        protected abstract void Move();

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
