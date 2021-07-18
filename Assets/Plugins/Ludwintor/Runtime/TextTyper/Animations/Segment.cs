using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ludwintor.Tools
{
    internal struct Segment
    {
        public int leftEdge;
        public int rightEdge;

        public Segment(int leftEdge, int rightEdge)
        {
            if (leftEdge <= rightEdge)
            {
                this.leftEdge = leftEdge;
                this.rightEdge = rightEdge;
            }
            else
            {
                this.leftEdge = rightEdge;
                this.rightEdge = leftEdge;
            }
        }

        public bool Contains(int number) => number >= leftEdge && number <= rightEdge;
    }
}