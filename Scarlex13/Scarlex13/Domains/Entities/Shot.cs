using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class Shot
    {
        private readonly byte _direction = 4;
        private Point _point;

        public Shot(byte direction, Point point)
        {
            _direction = direction;
            _point = point;
        }

        public Point Point
        {
            get { return _point; }
        }

        public void Update()
        {
            const int speed = 5;
            switch (_direction)
            {
                case 8:
                    _point.Y -= speed;
                    break;
                case 9:
                    _point.X += speed;
                    _point.Y -= speed;
                    break;
                case 6:
                    _point.X += speed;
                    break;
                case 3:
                    _point.X += speed;
                    _point.Y += speed;
                    break;
                case 2:
                    _point.Y += speed;
                    break;
                case 1:
                    _point.X -= speed;
                    _point.Y += speed;
                    break;
                case 4:
                    _point.X -= speed;
                    break;
                case 7:
                    _point.X -= speed;
                    _point.Y -= speed;
                    break;
            }
        }
    }
}