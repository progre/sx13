using Progressive.Scarlex13.Domains.ValueObjects;
using System;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class Shot
    {
        public Direction8 Direction;
        private Point _point;
        private readonly bool _homing;
        private int _homingState;
        private int _frame;

        public Shot(Direction8 direction, Point point, bool homing)
        {
            Direction = direction;
            _point = point;
            _homing = homing;
        }

        public bool Homing
        {
            get { return _homing; }
        }

        public Point Point
        {
            get { return _point; }
        }

        public void Update(Point playerPoint)
        {
            if (_homing)
            {
                switch (_homingState)
                {
                    case 0:
                        if (_point.Y >= playerPoint.Y)
                        {
                            Direction = new Direction8(5);
                            _homingState = 1;
                        }
                        break;
                    case 1:
                        _frame++;
                        if (_frame <= 30)
                        {
                            return;
                        }
                        Direction = new Direction8(
                            (byte)(Point.X < playerPoint.X ? 6 : 4));
                        _homingState = 2;
                        break;
                }
            }

            const int speed = 5;
            const int skewSpeed = (short)(speed / 1.41421356);
            switch (Direction.Value)
            {
                case 8:
                    _point.Y -= speed;
                    break;
                case 9:
                    _point.X += skewSpeed;
                    _point.Y -= skewSpeed;
                    break;
                case 6:
                    _point.X += speed;
                    break;
                case 3:
                    _point.X += skewSpeed;
                    _point.Y += skewSpeed;
                    break;
                case 2:
                    _point.Y += speed;
                    break;
                case 1:
                    _point.X -= skewSpeed;
                    _point.Y += skewSpeed;
                    break;
                case 4:
                    _point.X -= speed;
                    break;
                case 7:
                    _point.X -= skewSpeed;
                    _point.Y -= skewSpeed;
                    break;
            }
        }
    }
}