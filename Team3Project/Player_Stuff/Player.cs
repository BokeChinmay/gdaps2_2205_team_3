using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    internal class Player : Entity, IDamageable
    {
        //Fields
        private Texture2D playerTexture;
        private Texture2D meleeTexture;
        private Texture2D bulletTexture;
        private KeyboardState prevKbState;
        private MouseState prevMouseState;
        private Random rng = new Random();
        private LastKbState lastKbState;

        private VulnerabilityState vulnerabilityState = VulnerabilityState.Vulnerable;
        private double invinsibilityFrames = 5;
        private int meleeDamage = 50;
        private int projectileDamage = 20;
        private int currentIFrames;

        private int currentLevel;

        public event Action gameOver;

        //Consts
        const int PlayerOffsetX = 4;
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

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="health"></param>
        /// <param name="moveSpeed"></param>
        /// <param name="collision"></param>
        /// <param name="playerTexture"></param>
        public Player(int health, int moveSpeed, Rectangle collision, Texture2D playerTexture, Texture2D meleeTexture, Texture2D bulletTexture) 
            : base(health, moveSpeed, collision)
        {
            this.playerTexture = playerTexture;
            this.meleeTexture = meleeTexture;
            this.bulletTexture = bulletTexture;
            lastKbState = LastKbState.W;
            currentIFrames = 0;
            currentLevel = 1;
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
            }

            if (kbState.IsKeyDown(Keys.D) && !rightBlocked)
            {
                collision.X+= moveSpeed;
                lastKbState = LastKbState.D;
            }

            if (kbState.IsKeyDown(Keys.S) && !bottomBlocked)
            {
                collision.Y+= moveSpeed;
                lastKbState = LastKbState.S;
            }

            if (kbState.IsKeyDown(Keys.W) && !topBlocked)
            {
                collision.Y-= moveSpeed;
                lastKbState = LastKbState.W;
            }
        }

        public void MeleeAttack(MouseState mouseState, KeyboardState kbState, SpriteBatch spriteBatch)
        {
            prevMouseState = mouseState;
            prevKbState = kbState;

            if(mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
            {
                if(lastKbState == LastKbState.W)
                {
                    Bullet bullet = new Bullet(10, 0, 5, new Rectangle(collision.X, collision.Y - 30, 30, 30), meleeDamage, bulletTexture, true);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.None);
                }
                else if (lastKbState == LastKbState.S)
                {
                    Bullet bullet = new Bullet(10, 0, 5, new Rectangle(collision.X, collision.Y + 30, 30, 30), meleeDamage, bulletTexture, true);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.FlipVertically);
                }
                else if (lastKbState == LastKbState.A)
                {
                    Bullet bullet = new Bullet(10, 5, 0, new Rectangle(collision.X - 30, collision.Y, 30, 30), meleeDamage, bulletTexture, true);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.FlipHorizontally);
                }
                else if (lastKbState == LastKbState.D)
                {
                    Bullet bullet = new Bullet(10, 5, 0, new Rectangle(collision.X + 30, collision.Y, 30, 30), meleeDamage, bulletTexture, true);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.None);
                }
            }

            prevMouseState = mouseState;
            prevKbState = kbState;
        }

        public void RangedAttack(MouseState mouseState, MouseState prevMouseState, KeyboardState kbState, SpriteBatch spriteBatch)
        {

            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released) 
            {
                //Calculate unit vector
                Vector2 displacement = new Vector2(mouseState.X, mouseState.Y) - new Vector2(collision.X, collision.Y);
                float distance = (float)Math.Sqrt(Math.Pow(displacement.X, 2) + Math.Pow(displacement.Y, 2)); ;
                Vector2 unitVector = displacement / distance;

                //Create a new bullet
                LevelManager.ProjectileList.Add(new Bullet(10, unitVector.X, unitVector.Y, new Rectangle(collision.X, collision.Y - 30, 30, 30), projectileDamage, bulletTexture, true));
            }

            /*
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                if (lastKbState == LastKbState.W)
                {
                    Bullet bullet = new Bullet(10, 0, 1, new Rectangle(collision.X, collision.Y - 30, 30, 30), projectileDamage, bulletTexture);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.None);
                    LevelManager.ProjectileList.Add(bullet);
                }
                else if (lastKbState == LastKbState.S)
                {
                    Bullet bullet = new Bullet(10, 0, collision.Y, new Rectangle(collision.X, collision.Y + 30, 30, 30), projectileDamage, bulletTexture);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.None);
                    LevelManager.ProjectileList.Add(bullet);
                }
                else if (lastKbState == LastKbState.A)
                {
                    Bullet bullet = new Bullet(10, collision.X, 0, new Rectangle(collision.X - 30, collision.Y, 30, 30), projectileDamage, bulletTexture);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.None);
                    LevelManager.ProjectileList.Add(bullet);
                }
                else if (lastKbState == LastKbState.D)
                {
                    Bullet bullet = new Bullet(10, collision.X, 0, new Rectangle(collision.X + 30, collision.Y, 30, 30), projectileDamage, bulletTexture);
                    bullet.Update();
                    bullet.Draw(spriteBatch, SpriteEffects.None);
                    LevelManager.ProjectileList.Add(bullet);
                }   
            }
            */
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

                item.CheckCollision(this);
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
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects flipSprite)
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
                             flipSprite,
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
                gameOver();
            }

            if (currentIFrames > -5)
            {
                currentIFrames -= 1;
            }

            Move(kbState);

            // When adding attack capabilities to the player, make left click shoot and right click melee
        }

        /// <summary>
        /// Reorients the player when a new level is started
        /// </summary>
        public void nextLevel()
        {
            collision.X = 734;
            collision.Y = 864;
            currentLevel++;
            
            if (health < 3)
            {
                health += 1;
            }
        }
    }
}
