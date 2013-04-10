using Progressive.Scarlex13.Domains.ValueObjects;
using System;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class Player : Character
    {
        private const int EnemyArea = 180;
        private const short Speed = 3;
        private bool _firstTime = true;
        private int _reloadTime;
        public event EventHandler Shot;

        public int ShotCount { get; private set; }

        public void Update(Input input, bool canShot)
        {
            base.Update();
            if (Life <= 0)
                return;
            byte direction;
            bool shot;
            if (_firstTime)
            {
                direction = input.Direction;
                shot = input.Shot;
                _firstTime = false;
            }
            else
            {
                direction = input.DirectionToggled ? input.Direction : (byte)0;
                shot = input.ShotToggled && input.Shot;
            }

            if (direction > 0)
                Direction = direction;
            if (_reloadTime == 0)
            {
                if (canShot && shot)
                {
                    Shot(this, EventArgs.Empty);
                    ShotCount++;
                    _reloadTime = 8;
                }
            }
            else
            {
                _reloadTime--;
            }

            switch (Direction)
            {
                case 7:
                case 4:
                case 1:
                    _point.X -= Speed;
                    break;
                case 9:
                case 6:
                case 3:
                    _point.X += Speed;
                    break;
            }
            switch (Direction)
            {
                case 7:
                case 8:
                case 9:
                    _point.Y -= Speed;
                    break;
                case 1:
                case 2:
                case 3:
                    _point.Y += Speed;
                    break;
            }
            if (_point.Y < EnemyArea)
                _point.Y = EnemyArea;
            if (_point.Y >= Point.Height - SafeArea)
                _point.Y = Point.Height - SafeArea - 1;
            if (_point.X >= Point.Width - SafeArea)
                _point.X = Point.Width - SafeArea - 1;
            if (_point.X < SafeArea)
                _point.X = SafeArea;
        }
    }
}