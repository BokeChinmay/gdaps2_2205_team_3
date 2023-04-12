using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Player_Stuff;

//Name: Elevator
//Purpose: Class for the elevator object the player uses to progress to the next level
//Restrictions: Inherits from StageObject

namespace Team3Project.Stage_Stuff
{
    internal class Elevator : StageObject
    {
        // Field declarations
        private Texture2D open;
        private Texture2D closed;
        private bool isOpen;

        public event Action NewLevel;

        // Read-and-write propety for whether the elevator is open
        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }
        
        // Parameterized constructor
        public Elevator(Texture2D open, Texture2D closed) : base(114, 100, 693, 0)
        {
            this.open = open;
            this.closed = closed;
            isObstruction = false;
            isOpen = false;
        }

        /// <summary>
        /// Draws the elevator based on whether it's open
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="spriteEffects"></param>
        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            if (isOpen) 
            {
                spriteBatch.Draw(open, dimensions, Color.White);
            }
            else
            {
                spriteBatch.Draw(closed, dimensions, Color.White);
            }
        }

        /// <summary>
        /// Calls an event if the player enters the elevator while it's open
        /// </summary>
        /// <param name="player"> takes in the player to check if they're intersecting the elevator </param>
        public void PlayerEnters(Player player)
        {
            if (isOpen && player.Collision.Intersects(dimensions))
            {
                NewLevel();
            }
        }
    }
}
