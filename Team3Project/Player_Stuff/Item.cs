using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Team3Project.Player_Stuff
{
    public enum ItemType
    {
        DamageBoost,
        SpeedBoost
    }
    internal class Item : Entity
    {
        //loads in the current texture of the item
        private Texture2D itemTexture;

        private ItemType itemType;
        

        //for now we will make the item's location be the center of the room hopefully we can introduce enemy drops at a later date
        private Vector2 itemLoc = new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight);

        private Random rng = new Random();

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="collision"></param>
        /// <param name="itemTexture"></param>
        public Item(int health, int moveSpeed, Rectangle collision, Texture2D itemTexture , ItemType itemType) : base (health, moveSpeed, collision)
        {
            this.itemTexture = itemTexture;
            this.itemType = itemType;
        }


        public void CheckCollision(Entity check)
        {
            if (check.Collision.Intersects(collision))
            {
                Active = false;
            }

            if(itemType == ItemType.SpeedBoost)
            {
                if(check is Player)
                {
                    check = (Player)check;
                }
            }
        }


        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            while (Active)
            {
                spriteBatch.Draw(itemTexture, Collision, Color.White);  
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }





    }
}
