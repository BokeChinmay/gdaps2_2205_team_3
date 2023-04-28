using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Player_Stuff;

//Name: Health Display
//Purpose: Displays the player's health on the left side of the screen
//Restrictions: Inherits from UI Object

namespace Team3Project.Stage_Stuff
{
    internal class HealthDisplay : UIObject
    {
        // Field declarations
        private int health;
        private int lives;

        private SpriteFont font;

        private Texture2D fullHP;
        private Texture2D emptyHP;
        private Texture2D brokenHP;

        private Texture2D fullLife;
        private Texture2D emptyLife;

        // Read-and-write properties for fields
        public int Health { get { return health; } set { health = value; } }
        public int Lives { get { return lives; } set {  lives = value; } }

        // Parameterized constructor
        public HealthDisplay(SpriteFont font, Texture2D fullHP, Texture2D emptyHP, 
            Texture2D brokenHP, Texture2D fullLife, Texture2D emptyLife) : base(45, 75, 135, 740)
        {
            health = 3;
            lives = 3;
            this.font = font;
            this.fullHP = fullHP;
            this.emptyHP = emptyHP;
            this.brokenHP = brokenHP;
            this.fullLife = fullLife;
            this.emptyLife = emptyLife;
        }

        /// <summary>
        /// Draw method
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.DrawString(font, "HP", new Vector2(65, 35), Color.LightGreen);
            
            if (health == 3)
            {
                spriteBatch.Draw(fullHP, new Rectangle(37, 75, 105, 210), Color.White);
                spriteBatch.Draw(fullHP, new Rectangle(37, 295, 105, 210), Color.White);
                spriteBatch.Draw(fullHP, new Rectangle(37, 515, 105, 210), Color.White);
            }
            else if (health == 2) 
            {
                spriteBatch.Draw(emptyHP, new Rectangle(37, 75, 105, 210), Color.White);
                spriteBatch.Draw(fullHP, new Rectangle(37, 295, 105, 210), Color.White);
                spriteBatch.Draw(fullHP, new Rectangle(37, 515, 105, 210), Color.White);
            }
            else if (health == 1) 
            {
                spriteBatch.Draw(emptyHP, new Rectangle(37, 75, 105, 210), Color.White);
                spriteBatch.Draw(emptyHP, new Rectangle(37, 295, 105, 210), Color.White);
                spriteBatch.Draw(fullHP, new Rectangle(37, 515, 105, 210), Color.White);
            }
            else if (health == 0)
            {
                spriteBatch.Draw(emptyHP, new Rectangle(37, 75, 105, 210), Color.White);
                spriteBatch.Draw(emptyHP, new Rectangle(37, 295, 105, 210), Color.White);
                spriteBatch.Draw(emptyHP, new Rectangle(37, 515, 105, 210), Color.White);
            }
        }
    }
}
