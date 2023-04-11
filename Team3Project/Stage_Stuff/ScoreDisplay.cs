using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team3Project.Player_Stuff;

namespace Team3Project.Stage_Stuff
{
    internal class ScoreDisplay : UIObject
    {
        private SpriteFont font;
        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public ScoreDisplay(SpriteFont font) : base (1700, 75, 135, 740)
        {
            this.font = font;
        }

        public void Update(Player player)
        {
            level = player.Level;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.DrawString(font, $"Level: {level}", new Vector2(1355, 25), Color.LightGreen);
        }
    }
}
