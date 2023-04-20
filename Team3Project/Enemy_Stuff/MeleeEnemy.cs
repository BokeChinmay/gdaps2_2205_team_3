using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

//Name: MeleeEnemy
//Purpose: Enemy child class for melee enemy objects.
//Restrictions: Inherits from Enemy

namespace Team3Project.Enemy_Stuff
{
    

    internal class MeleeEnemy : Enemy, IDamageable
    {
        EnemyState currentState;
        //Range at which the enemy begins the attack process
        const int AGGRO_RANGE = 600;
        //Range at which attack begins
        const int ATTACK_RANGE = 200;
        //Duration of attack
        const int ATTACK_DURATION = 120;
        //Duration of recovery
        const int RECOVERY_DURATION = 120;

        //# of frames between telegraphing and attacking
        int attackDelay;
        //Attack timer, is changed per frame
        int attackTimer;
        //Direction of the attack
        Vector2 attackDirection;

        //Animation
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        SpriteEffects flipsprite;

        // Constants for source rectangle
        const int RectHeight = 22;
        const int RectWidth = 24;
        const int HORIZONTAL_BUFFER = 24;
        const int VERTICAL_BUFFER = 0;

        //Constructor
        public MeleeEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay, Texture2D texture) : base(health, moveSpeed, collision, texture)
        {
            currentState = EnemyState.Idle;
            vulnerabilityState = VulnerabilityState.Vulnerable;
            this.attackDelay = attackDelay;
            type = EnemyTypes.Melee;

            //Initialize animation
            fps = 5.0;
            timePerFrame = 1.0 / fps;
            frame = 1;
            flipsprite = SpriteEffects.None;
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
        public override void Update(Rectangle playerCollision, List<Projectile> projectileList, GameTime gameTime)
        {
            //Vector 2 of player position
            Vector2 playerPos = new Vector2((int)playerCollision.X, (int)playerCollision.Y);

            //STATE MACHINE - Movement/attacking state
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
                        attackDirection = playerPos;
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
                    if (DistanceFromPlayer(attackDirection) < 10)
                    {
                        collision.X = (int)attackDirection.X;
                        collision.Y = (int)attackDirection.Y;
                        attackTimer = RECOVERY_DURATION;
                        currentState = EnemyState.Recovering;
                    }
                    else if (attackTimer <= 0)
                    {
                        attackTimer = RECOVERY_DURATION;
                        currentState = EnemyState.Recovering;
                    }
                    else
                    {
                        MoveTowardPos(attackDirection, moveSpeed * 3);
                        attackTimer--;
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
                //Hurt - A few frames of idleness after being hurt
                case EnemyState.Hurt:
                    //No update
                    break;
                //Death - Play death animation and then die
                case EnemyState.Death:
                    //No update
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
                        if (collision.Intersects(projectile.Collision) && !(projectile is EnemyBullet))
                        {
                            TakeDamage(projectile.Damage);
                            invincibilityTimer = INVINCIBILITY_DURATION;
                            vulnerabilityState = VulnerabilityState.Invincible;

                            if (health <= 0)
                            {
                                currentState = EnemyState.Death;
                            }
                            else
                            {
                                currentState = EnemyState.Hurt;
                            }                          
                        }
                    }
                    break;
                //Enemy is invincible - cannot take damage for a few frames.
                //When timer hits 0, return to vulnerable state
                case VulnerabilityState.Invincible:
                    if (currentState == EnemyState.Death)
                    {
                        break;
                    }
                    invincibilityTimer--;
                    if (invincibilityTimer <= 0)
                    {
                        vulnerabilityState = VulnerabilityState.Vulnerable;
                        currentState = EnemyState.Idle;
                    }
                    break;

            }

            //Animation - update frame if enough time has passed
            // How much time has passed?  
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
            // If enough time has passed:
            if (timeCounter >= timePerFrame)
            {
                UpdateFrame();
                timeCounter = 0;
            }

            //Flip sprite if moving to the left
            if (currentState == EnemyState.Moving)
            {
                if (DirectionToPlayer(playerPos).X < 0)
                {
                    flipsprite = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    flipsprite = SpriteEffects.None;
                }
            }

        }

        /// <summary>
        /// Overriden draw method. Draws a source rectangle from within the spritesheet based on the current frame
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
                texture,
                new Vector2(collision.X - 15, collision.Y - 15),
                new Rectangle(
                    HORIZONTAL_BUFFER * (frame - 1), 
                    VERTICAL_BUFFER, 
                    RectWidth, 
                    RectHeight),
                Color.White,
                0,
                Vector2.Zero,
                3.0f,
                flipsprite,
                0
                );
        }

        /// <summary>
        /// Updates the current frame based on the enemy's FSM state
        /// </summary>
        private void UpdateFrame()
        {
            //For this spritesheet:
            //Idle: Frames 1-7
            //Moving: Frames 8-13
            //Telegraphing: Frames 14-16
            //Attacking: Frames 17-21
            //Recovering: Frames 22-23
            //Hurt: Frames 25-28
            //Death: Frames 29-37
            switch (currentState)
            {
                //Idle frames - looping
                case EnemyState.Idle:
                    if (frame < 1 || frame > 7)
                    {
                        frame = 1;
                    }
                    frame++;
                    break;
                //Moving frames - looping
                case EnemyState.Moving:
                    if (frame < 8 || frame > 13)
                    {
                        frame = 8;
                    }
                    frame++;
                    break;
                //Telegraph - does not loop
                case EnemyState.Telegraphing:
                    if (frame < 14 || frame > 16)
                    {
                        frame = 14;
                    }
                    else if (frame != 16)
                    {
                        frame++;
                    }      
                    break;
                //Attacking - looping
                case EnemyState.Attacking:
                    if (frame < 17 || frame > 20)
                    {
                        frame = 17;
                    }
                    frame++;
                    break;
                //Recovering - looping
                case EnemyState.Recovering:
                    if (frame < 21 || frame > 23)
                    {
                        frame = 21;
                    }
                    frame++;
                    break;
                //Hurt - does not loop
                case EnemyState.Hurt:
                    if (frame < 25 || frame > 28)
                    {
                        frame = 25;
                    }
                    else if (frame != 28)
                    {
                        frame++;
                    }
                    break;
                //Death - does not loop
                case EnemyState.Death:
                    if (frame < 29 || frame > 37)
                    {
                        frame = 29;
                    }
                    else if (frame != 37)
                    {
                        frame++;
                    }
                    //When the final frame of the death animation is complete, deactivate the object
                    else
                    {
                        active = false;
                    }
                    break;
            }
        }
    }
}
