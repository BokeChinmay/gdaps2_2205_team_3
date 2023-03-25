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

        Texture2D rangedTexture;

        //Constant bullet speed
        const int BULLET_SPEED = 10;
        //Constant bullet damage
        const int BULLET_DAMAGE = 50;
        
        //Range at which the attack sequence begins
        const int AGGRO_RANGE = 600;
        //Range at which the enemy runs away from the player
        const int ESCAPE_RANGE = 300;
        //Amount of frame downtime for telegraph/recovery
        const int DOWNTIME = 60;

        //Frames since last attack
        int attackTimer;
        //Frame timer for telegraph/recovery
        int downTimer;

        // Parameterized constructor
        public RangedEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay, int projectileSpeed, Texture2D rangedTexture) : base(health, moveSpeed, collision, rangedTexture)
        {
            this.attackDelay = attackDelay;
            this.projectileSpeed = projectileSpeed;
            currentState = RangedEnemyState.Idle;
            type = EnemyTypes.Ranged;
            this.rangedTexture = rangedTexture;
            attackTimer = 0;
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
                    attackTimer++;
                    if (DistanceFromPlayer(playerPos) < AGGRO_RANGE)
                    {
                        currentState = RangedEnemyState.Move;
                    }
                    break;
                //Move if too close to the player. Otherwise, begin telegraphing.
                case RangedEnemyState.Move:
                    attackTimer++;
                    if (DistanceFromPlayer(playerPos) < ESCAPE_RANGE)
                    {
                        MoveAwayFromPos(playerPos);
                    }
                    else if (attackTimer < attackDelay)
                    {
                        downTimer = DOWNTIME;
                        currentState = RangedEnemyState.Telegraphing;
                    }
                    break;
                //Begin telegraph delay. After timer reaches 0, begin attack
                case RangedEnemyState.Telegraphing:
                    downTimer--;
                    if (downTimer <= 0)
                    {
                        currentState = RangedEnemyState.Attack;
                    }
                    break;
                //Create a bullet and move to recovery state
                case RangedEnemyState.Attack:
                    Shoot(BULLET_SPEED, playerPos, BULLET_DAMAGE);
                    downTimer = DOWNTIME;
                    currentState = RangedEnemyState.Recovering;
                    break;
                //Begin recovery delay. When timer reaches 0, switch to idle state
                case RangedEnemyState.Recovering:
                    downTimer--;
                    if (downTimer <= 0)
                    {
                        currentState = RangedEnemyState.Idle;
                        attackTimer = 0;
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
                        if (!(projectile is EnemyBullet) && collision.Intersects(projectile.Collision))
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


        /// <summary>
        /// This is implemented in the enemy class
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            base.Draw(spriteBatch, spriteEffects);
        }
    }
}
