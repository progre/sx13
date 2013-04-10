using System;
using System.Collections.Generic;
using System.Linq;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class ShootingWorld
    {
        private readonly IReadOnlyList<Enemy> _enemies;
        private readonly Player _player = new Player();
        private readonly List<Shot> _playerShots = new List<Shot>(7);
        private readonly List<Shot> _shots = new List<Shot>();

        public event EventHandler Cleared;
        public event EventHandler Failed;

        public ShootingWorld(IReadOnlyList<Enemy> enemies)
        {
            _enemies = enemies;
            _player.Shot += (sender, args) =>
                _playerShots.Add(new Shot(
                    new Direction8(8), _player.Point.Shift(0, -17), false));
            _player.Died += (sender, args) => Failed(this, EventArgs.Empty);
            foreach (Enemy enemy in _enemies)
            {
                enemy.Shot += (sender, args) =>
                    _shots.Add(new Shot(((Enemy)sender).Direction,
                        ((Enemy)sender).Point, enemy.Type == EnemyType.Silver));
                enemy.Determined += (sender, args) =>
                {
                    if (_enemies.All(x => x.Life == -1))
                    {
                        Cleared(this, EventArgs.Empty);
                    }
                };
            }
        }

        public Player Player
        {
            get { return _player; }
        }

        public IReadOnlyList<Enemy> Enemies
        {
            get { return _enemies; }
        }

        public IEnumerable<Shot> Shots
        {
            get { return _shots.Concat(_playerShots); }
        }

        public int AllHitCount
        {
            get 
            {
                return _enemies.Sum(
                    x => x.Type == EnemyType.Silver
                    || x.Type == EnemyType.Gold
                    ? 2 : 1);
            }
        }

        public void Update(Input input)
        {
            Player.Update(input, _playerShots.Count < 7);
            foreach (Enemy enemy in _enemies)
            {
                enemy.Update(_player.Point);
            }
            foreach (Shot shot in _playerShots.ToArray())
            {
                shot.Update(_player.Point);
                if (IsOutOfStage(shot))
                {
                    _playerShots.Remove(shot);
                    continue;
                }
                Enemy hitEnemy = _enemies.FirstOrDefault(x => x.HitTest(shot.Point));
                if (hitEnemy == null)
                    continue;
                hitEnemy.Damage();
                _playerShots.Remove(shot);
            }
            foreach (Shot shot in _shots.ToArray())
            {
                shot.Update(_player.Point);
                if (IsOutOfStage(shot))
                {
                    _shots.Remove(shot);
                    continue;
                }
                if (!_player.HitTest(shot.Point))
                    continue;
                _player.Damage();
                _shots.Remove(shot);
            }
            if (_enemies.Any(_player.HitTest))
            {
                _player.Damage();
            }
        }

        private static bool IsOutOfStage(Shot shot)
        {
            Point point = shot.Point;
            return point.X < 0 || Point.Width - 1 < point.X
                || point.Y < 0 || Point.Height - 1 < point.Y;
        }
    }
}