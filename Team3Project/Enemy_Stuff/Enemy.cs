using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Class: Enemy
//Purpose: Parent class for enemy classes. Contains helpful methods and enums that are used in multiple different enemy classes.

namespace Team3Project.Enemy_Stuff
{
    //Enum for possible vulnerability states
    enum VulnerabilityState
    {
        Vulnerable,
        Invincible
    }

    //Enum for enemy AI states
    enum EnemyState
    {
        Idle,
        Moving,
        Telegraphing,
        Attacking,
        Recovering,
        Hurt,
        Death
    }

    internal abstract class Enemy : Entity
    {
        //Texture/Sprite sheet
        protected Texture2D texture;

        //Vulnerability/Invincibility fields and consts
        protected VulnerabilityState vulnerabilityState;
        //Duration of invincibility
        protected const int INVINCIBILITY_DURATION = 30;
        //Invincibility timer, designates the number of invincibility frames left
        protected int invincibilityTimer;

        //Type of enemy, according to enum
        protected EnemyTypes type;
        public EnemyTypes Type
        {
            get { return type; }
        }

        public Enemy(int health, int moveSpeed, Rectangle collision, Texture2D texture) : base(health, moveSpeed, collision)
        {
            this.health = health;
            this.moveSpeed = moveSpeed;
            this.collision = collision;
            this.texture = texture;
        }

        //Update method
        public abstract void Update(Rectangle playerCollision, List<Projectile> projectileList, GameTime gameTime);

        //Name: MoveTowardPos
        //Purpose: Moves entity one speed unit directly toward a Vector
        //Params: Vector containing X and Y positions of the target position.
        public void MoveTowardPos(Vector2 targetPos)
        {
            //Calculate unit vector
            Vector2 displacement = targetPos - new Vector2(collision.X, collision.Y);
            float distance = DistanceFromPlayer(targetPos);
            Vector2 unitVector = displacement / distance;

            //Multiply by speed and move
            int yMotion = (int)(unitVector.Y * moveSpeed);
            int xMotion = (int)(unitVector.X * moveSpeed);
            //Check if y motion should be applied
            if (yMotion < 0 && topBlocked)
            {
                yMotion = 0;
                if (xMotion != 0)
                {
                    xMotion = (xMotion / Math.Abs(xMotion)) * moveSpeed;
                }
            }
            else if (yMotion > 0 && bottomBlocked)
            {
                yMotion = 0;
                if (xMotion != 0)
                {
                    xMotion = (xMotion / Math.Abs(xMotion)) * moveSpeed;
                }
            }           

            //Check if x motion should be applied
            if (xMotion < 0 && leftBlocked)
            {
                xMotion = 0;
                if (yMotion != 0)
                {
                    yMotion = (yMotion / Math.Abs(yMotion)) * moveSpeed;
                }
            }
            else if (xMotion > 0 && rightBlocked)
            {
                xMotion = 0;
                if (yMotion != 0)
                {
                    yMotion = (yMotion / Math.Abs(yMotion)) * moveSpeed;
                }
            }
            collision.X += xMotion;
            collision.Y += yMotion;
        }

        //Overload that can be passed a new speed that is different from the field moveSpeed
        public void MoveTowardPos(Vector2 targetPos, int newSpeed)
        {
            //Calculate unit vector
            Vector2 displacement = targetPos - new Vector2(collision.X, collision.Y);
            float distance = DistanceFromPlayer(targetPos);
            Vector2 unitVector = displacement / distance;

            //Multiply by speed and move
            int yMotion = (int)(unitVector.Y * newSpeed);
            int xMotion = (int)(unitVector.X * newSpeed);

            //Check if y motion should be applied
            if (yMotion < 0 && topBlocked)
            {
                yMotion = 0;
                if (xMotion != 0)
                {
                    xMotion = (xMotion / Math.Abs(xMotion)) * newSpeed;
                }
            }
            else if (yMotion > 0 && bottomBlocked)
            {
                yMotion = 0;
                if (xMotion != 0)
                {
                    xMotion = (xMotion / Math.Abs(xMotion)) * newSpeed;
                }
            }          

            //Check if x motion should be applied           
            if (xMotion < 0 && leftBlocked)
            {
                xMotion = 0;
                if (yMotion != 0)
                {
                    yMotion = (yMotion / Math.Abs(yMotion)) * newSpeed;
                }
            }
            else if (xMotion > 0 && rightBlocked)
            {
                xMotion = 0;
                if (yMotion != 0)
                {
                    yMotion = (yMotion / Math.Abs(yMotion)) * newSpeed;
                }
            }
            collision.X += xMotion;
            collision.Y += yMotion;
        }

        /// <summary>
        /// Similar to MoveTowardPos, except moves in the opposite direction
        /// </summary>
        /// <param name="targetPos"></param>
        public void MoveAwayFromPos(Vector2 targetPos)
        {
            //Calculate unit vector
            Vector2 displacement = targetPos - new Vector2(collision.X, collision.Y);
            float distance = DistanceFromPlayer(targetPos);
            Vector2 unitVector = displacement / distance;

            //Multiply by speed and move
            int yMotion = -(int)(unitVector.Y * moveSpeed);
            int xMotion = -(int)(unitVector.X * moveSpeed);

            //Check if y motion should be applied
            if (yMotion < 0 && topBlocked)
            {
                yMotion = 0;
                if (xMotion != 0)
                {
                    xMotion = (xMotion / Math.Abs(xMotion)) * moveSpeed;
                }
            }
            else if (yMotion > 0 && bottomBlocked)
            {
                yMotion = 0;
                if (xMotion != 0)
                {
                    xMotion = (xMotion / Math.Abs(xMotion)) * moveSpeed;
                }
            }

            //Check if x motion should be applied
            if (xMotion < 0 && leftBlocked)
            {
                xMotion = 0;
                if (yMotion != 0)
                {
                    yMotion = (yMotion / Math.Abs(yMotion)) * moveSpeed;
                }
            }
            else if (xMotion > 0 && rightBlocked)
            {
                xMotion = 0;
                if (yMotion != 0)
                {
                    yMotion = (yMotion / Math.Abs(yMotion)) * moveSpeed;
                }
            }
            collision.Y += yMotion;
            collision.X += xMotion;
        }

        //Name: DistanceFromPlayer
        //Purpose: Determines an enemy's distance from the player
        //Params: Vector containing X and Y positions of the player
        public float DistanceFromPlayer(Vector2 playerPos)
        {
            //Find displacement vector
            Vector2 displacement = playerPos - new Vector2(collision.X, collision.Y);

            //Determine distance using distance formula
            float distance = (float)Math.Sqrt(Math.Pow(displacement.X, 2) + Math.Pow(displacement.Y, 2));
            return distance;
        }
        
        //Name: Direction to player
        //Purpose: Gives unit vector in the direction from the enemy to the player
        //Params: Vector containing X and Y positions of the player
        public Vector2 DirectionToPlayer(Vector2 playerPos)
        {
            //Calculate unit vector
            Vector2 displacement = playerPos - new Vector2(collision.X, collision.Y);
            float distance = DistanceFromPlayer(playerPos);
            Vector2 unitVector = displacement / distance;
            return unitVector;
        }

        //Name: Take Damage
        //Purpose: Changes health by damage amount
        //Params: Damage amount
        public void TakeDamage(Projectile bullet)
        {
            if (0 < bullet.Damage)
            {
                health -= bullet.Damage;
            }
            else
            {
                health -= 1;
            }
        }

        /// <summary>
        /// Creates an enemy bullet in the direction of the player
        /// </summary>
        /// <param name="bulletSpeed">Speed of the projectile</param>
        /// <param name="playerPos">Position vector of the player</param>
        /// <param name="bulletDamage">Damage of the bullet</param>
        /// <param name="pTexture">Bullet texture</param>
        public void Shoot(int bulletSpeed, Vector2 playerPos, int bulletDamage, Texture2D pTexture)
        {
            LevelManager.AddProjectile(new EnemyBullet(bulletSpeed, DirectionToPlayer(playerPos).X, DirectionToPlayer(playerPos).Y, new Rectangle(collision.X, collision.Y, 20, 20), bulletDamage, pTexture));
        }

        /// <summary>
        /// Basic draw method. Not useful for spritesheets
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(texture, collision, Color.White);
        }
    }
}
