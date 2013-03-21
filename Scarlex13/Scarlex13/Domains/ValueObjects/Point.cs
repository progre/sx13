using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressive.Scarlex13.Domains.ValueObjects
{
    struct Point
    {
        public short X;
        public short Y;

        public Point(short x, short y)
        {
            X = x;
            Y = y;
        }
    }
}
