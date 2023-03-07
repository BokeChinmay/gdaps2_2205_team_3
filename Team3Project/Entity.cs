using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal abstract class Entity
    {
        //Fields
        protected bool isActive;

        /// <summary>
        /// Abstract class to function movement.
        /// </summary>
        public abstract void Move();
    }
}
