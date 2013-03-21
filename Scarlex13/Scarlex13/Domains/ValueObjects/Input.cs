using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressive.Scarlex13.Domains.ValueObjects
{
    class Input
    {
        public readonly byte Direction;
        public readonly bool? Shot;
        public readonly bool? Pause;

        public Input(byte direction, bool? shot, bool? pause)
        {
            Direction = direction;
            Shot = shot;
            Pause = pause;
        }
    }
}
