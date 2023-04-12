using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Name: IObstruction
// Purpose: interface defining properties that can be used to establish an objects's edges,
//          so that other objects can be obstructed from moving through them

namespace Team3Project
{
    internal interface IObstruction
    {
        // Properties defining edges
        public int TopEdge { get; }
        public int LeftEdge { get; }
        public int RightEdge { get; }
        public int BottomEdge { get; }

        // Properties defining whether the object should be treated as an obstruction
        public bool IsObstruction { get; }
    }
}
