﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//Name: Bullet
//Purpose: A class for bullet projectiles that move in a straight line.

namespace Team3Project
{
    internal class Bullet : Projectile
    {
        // A vector for the projectile's direction
        private Vector2 direction;

        public Bullet(int speed, float xDirection, float yDirection, Rectangle collision, int damage, Texture2D texture) : base(1, speed, collision, texture)
        {
            direction = new Vector2(xDirection * speed, yDirection * speed);
            this.damage = damage;
        }

        public void Move()
        {
            //Update position by direction vector
            collision.X += (int)direction.X;
            collision.Y += (int)direction.Y;

            //Check if the direction the bullet is moving in is blocked. If so, set active to false
            if (direction.X > 0 && rightBlocked)
            {
                active = false;
            }
            if (direction.X < 0 && leftBlocked)
            {
                active = false;
            }
            if (direction.Y > 0 && bottomBlocked)
            {
                active = false;
            }
            if (direction.Y < 0 && topBlocked)
            {
                active = false;
            }
        }

        public override void Update()
        {
            Move();
        }

    }
}