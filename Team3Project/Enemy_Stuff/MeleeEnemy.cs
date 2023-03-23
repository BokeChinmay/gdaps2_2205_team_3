using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    enum EnemyState
    {
        Idle,
        Moving,
        Telegraphing,
        Attacking,
        Recovering,
    }

    internal class MeleeEnemy : Enemy, IDamageable
    {
        EnemyState currentState;
        //Range at which the enemy begins the attack process
        const int AGGRO_RANGE = 200;
        //Range at which attack begins
        const int ATTACK_RANGE = 60;
        //Duration of attack
        const int ATTACK_DURATION = 30;
        //Duration of recovery
        const int RECOVERY_DURATION = 30;

        //# of frames between telegraphing and attacking
        int attackDelay;
        //Attack timer, is changed per frame
        int attackTimer;
        //Direction of the attack
        Vector2 attackDirection;

        public MeleeEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay) : base(health, moveSpeed, collision)
        {
            currentState = EnemyState.Idle;
            this.attackDelay = attackDelay;
        }

        public override void Move()
        {
            
        }

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
            collision.X += (int)(unitVector.X * moveSpeed);
            collision.Y += (int)(unitVector.Y * moveSpeed);
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

        //Name: Attack
        //Purpose: Lunges enemy in a specific direction
        //Params: Vector 2 containing unit vector in desired direction
        public void Attack(Vector2 direction)
        {
            collision.X = (int)(direction.X * moveSpeed * 2);
            collision.Y = (int)(direction.Y * moveSpeed * 2);
        }

        public void TakeDamage(int amount)
        {
            health -= amount;
        }

        public override void Update() 
        {
            
            throw new NotImplementedException();
        }

        public void Update(Rectangle playerCollision, List<Projectile> projectileList)
        {
            //Vector 2 of player position
            Vector2 playerPos = new Vector2((int)playerCollision.X, (int)playerCollision.Y);

            //STATE MACHINE
            switch (currentState)
            {
                //Idle - Check if player position is within aggro range
                case EnemyState.Idle:
                    if (DistanceFromPlayer(playerPos) < AGGRO_RANGE)
                    {
                        currentState = EnemyState.Moving;
                    }
                    break;
                //Moving - Move directly toward player until in attacking range
                case EnemyState.Moving:
                    MoveTowardPos(playerPos);
                    if (DistanceFromPlayer(playerPos) < ATTACK_RANGE)
                    {
                        attackTimer = attackDelay;
                        attackDirection = DirectionToPlayer(playerPos);
                        currentState = EnemyState.Telegraphing;
                    }
                    break;
                //Telegraphing - wait attackSpeed # of frames before attacking
                case EnemyState.Telegraphing:
                    attackTimer--;
                    if (attackTimer <= 0)
                    {
                        attackTimer = ATTACK_DURATION;
                        currentState = EnemyState.Attacking;
                    }
                    break;
                //Attacking - Move quickly in one direction for a few frames. After attack, change to idle
                case EnemyState.Attacking:
                    attackTimer--;
                    Attack(attackDirection);
                    if (attackTimer <= 0)
                    {
                        attackTimer = RECOVERY_DURATION;
                        currentState = EnemyState.Recovering;
                    }
                    break;
                //Recovering - A few frames of cooldown after the attack but before returning to idle state
                case EnemyState.Recovering:
                    attackTimer--;
                    if (attackTimer <= 0)
                    {
                        currentState = EnemyState.Idle;
                    }
                    break;
            }

            //Take damage
            foreach (Projectile projectile in projectileList)
            {
                if (this.collision.Intersects(projectile.Collision))
                {

                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new NotImplementedException();
        }
    }
}
