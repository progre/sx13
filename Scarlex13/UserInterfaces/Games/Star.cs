using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;
using Progressive.Scarlex13.UserInterfaces.Commons.ValueObjects;

namespace Progressive.Scarlex13.UserInterfaces.Games
{
    internal class Star
    {
        private readonly Color _color;
        private int _length = 1;
        private Point _point;
        private double _speed;

        public Star(Point point, int speed, Color color)
        {
            _point = point;
            _speed = speed;
            _color = color;
        }

        public Point Point
        {
            get { return _point; }
        }

        public void Update()
        {
            _point.Y += (short)_speed;
            if (_point.Y >= Point.Height)
                _point.Y -= Point.Height;
        }

        public void Extend()
        {
            if (_length < 8)
                _length += 2;
            _speed = 1 + _speed * 1.15;
        }

        public void Warp()
        {
            _length *= 10;
            _speed *= 4;
        }

        public void Render(Renderer renderer)
        {
            if (_length == 1)
            {
                renderer.DrawPixel(_point, _color);
                return;
            }
            renderer.DrawLine(_point, _point.Shift(0, _length), _color);
        }
    }
}