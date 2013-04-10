using System;
using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class Character
    {
        private const int DetermineTime = 25;
        protected const short SafeArea = 17;
        private int _deadTime = -1;
        // ReSharper disable InconsistentNaming
        protected Point _point = new Point(250, 400);
        // ReSharper restore InconsistentNaming

        public Character()
        {
            Life = 1;
            Direction = new Direction8(8);
        }

        public Point Point
        {
            get { return _point; }
        }

        public Direction8 Direction { get; protected set; }
        public int Life { get; private set; }
        public event EventHandler Damaged = (sender, args) => { };
        public event EventHandler Died = (sender, args) => { };
        public event EventHandler Determined = (sender, args) => { };

        public virtual void Update()
        {
            if (Life <= -1 || 0 < Life)
                return;

            if (Life == 0 && _deadTime < DetermineTime)
            {
                _deadTime++;
            }
            if (_deadTime != DetermineTime)
                return;
            Life = -1;
            Determined(this, EventArgs.Empty);
        }

        public void Damage()
        {
            Life--;
            if (Life > 0)
                Damaged(this, EventArgs.Empty);
            else
                Died(this, EventArgs.Empty);
        }

        public bool HitTest(Point point)
        {
            if (Life <= 0)
                return false;
            const int halfSize = 17;
            return _point.X - halfSize < point.X && point.X < _point.X + halfSize
                && _point.Y - halfSize < point.Y && point.Y < _point.Y + halfSize;
        }

        public bool HitTest(Enemy enemy)
        {
            if (Life <= 0)
                return false;
            if (enemy.Life <= 0)
                return false;
            Point point = enemy.Point;
            const int halfSize = 17;
            return _point.X - halfSize < point.X && point.X < _point.X + halfSize
                && _point.Y - halfSize < point.Y && point.Y < _point.Y + halfSize;
        }
    }
}