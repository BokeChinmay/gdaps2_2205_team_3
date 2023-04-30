using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Stage_Stuff;
using Team3Project.Enemy_Stuff;

namespace Team3Project
{
    internal class Healthbar : UIObject
    {
        // Field declarations
        private Enemy_Stuff.Enemy enemy;
        private int maxHealth;
        private int healthRemaining;
        
        private Texture2D backTexture;
        private Texture2D frontTexture;
        private Texture2D backTextureBoss;
        private Texture2D frontTextureBoss;

        // Testing properties
        public int Max
        {
            get { return maxHealth; }
        }

        public int Remaining
        {
            get { return healthRemaining; }
        }

        public Rectangle Dimensions
        {
            get { return dimensions; }
        }

        public Enemy number1 
        { 
            get { return enemy; } 
        }

        public Healthbar(Enemy enemy, Texture2D back1, Texture2D front1, 
            Texture2D back2, Texture2D front2) : base (enemy.Collision.X, enemy.Collision.Y, 0, 0)
        {
            this.enemy = enemy;
            maxHealth = enemy.Health;

            this.backTexture = back1;
            this.frontTexture = front1;
            this.backTextureBoss = back2;
            this.frontTextureBoss = front2;

            if (enemy is BossEnemy)
            {
                dimensions = new System.Drawing.Rectangle(375, 790, 750, 90);
                healthRemaining = 726;
            }
            else
            {
                dimensions = new System.Drawing.Rectangle(enemy.Collision.X, enemy.Collision.Y + enemy.Collision.Height + 5, 50, 20);
                healthRemaining = 46;
            }
        }

        public void Update()
        {
            if (enemy is BossEnemy)
            {
                healthRemaining = (int)((float)((float)enemy.Health / (float)maxHealth) * 726f);
            }
            else
            {
                healthRemaining = (int)((float)((float)enemy.Health / (float)maxHealth) * 46f);
                dimensions.X = enemy.Collision.X;
                dimensions.Y = enemy.Collision.Y + enemy.Collision.Height + 5;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            if (enemy is BossEnemy)
            {
                spriteBatch.Draw(
                    backTextureBoss, new Microsoft.Xna.Framework.Vector2(dimensions.X, dimensions.Y), 
                    Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(
                    frontTextureBoss, new Microsoft.Xna.Framework.Rectangle(dimensions.X + 12, dimensions.Y + 12, healthRemaining, 66), 
                    Microsoft.Xna.Framework.Color.White);
            }
            else
            {
                spriteBatch.Draw(
                    backTexture, new Microsoft.Xna.Framework.Vector2(dimensions.X, dimensions.Y),
                    Microsoft.Xna.Framework.Color.White);
                spriteBatch.Draw(
                    frontTexture, new Microsoft.Xna.Framework.Rectangle(dimensions.X + 2, dimensions.Y + 2, healthRemaining, 16),
                    Microsoft.Xna.Framework.Color.White);
            }
        }
    }
}
