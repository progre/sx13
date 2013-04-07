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
        public readonly bool DirectionToggled;
        public readonly bool Shot;
        public readonly bool ShotToggled;
        public readonly bool Pause;
        public readonly bool PauseToggled;

        public Input(
            byte direction, bool directionToggled,
            bool shot, bool shotToggled,
            bool pause, bool pauseToggled)
        {
            Direction = direction;
            DirectionToggled = directionToggled;
            Shot = shot;
            ShotToggled = shotToggled;
            Pause = pause;
            PauseToggled = pauseToggled;
        }
    }
}
