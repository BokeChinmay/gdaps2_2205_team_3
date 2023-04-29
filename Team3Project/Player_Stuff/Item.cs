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


        public void CheckCollision(Entity check)
        {
            if (active == true && check.Collision.Intersects(collision))
            {
                Active = false;

                if (itemType == ItemType.SpeedBoost)
                {
                    if (check is Player)
                    {
                        Player player = (Player)check;

                        player.MoveSpeed = player.MoveSpeed + 1;
                    }
                }

                if (itemType == ItemType.DamageBoost)
                {
                    if (check is Player)
                    {
                        Player player = (Player)check;

                        player.MeleeDamage = player.MeleeDamage + (player.MeleeDamage / 10);
                        player.ProjectileDamage = player.ProjectileDamage + (player.ProjectileDamage / 10);
                    }
                }

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

                if (itemType == ItemType.HealthPickup)
                {
                    if (check is Player)
                    {
                        Player player = (Player)check;

                        player.TakeDamage(-1);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            if (Active)
            {
                if (itemType == ItemType.LifePickup)
                {
                    spriteBatch.Draw(itemTexture, new Rectangle(collision.X - 6, collision.Y - 6, 32, 32), Color.White);
                }
                else if (itemType == ItemType.HealthPickup)
                {
                    spriteBatch.Draw(itemTexture, new Rectangle(collision.X - 1, collision.Y - 1, 22, 22), Color.White);
                }
                else
                {
                    spriteBatch.Draw(itemTexture, Collision, Color.White);
                }
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public void Update(Player player)
        {
            CheckCollision(player);
        }
    }
}
