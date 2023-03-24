using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    enum MeleeEnemyState
    {
        Idle,
        Moving,
        Telegraphing,
        Attacking,
        Recovering,
    }

    internal class MeleeEnemy : Enemy, IDamageable
    {
        MeleeEnemyState currentState;
        VulnerabilityState vulnerabilityState;
        //Range at which the enemy begins the attack process
        const int AGGRO_RANGE = 200;
        //Range at which attack begins
        const int ATTACK_RANGE = 60;
        //Duration of attack
        const int ATTACK_DURATION = 30;
        //Duration of recovery
        const int RECOVERY_DURATION = 30;
        //Duration of invincibility
        const int INVINCIBILITY_DURATION = 10;

        //# of frames between telegraphing and attacking
        int attackDelay;
        //Attack timer, is changed per frame
        int attackTimer;
        //Direction of the attack
        Vector2 attackDirection;

        //Invincibility timer, designates the number of invincibility frames left
        int invincibilityTimer;

        public MeleeEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay) : base(health, moveSpeed, collision)
        {
            currentState = MeleeEnemyState.Idle;
            vulnerabilityState = VulnerabilityState.Vulnerable;
            this.attackDelay = attackDelay;
        }

        //Name: Move
        //Purpose: Does nothing
        //Params: None
        public override void Move()
        {
            
        }

        //Name: Attack
        //Purpose: Lunges enemy in a specific direction
        //Params: Vector 2 containing unit vector in desired direction
        public void Attack(Vector2 direction)
        {
            collision.X = (int)(direction.X * moveSpeed * 2);
            collision.Y = (int)(direction.Y * moveSpeed * 2);
        }

        public override void Update() 
        {
            
            throw new NotImplementedException();
        }

        public override void Update(Rectangle playerCollision, List<Projectile> projectileList)
        {
            //Vector 2 of player position
            Vector2 playerPos = new Vector2((int)playerCollision.X, (int)playerCollision.Y);

            //STATE MACHINE - Movement/attacking state
            switch (currentState)
            {
                //Idle - Check if player position is within aggro range
                case MeleeEnemyState.Idle:
                    if (DistanceFromPlayer(playerPos) < AGGRO_RANGE)
                    {
                        currentState = MeleeEnemyState.Moving;
                    }
                    break;
                //Moving - Move directly toward player until in attacking range
                case MeleeEnemyState.Moving:
                    MoveTowardPos(playerPos);
                    if (DistanceFromPlayer(playerPos) < ATTACK_RANGE)
                    {
                        attackTimer = attackDelay;
                        attackDirection = DirectionToPlayer(playerPos);
                        currentState = MeleeEnemyState.Telegraphing;
                    }
                    break;
                //Telegraphing - wait attackSpeed # of frames before attacking
                case MeleeEnemyState.Telegraphing:
                    attackTimer--;
                    if (attackTimer <= 0)
                    {
                        attackTimer = ATTACK_DURATION;
                        currentState = MeleeEnemyState.Attacking;
                    }
                    break;
                //Attacking - Move quickly in one direction for a few frames. After attack, change to idle
                case MeleeEnemyState.Attacking:
                    attackTimer--;
                    Attack(attackDirection);
                    if (attackTimer <= 0)
                    {
                        attackTimer = RECOVERY_DURATION;
                        currentState = MeleeEnemyState.Recovering;
                    }
                    break;
                //Recovering - A few frames of cooldown after the attack but before returning to idle state
                case MeleeEnemyState.Recovering:
                    attackTimer--;
                    if (attackTimer <= 0)
                    {
                        currentState = MeleeEnemyState.Idle;
                    }
                    break;
            }

            //STATE MACHINE - Vulnerability state
            switch (vulnerabilityState)
            {
                //Enemy is vulnerable - check if collision is intersecting a projectile.
                //If so, take damage and become invincible
                case VulnerabilityState.Vulnerable:
                    foreach (Projectile projectile in projectileList)
                    {
                        if (collision.Intersects(projectile.Collision))
                        {
                            TakeDamage(projectile.Damage);
                            invincibilityTimer = INVINCIBILITY_DURATION;
                            vulnerabilityState = VulnerabilityState.Invincible;
                        }
                    }
                    break;
                //Enemy is invincible - cannot take damage for a few frames.
                //When timer hits 0, return to vulnerable state
                case VulnerabilityState.Invincible:
                    invincibilityTimer--;
                    if (invincibilityTimer <= 0)
                    {
                        vulnerabilityState = VulnerabilityState.Vulnerable;
                    }
                    break;
                    
            }

        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            throw new NotImplementedException();
        }
    }
}
