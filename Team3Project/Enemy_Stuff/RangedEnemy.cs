using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project.Enemy_Stuff
{
    enum RangedEnemyState
    {
        Idle,
        Move,
        Telegraphing,
        Attack,
        Recovering,
    }

    internal class RangedEnemy : Enemy, IDamageable
    {
        // Class-specific fields
        int attackDelay;
        int projectileSpeed;
        RangedEnemyState currentState;

        //Constant bullet speed
        const int BULLET_SPEED = 2;
        //Constant bullet damage
        const int BULLET_DAMAGE = 50;
        
        //Range at which the attack sequence begins
        const int AGGRO_RANGE = 200;
        //Range at which the enemy runs away from the player
        const int ESCAPE_RANGE = 50;

        //Frame timer for telegraph/recovery
        int attackTimer;

        // Parameterized constructor
        public RangedEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay, int projectileSpeed) : base(health, moveSpeed, collision)
        {
            this.attackDelay = attackDelay;
            this.projectileSpeed = projectileSpeed;
            currentState = RangedEnemyState.Idle;
            type = EnemyTypes.Ranged;
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public override void Move()
        {

        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Update()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates once per frame, called once per Game1's Update()
        /// </summary>
        /// <param name="playerCollision"></param>
        /// <param name="projectileList"></param>
        public override void Update(Rectangle playerCollision, List<Projectile> projectileList)
        {
            //Player position vector
            Vector2 playerPos = new Vector2((int)playerCollision.X, (int)playerCollision.Y);

            switch (currentState)
            {
                //Wait until player is within aggro range. If so, begin move.
                case RangedEnemyState.Idle:
                    if (DistanceFromPlayer(playerPos) < AGGRO_RANGE)
                    {
                        currentState = RangedEnemyState.Move;
                    }
                    break;
                //Move if too close to the player. Otherwise, begin telegraphing.
                case RangedEnemyState.Move:
                    if (DistanceFromPlayer(playerPos) < ESCAPE_RANGE)
                    {
                        MoveTowardPos(-1 * playerPos);
                    }
                    else
                    {
                        attackTimer = attackDelay;
                        currentState = RangedEnemyState.Telegraphing;
                    }
                    break;
                //Begin telegraph delay. After timer reaches 0, begin attack
                case RangedEnemyState.Telegraphing:
                    attackTimer--;
                    if (attackTimer <= 0)
                    {
                        currentState = RangedEnemyState.Attack;
                    }
                    break;
                //Create a bullet and move to recovery state
                case RangedEnemyState.Attack:
                    LevelManager.AddProjectile(new EnemyBullet(BULLET_SPEED, DirectionToPlayer(playerPos).X, DirectionToPlayer(playerPos).Y, new Rectangle(collision.X, collision.Y, 20, 20), BULLET_DAMAGE));
                    attackTimer = attackDelay;
                    break;
                //Begin recovery delay. When timer reaches 0, switch to idle state
                case RangedEnemyState.Recovering:
                    attackTimer--;
                    if (attackTimer <= 0)
                    {
                        currentState = RangedEnemyState.Idle;
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
