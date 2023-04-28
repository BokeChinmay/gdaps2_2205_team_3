using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Name: RangedEnemy
//Purpose: Enemy child class for ranged enemy objects
//Restrictions: Inherits from Enemy

namespace Team3Project.Enemy_Stuff
{
    internal class RangedEnemy : Enemy
    {
        // Class-specific fields
        int attackDelay;
        int projectileSpeed;

        Texture2D rangedTexture;
        Texture2D pTexture;

        //Constant bullet speed
        const int BULLET_SPEED = 10;
        //Constant bullet damage
        const int BULLET_DAMAGE = 1;
        
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

        //Animation
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;
        SpriteEffects flipsprite;

        // Constants for source rectangle
        const int RECT_HEGHT = 27;
        const int RECT_WIDTH = 21;
        const int HORIZONTAL_BUFFER = 21;
        const int VERTICAL_BUFFER = 0;

        // Parameterized constructor
        public RangedEnemy(int health, int moveSpeed, Rectangle collision, int attackDelay, int projectileSpeed, Texture2D rangedTexture, Texture2D pTexture) : base(health, moveSpeed, collision, rangedTexture)
        {
            this.attackDelay = attackDelay;
            this.projectileSpeed = projectileSpeed;
            currentState = EnemyState.Idle;
            type = EnemyTypes.Ranged;
            this.rangedTexture = rangedTexture;
            this.pTexture = pTexture;
            attackTimer = 0;

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
        /// Updates once per frame, called once per Game1's Update()
        /// </summary>
        /// <param name="playerCollision"></param>
        /// <param name="projectileList"></param>
        public override void Update(Rectangle playerCollision, List<Projectile> projectileList, GameTime gameTime)
        {
            //Player position vector
            Vector2 playerPos = new Vector2((int)playerCollision.X, (int)playerCollision.Y);

            switch (currentState)
            {
                //Wait until player is within aggro range. If so, begin move.
                case EnemyState.Idle:
                    attackTimer++;
                    if (DistanceFromPlayer(playerPos) < AGGRO_RANGE)
                    {
                        currentState = EnemyState.Moving;
                    }
                    break;
                //Move if too close to the player. Otherwise, begin telegraphing.
                case EnemyState.Moving:
                    attackTimer++;
                    if (DistanceFromPlayer(playerPos) < ESCAPE_RANGE)
                    {
                        MoveAwayFromPos(playerPos);
                    }
                    else if (attackTimer < attackDelay)
                    {
                        downTimer = DOWNTIME;
                        currentState = EnemyState.Telegraphing;
                    }
                    break;
                //Begin telegraph delay. After timer reaches 0, begin attack
                case EnemyState.Telegraphing:
                    downTimer--;
                    if (downTimer <= 0)
                    {
                        currentState = EnemyState.Attacking;
                    }
                    break;
                //Create a bullet and move to recovery state
                case EnemyState.Attacking:
                    Shoot(BULLET_SPEED, playerPos, BULLET_DAMAGE, pTexture);
                    downTimer = DOWNTIME;
                    currentState = EnemyState.Recovering;
                    break;
                //Begin recovery delay. When timer reaches 0, switch to idle state
                case EnemyState.Recovering:
                    downTimer--;
                    if (downTimer <= 0)
                    {
                        currentState = EnemyState.Idle;
                        attackTimer = 0;
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
                            invincibilityTimer = INVINCIBILITY_DURATION;
                            vulnerabilityState = VulnerabilityState.Invincible;

                            if (health <= 0 + projectile.Damage)
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
        /// This is implemented in the enemy class
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
                texture,
                new Vector2(collision.X - 15, collision.Y - 15),
                new Rectangle(
                    HORIZONTAL_BUFFER * (frame - 1),
                    VERTICAL_BUFFER,
                    RECT_WIDTH,
                    RECT_HEGHT),
                Color.White,
                0,
                Vector2.Zero,
                3.5f,
                flipsprite,
                0
                );
        }

        /// <summary>
        /// Updates frame based on AI state
        /// </summary>
        private void UpdateFrame()
        {
            //For this spritesheet:
            //Idle: Frames 1-10
            //Moving: Frame 11
            //Telegraphing: Frames 12-13
            //Attacking: Frame 12
            //Recovering: Frames 14-16
            //Hurt: Frames 20-17 (backwards)
            //Death: Frames 17-25
            switch (currentState)
            {
                //Idle - looping
                case EnemyState.Idle:
                    if (frame < 1 || frame > 10)
                    {
                        frame = 1;
                    }
                    frame++;
                    break;
                //Moving - static frame
                case EnemyState.Moving:
                    frame = 11;
                    break;
                //Telegraphing - looping
                case EnemyState.Telegraphing:
                    if (frame < 11 || frame > 12)
                    {
                        frame = 11;
                    }
                    frame++;
                    break;
                //Attacking - static frame
                case EnemyState.Attacking:
                    frame = 12;
                    break;
                //Recovering - looping
                case EnemyState.Recovering:
                    if (frame < 13 || frame > 15)
                    {
                        frame = 13;
                    }
                    frame++;
                    break;
                //Hurt - does not loop
                case EnemyState.Hurt:
                    if (frame < 17 || frame > 20)
                    {
                        frame = 20;
                    }
                    else if (frame != 17)
                    {
                        frame--;
                    }
                    break;
                //Death - does not loop
                case EnemyState.Death:
                    if (frame < 17 || frame > 25)
                    {
                        frame = 17;
                    }
                    else if (frame != 25)
                    {
                        frame++;
                    }
                    //When the final frame of the death animation is reached, deactivate the object
                    else
                    {
                        active = false;
                    }
                    break;
            }
        }
    }
}
