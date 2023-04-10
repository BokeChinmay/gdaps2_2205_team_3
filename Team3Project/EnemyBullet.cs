using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//Class name: Enemy bullet
//Purpose: Inherits from bullet class. Used for distinction between enemy and player bullets.

namespace Team3Project
{
    internal class EnemyBullet : Bullet
    {

        public EnemyBullet(int speed, float xDirection, float yDirection, Rectangle collision, int damage, Texture2D texture) : base(speed, xDirection, yDirection, collision, damage, texture)
        {
            
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
