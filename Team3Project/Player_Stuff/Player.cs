using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Name: Player
//Purpose: Class for the player object
//Restrictions: Inherits from Entity

namespace Team3Project.Player_Stuff
{
    enum VulnerabilityState
    {  
        Vulnerable,
        Invincible
    }
    enum LastKbState
    {
        W, A, S, D
    }
    enum PlayerState
    {
        Moving,
        MeleeAttack,
        Death
    }

    internal class Player : Entity, IDamageable
    {
        //Fields
        private Texture2D playerTexture;
        private Texture2D meleeTexture;
        private Texture2D bulletTexture;
        private SpriteEffects flipsprite;
        private KeyboardState prevKbState;
        private MouseState prevMouseState;
        private Random rng = new Random();
        private LastKbState lastKbState;
        private PlayerState playerState;

        private VulnerabilityState vulnerabilityState = VulnerabilityState.Vulnerable;
        private double invinsibilityFrames = 5;
        private int meleeDamage = 50;
        private int projectileDamage;
        private int currentIFrames;
        private int lives;
        private int maxHealth;
        private const int ATTACK_DELAY = 30;
        private int attackTimer;
        private int deathFrameTimer;
        int PlayerOffsetX;

        private int currentLevel;

        public event Action gameOver;
        public event Action lostLife;

        //Consts
        
        const int PlayerOffsetY = 9;
        const int PlayerRectHeight = 26;
        const int PlayerRectWidth = 32;

        //Properties
        public int MeleeDamage
        {
            get { return meleeDamage; }
            set { meleeDamage = value; }
        }

        public int ProjectileDamage
        {
            get { return projectileDamage; }
            set { projectileDamage = value; }
        }

        public int Level
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }

        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="collision"></param>
        /// <param name="playerTexture"></param>
        public Player(int health, int maxHealth, int lives, int moveSpeed, int bulletDamage, Rectangle collision, Texture2D playerTexture, Texture2D meleeTexture, Texture2D bulletTexture) 
            : base(health, moveSpeed, collision)
        {
            this.playerTexture = playerTexture;
            this.meleeTexture = meleeTexture;
            this.bulletTexture = bulletTexture;
            projectileDamage = bulletDamage;
            this.lives = lives;
            this.maxHealth = maxHealth;

            lastKbState = LastKbState.W;
            currentIFrames = 0;
            currentLevel = 1;
            playerState = PlayerState.Moving;
            attackTimer = 0;
            deathFrameTimer = 30;
            PlayerOffsetX = 4;

            lostLife += Demoted;
            gameOver += ResetStats;
        }

        /// <summary>
        /// Makes the character move.
        /// </summary>
        public void Move(KeyboardState kbState)
        {

            if (kbState.IsKeyDown(Keys.A) && !leftBlocked)
            {
                collision.X-= moveSpeed;
                lastKbState = LastKbState.A;
                flipsprite = SpriteEffects.None;
                PlayerOffsetX = 4;
            }

            if (kbState.IsKeyDown(Keys.D) && !rightBlocked)
            {
                collision.X+= moveSpeed;
                lastKbState = LastKbState.D;
                flipsprite = SpriteEffects.FlipHorizontally;
                PlayerOffsetX = 4;
            }

            if (kbState.IsKeyDown(Keys.S) && !bottomBlocked)
            {
                collision.Y+= moveSpeed;
                lastKbState = LastKbState.S;
                PlayerOffsetX = 43;
            }

            if (kbState.IsKeyDown(Keys.W) && !topBlocked)
            {
                collision.Y-= moveSpeed;
                lastKbState = LastKbState.W;
                PlayerOffsetX = 43;
            }
        }

        public void MeleeAttack(MouseState mouseState, MouseState prevMouseState, KeyboardState kbState, SpriteBatch spriteBatch)
        {
            prevKbState = kbState;

            if(mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released && attackTimer <= 0)
            {
                playerState = PlayerState.MeleeAttack;

                //Calculate direction of attack
                Vector2 displacement = new Vector2(mouseState.X, mouseState.Y) - new Vector2(collision.X, collision.Y);
                float distance = (float)Math.Sqrt(Math.Pow(displacement.X, 2) + Math.Pow(displacement.Y, 2)); ;
                Vector2 unitVector = displacement / distance;

                //Calculate attack angle in radians
                float rotation = (float)(1 * Math.PI / 2) - (float)Math.Atan2(unitVector.X, unitVector.Y);

                //Create new projectile and add it to the projectile list
                MeleeProjectile slash = new MeleeProjectile(
                    0,
                    50,
                    new Rectangle((int)(collision.X + (unitVector.X * 40) + 15), (int)(collision.Y + (unitVector.Y * 40) + 15), 50, 20),
                    meleeTexture,
                    true,
                    rotation);
                LevelManager.AddProjectile(slash);
                attackTimer = ATTACK_DELAY;
            }

            if (attackTimer > 0)
            {
                attackTimer--;
            }
            else
            {
                playerState = PlayerState.Moving;
            }

            prevKbState = kbState;
        }

        public void RangedAttack(MouseState mouseState, MouseState prevMouseState, KeyboardState kbState, SpriteBatch spriteBatch)
        {

            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released && attackTimer <= 0) 
            {
                //Calculate unit vector
                Vector2 displacement = new Vector2(mouseState.X, mouseState.Y) - new Vector2(collision.X, collision.Y);
                float distance = (float)Math.Sqrt(Math.Pow(displacement.X, 2) + Math.Pow(displacement.Y, 2)); ;
                Vector2 unitVector = displacement / distance;

                //Create a new bullet
                LevelManager.ProjectileList.Add(new Bullet(10, unitVector.X, unitVector.Y, new Rectangle(collision.X, collision.Y + playerTexture.Height/4, 30, 30), projectileDamage, bulletTexture, true));
                attackTimer = ATTACK_DELAY;
            }
            
            if (attackTimer > 0)
            {
                attackTimer--;
            }
        }

        /// <summary>
        /// Method to check the collision between two objects.
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        public void CheckCollision(Entity check)
        {
            
            if(check is Item && check.Collision.Intersects(collision))
            {
                Item item = (Item)check;

                //item.CheckCollision(this);
            }
            else if (check.Collision.Intersects(collision))
            {
                TakeDamage(1);
            }
        }

        /// <summary>
        /// Method that simulates taking damage and sets the entity to inactive if the health reaches zero
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="entity"></param>
        public void TakeDamage(int damage)
        {
            if (currentIFrames <= 0)
            {
                Health = Health - damage;
                currentIFrames = 60;
            }
        }

        /// <summary>
        /// Draw Method.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="flipSprite"></param>
        /// <returns></returns>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            
            spriteBatch.Draw(playerTexture,
                             collision,
                             new Rectangle(
                                 PlayerOffsetX,
                                 PlayerOffsetY,
                                 PlayerRectWidth,
                                 PlayerRectHeight),
                             Color.White,
                             0,
                             Vector2.Zero,
                             flipsprite,
                             0
                             );
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public void Update(KeyboardState kbState)
        {
            if(Health <= 0)
            {
                playerState = PlayerState.Death;
                //Death animation
                if (deathFrameTimer >= 0)
                {
                    PlayerOffsetX = 4 + (6 - ((deathFrameTimer / 6) % 5)) * 39;
                    deathFrameTimer--;
                }
                //Buffer between player death / showing the end screen
                else if (deathFrameTimer > -60)
                {
                    deathFrameTimer--;
                }
                //Reset death animation stuff, call gameOver() event
                else
                {
                    PlayerOffsetX = 4;
                    deathFrameTimer = 30;

                    if (lives <= 1)
                    {
                        gameOver();
                    }
                    else
                    {
                        active = false;
                    }
                }
            }

            if (currentIFrames > -5)
            {
                currentIFrames -= 1;
            }

            if (playerState == PlayerState.Moving)
            {
                Move(kbState);
            }
        }

        /// <summary>
        /// Reorients the player when a new level is started
        /// </summary>
        public void nextLevel()
        {
            collision.X = 734;
            collision.Y = 864;
            currentLevel++;
        }

        public void Demoted()
        {
            collision.X = 734;
            collision.Y = 864;

            lives--;
            maxHealth--;

            active = true;
            health = maxHealth;
        }

        public void ResetStats()
        {
            lives = 3;
            maxHealth = 3;
            health = maxHealth;
            moveSpeed = 5;
            projectileDamage = 20;
            Level = 1;
            active = true;
            collision.X = 734;
            collision.Y = 864;
        }
    }
}
