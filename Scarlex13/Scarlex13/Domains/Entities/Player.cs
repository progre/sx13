using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class Player
    {
        private Point _point = new Point(250, 400);
        private byte _direction = 5;

        public Point Point
        {
            get { return _point; }
        }

        public void Update(Input input)
        {
            if (input.Direction != 0)
                _direction = input.Direction;

            switch (_direction)
            {
                case 7:
                case 4:
                case 1:
                    _point.X -= 2;
                    break;
                case 9:
                case 6:
                case 3:
                    _point.X += 2;
                    break;
            }
            switch (_direction)
            {
                case 7:
                case 8:
                case 9:
                    _point.Y -= 2;
                    break;
                case 1:
                case 2:
                case 3:
                    _point.Y += 2;
                    break;
            }
        }

        public bool HitTest(Point point)
        {
            const int halfSize = 17;
            return _point.X - halfSize < point.X && point.X < _point.X + halfSize
                && _point.Y - halfSize < point.Y && point.Y < _point.Y + halfSize;
        }
    }
}