using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressive.Scarlex13.Domains.ValueObjects
{
    struct Direction8
    {
        private static readonly byte[] Directions
            = new byte[] { 8, 9, 6, 3, 2, 1, 4, 7 };
        private readonly byte _value;

        public Direction8(byte value)
        {
            _value = value;
        }

        public byte Value { get { return _value; } }

        public static Direction8 FromRadian(double rad)
        {
            // Math.PIがconstだからコンパイル時に"rad < (定数)"になってくれる筈
            // 無理ならMath.PI / 8を先に計算する
            if (rad < -Math.PI * 7 / 8) return new Direction8(7);
            if (rad < -Math.PI * 5 / 8) return new Direction8(8);
            if (rad < -Math.PI * 3 / 8) return new Direction8(9);
            if (rad < Math.PI / 8) return new Direction8(6);
            if (rad < Math.PI * 3 / 8) return new Direction8(3);
            if (rad < Math.PI * 5 / 8) return new Direction8(2);
            if (rad < Math.PI * 7 / 8) return new Direction8(1);
            if (rad < Math.PI * 9 / 8) return new Direction8(4);
            return new Direction8(7);
        }

        public Direction8 TurnLeft()
        {
            int index = Array.IndexOf(Directions, _value) + 1;
            if (index >= Directions.Length)
                index -= Directions.Length;
            return new Direction8(Directions[index]);
        }

        public Direction8 TurnRight()
        {
            int index = Array.IndexOf(Directions, _value) - 1;
            if (index < 0)
                index += Directions.Length;
            return new Direction8(Directions[index]);
        }

        public int GetEarliest(Direction8 target)
        {
            int ti = Array.IndexOf(Directions, target._value);
            int ci = Array.IndexOf(Directions, _value);
            int i = ti - ci;
            if (i < 0)
                i += Directions.Length;
            else if (i >= Directions.Length)
                i -= Directions.Length;
            if (i < 4)
                return 1;
            else if (i != 4)
                return -1;
            return 0;
        }
    }
}
