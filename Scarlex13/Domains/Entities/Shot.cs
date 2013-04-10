using Progressive.Scarlex13.Domains.ValueObjects;
using System;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class Shot
    {
        public readonly Direction8 Direction;
        private Point _point;

        public Shot(Direction8 direction, Point point)
        {
            Direction = direction;
            _point = point;
        }

        public Point Point
        {
            get { return _point; }
        }

        public void Update()
        {
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