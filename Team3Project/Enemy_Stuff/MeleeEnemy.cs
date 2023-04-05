using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        //Range at which the enemy begins the attack process
        const int AGGRO_RANGE = 600;
        //Range at which attack begins
        const int ATTACK_RANGE = 200;
        //Duration of attack
        const int ATTACK_DURATION = 30;
        //Duration of recovery
        const int RECOVERY_DURATION = 120;

        //# of frames between telegraphing and attacking
        int attackDelay;
        //Attack timer, is changed per frame
        int attackTimer;
        //Direction of the attack
        Vector2 attackDirection;

        //Enemy texture (testing!)
        Texture2D meleeTexture;

        //Animation
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;

        // Constants for source rectangle
        const int WalkFrameCount = 4;
        const int RectHeight = 16;
        const int RectWidth = 16;

        //Constructor
        public MeleeEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay, Texture2D meleeTexture) : base(health, moveSpeed, collision, meleeTexture)
        {
            currentState = MeleeEnemyState.Idle;
            vulnerabilityState = VulnerabilityState.Vulnerable;
            this.attackDelay = attackDelay;
            type = EnemyTypes.Melee;
            this.meleeTexture = meleeTexture;

            //Initialize animation
            fps = 10;
            timePerFrame = 1.0 / fps;
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
        /// New version of update() that requires player collision and projectile list
        /// </summary>
        /// <param name="playerCollision"></param>
        /// <param name="projectileList"></param>
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
                        attackDirection = playerPos;
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
                    if (DistanceFromPlayer(attackDirection) < 10)
                    {
                        collision.X = (int)attackDirection.X;
                        collision.Y = (int)attackDirection.Y;
                        attackTimer = RECOVERY_DURATION;
                        currentState = MeleeEnemyState.Recovering;
                    }
                    else
                    {
                        MoveTowardPos(attackDirection, moveSpeed * 3);
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
            //In progress
            switch (currentState)
            {
                case MeleeEnemyState.Idle:
                    break;
                case MeleeEnemyState.Moving:
                    break;
                case MeleeEnemyState.Telegraphing:
                    break;
                case MeleeEnemyState.Attacking:
                    break;
                case MeleeEnemyState.Recovering:
                    break;
            }
            //base.Draw(spriteBatch, spriteEffects);
            
            spriteBatch.Draw(
                meleeTexture,
                collision,
                new Rectangle(24, 18, RectWidth, RectHeight),
                Color.White,
                0,
                Vector2.Zero,
                spriteEffects,
                0
                );
            
        }

        private void DrawMoving(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
                meleeTexture,                                   // - The texture to draw
                new Vector2(collision.X, collision.Y),          // - The location to draw on the screen
                new Rectangle(                                  // - The "source" rectangle
                    0,                                          //   - This rectangle specifies
                    frame * RectHeight,                         //	   where "inside" the texture
                    RectWidth,                                  //     to get pixels (We don't want to
                    RectHeight),                                //     draw the whole thing)
                Color.White,                                    // - The color
                0,                                              // - Rotation (none currently)
                Vector2.Zero,                                   // - Origin inside the image (top left)
                1.0f,                                           // - Scale (100% - no change)
                spriteEffects,                                  // - Can be used to flip the image
                0);
        }
    }
}
