using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team3Project
{
    internal interface IObstruction
    {
        public int TopEdge { get; }
        public int LeftEdge { get; }
        public int RightEdge { get; }
        public int BottomEdge { get; }

    }
}
