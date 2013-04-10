using System;
using Progressive.Scarlex13.Domains.ValueObjects;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class Enemy : Character
    {
        private const int TurnTime = 12;
        private const int Speed = 2;
        private const int SkewSpeed = 1; // 1.4;

        public readonly EnemyType Type;
        private readonly Random _random;
        private int _frame = -1;
        private MovingState _movingState;
        private double _vectorX;

        public Enemy(EnemyType type, Point point, Random random)
        {
            Type = type;
            _point = point;
            _random = random;
        }

        public event EventHandler Shot;

        public void Update(Point playerPoint)
        {
            base.Update();
            if (Life <= 0)
                return;
            _frame++;
            switch (_movingState)
            {
                case MovingState.Group:
                    if (_random.Next(970) == 0)
                    {
                        _movingState = _random.Next(2) == 0
                            ? MovingState.TurnLeft
                            : MovingState.TurnRight;
                        _frame = 0;
                    }
                    break;

                case MovingState.TurnLeft:
                case MovingState.TurnRight:
                    if (_frame >= TurnTime * 4)
                    {
                        _movingState = MovingState.Attack;
                        Direction = new Direction8(2);
                        _frame = 0;
                    }
                    break;
                case MovingState.Attack:
                    if (Type == EnemyType.Gold && _random.Next(85) == 0)
                    {
                        _vectorX = 0;
                        _movingState = MovingState.SpinAttack;
                        _frame = 0;
                    }
                    break;
                case MovingState.SpinAttack:
                    if (Direction.Value == 2 && _random.Next(10) == 0)
                    {
                        _movingState = MovingState.Attack;
                    }
                    break;
            }
            switch (_movingState)
            {
                case MovingState.Group:
                    int myFrame = _frame % 140;
                    if (myFrame < 35)
                    {
                        _point.X += Speed;
                    }
                    else if (myFrame < 35 + 70)
                    {
                        _point.X -= Speed;
                    }
                    else
                    {
                        _point.X += Speed;
                    }
                    break;

                case MovingState.TurnLeft:
                    Direction = new Direction8((byte)(
                        _frame < TurnTime ? 8
                            : _frame < TurnTime * 2 ? 7
                                : _frame < TurnTime * 3 ? 4
                                    : 1));
                    TurnMove();
                    break;
                case MovingState.TurnRight:
                    Direction = new Direction8((byte)(
                        _frame < TurnTime ? 8
                            : _frame < TurnTime * 2 ? 9
                                : _frame < TurnTime * 3 ? 6
                                    : 3));
                    TurnMove();
                    break;
                case MovingState.Attack:
                    // 移動
                    if (_point.X < playerPoint.X)
                        _vectorX += 0.1;
                    else if (_point.X > playerPoint.X)
                        _vectorX -= 0.1;
                    _point.Y += Speed;
                    _point.X += (short)_vectorX;

                    // 旋回
                    if (Type == EnemyType.Blue
                        || Type == EnemyType.Red
                        || Type == EnemyType.Silver)
                    {
                        double radian = GetRadian(Point, playerPoint);
                        Direction = Direction8.FromRadian(radian);
                        if (Type == EnemyType.Blue
                            || Type == EnemyType.Silver)
                        {
                            switch (Direction.Value)
                            {
                                case 8:
                                    Direction = new Direction8(2);
                                    break;
                                case 7:
                                case 4:
                                    Direction = new Direction8(1);
                                    break;
                                case 9:
                                case 6:
                                    Direction = new Direction8(3);
                                    break;
                            }
                        }
                    }

                    // 攻撃
                    if (_random.Next(100) == 0)
                        Shot(this, EventArgs.Empty);
                    if (_frame < TurnTime)
                        break;
                    Turn();
                    break;

                case MovingState.SpinAttack:
                    if (_frame == 0)
                        Shot(this, EventArgs.Empty);
                    if (_frame < 3)
                        break;
                    Direction = Direction.TurnRight();
                    _frame = -1;
                    break;
            }

            // 行動範囲制限
            if (_point.Y >= Point.Height)
                _point.Y -= Point.Height;
            if (_point.X >= Point.Width - 13)
                _point.X = Point.Width - 13 - 1;
            if (_point.X < 13)
                _point.X = 13;
        }

        private void Turn()
        {
        }

        private void TurnMove()
        {
            switch (Direction.Value)
            {
                case 8:
                    _point.Y -= Speed;
                    break;
                case 7:
                    _point.X -= SkewSpeed;
                    _point.Y -= SkewSpeed;
                    break;
                case 9:
                    _point.X += SkewSpeed;
                    _point.Y -= SkewSpeed;
                    break;
                case 4:
                    _point.X -= Speed;
                    break;
                case 6:
                    _point.X += Speed;
                    break;
                case 1:
                    _point.X -= SkewSpeed;
                    _point.Y += SkewSpeed;
                    break;
                case 3:
                    _point.X += SkewSpeed;
                    _point.Y += SkewSpeed;
                    break;
            }
        }

        private static double GetRadian(Point source, Point target)
        {
            Point relative = target.Shift(-source.X, -source.Y);
            return Math.Atan2(relative.Y, relative.X);
        }

        private enum MovingState
        {
            Group,
            TurnLeft,
            TurnRight,
            Attack,
            SpinAttack
        }
    }

    public enum EnemyType
    {
        Green,
        Blue,
        Red,
        Silver,
        Gold
    }
}