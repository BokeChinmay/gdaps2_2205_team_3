using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//Name: Item
//Purpose: Class for item objects, which the player can pick up to boost stats
//Restrictions: Inherits from Entity

namespace Team3Project.Player_Stuff
{
    public enum ItemType
    {
        DamageBoost,
        SpeedBoost,
        HealthPickup,
        LifePickup
    }
    internal class Item : Entity
    {
        //loads in the current texture of the item
        private Texture2D itemTexture;

        private ItemType itemType;

        private int playerHealth;
        private int playerLives;

        private Random rng = new Random();

        //Properties
        public ItemType ItemType
        {
            get { return itemType; }
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="itemTexture"></param>
        public Item(int health, int moveSpeed, Rectangle collision, Texture2D itemTexture , ItemType itemType) : base (health, moveSpeed, collision)
        {
            this.itemTexture = itemTexture;
            this.itemType = itemType;
            active = false;
        }

        /// <summary>
        /// This check collision applies the effects of the items to the player
        /// </summary>
        /// <param name="check"></param>
        public void CheckCollision(Entity check)
        {
            if (active == true && check.Collision.Intersects(collision))
            {
                Active = false;

                //If the item is a speed boost, increase player move speed by 1
                if (itemType == ItemType.SpeedBoost)
                {
                    if (check is Player)
                    {
                        Player player = (Player)check;

                        player.MoveSpeed = player.MoveSpeed + 1;
                    }
                }

                //If the item is a damage boost, increase ranged and melee damage by 2 and 5, respectively
                if (itemType == ItemType.DamageBoost)
                {
                    if (check is Player)
                    {
                        Player player = (Player)check;

                        player.MeleeDamage = player.MeleeDamage + 5;
                        player.ProjectileDamage = player.ProjectileDamage + 2;
                    }
                }

                //If the item is a life pickup, increase lives by 1
                if (itemType == ItemType.LifePickup)
                {
                    if (check is Player)
                    {
                        Player player = (Player)check;

                        if (player.Lives < 3)
                        {

                            player.Lives += 1;
                        }
                    }
                }

                //If the item is a health pickup, increase health by 1
                if (itemType == ItemType.HealthPickup)
                {
                    if (check is Player)
                    {
                        Player player = (Player)check;

                        player.TakeDamage(-1);
                    }
                }

                LevelManager.AddItem(itemType);
            }
        }

        /// <summary>
        /// Overriden draw method to draw the pickup
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            if (Active)
            {
                if (itemType == ItemType.LifePickup)
                {
                    spriteBatch.Draw(itemTexture, new Rectangle(collision.X - 6, collision.Y - 6, 64, 64), Color.White);
                }
                else if (itemType == ItemType.HealthPickup)
                {
                    spriteBatch.Draw(itemTexture, new Rectangle(collision.X - 1, collision.Y - 1, 66, 66), Color.White);
                }
                else
                {
                    spriteBatch.Draw(itemTexture, Collision, Color.White);
                }
            }
        }

        /// <summary>
        /// Not implemented without overloads
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override void Update()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update: checks collision with the player object
        /// </summary>
        /// <param name="player"></param>
        public void Update(Player player)
        {
            CheckCollision(player);
        }
    }
}
