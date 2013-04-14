using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class ShootingGame
    {
        private const int ClearedTime = 50;
        private const int FailedTime = 50;

        private readonly StageFactory _stageFactory
            = StageFactory.FromData(new File().GetStages());

        private int _intervalFrame = -1;
        private bool _newWorld = true;
        private int _secondTimeKeeper;
        private int _allHitCount = 0;
        private int _shotCountHistory = 0;

        public ShootingGame()
        {
            Time = new TimeSpan(0, 3, 0);
        }

        public bool Cleared { get; private set; }
        public bool Failed { get; private set; }
        public ShootingWorld World { get; private set; }
        public SoundManager SoundManager { private get; set; }
        public TimeSpan Time { get; private set; }
        public int Score { get; private set; }
        public int StageNo { get; private set; }
        public int MissCount { get; private set; }

        public int TotalHitRatioPercent
        {
            get
            {
                return _shotCountHistory == 0 ? 0 : _allHitCount * 100 / _shotCountHistory;
            }
        }

        public int HitRatioPercent
        {
            get
            {
                return World.Player.ShotCount == 0
                    ? 0
                    : World.AllHitCount * 100 / World.Player.ShotCount;
            }
        }

        public int BonusTime
        {
            get
            {
                int hitRatio = HitRatioPercent;
                if (hitRatio >= 100)
                    return 15;
                if (hitRatio >= 90)
                    return 10;
                if (hitRatio >= 80)
                    return 7;
                if (hitRatio >= 70)
                    return 5;
                return 0;
            }
        }

        public void Update(Input input)
        {
            if (Time <= TimeSpan.FromTicks(0))
            {
                Time = TimeSpan.FromTicks(0);
                return;
            }
            if (_newWorld)
            {
                CreateWorld();
                _newWorld = false;
            }

            if ( /* ポーズ状態 */ false)
            {
                return;
            }

            if (!Cleared)
            {
                // １フレームの時間は17ms, 17ms, 16ms
                if (_secondTimeKeeper < 2)
                {
                    Time -= TimeSpan.FromMilliseconds(17);
                }
                else
                {
                    Time -= TimeSpan.FromMilliseconds(16);
                    _secondTimeKeeper = 0;
                }

                World.Update(input);
                if (!Failed)
                    return;
            }
            _intervalFrame++;
            if (Cleared && _intervalFrame > ClearedTime)
            {
                if (StageNo < _stageFactory.LastStage)
                    StageNo++;
                _newWorld = true;
                return;
            }
            if (Failed && _intervalFrame > FailedTime)
            {
                MissCount++;
                Time -= TimeSpan.FromSeconds(10);
                _newWorld = true;
                return;
            }
        }

        private void CreateWorld()
        {
            if (Cleared && World != null)
            {
                _allHitCount += World.AllHitCount;
                _shotCountHistory += World.Player.ShotCount;
            }
            Cleared = false;
            Failed = false;
            _intervalFrame = -1;
            World = new ShootingWorld(_stageFactory.GetEnemies(StageNo));
            World.Cleared += (sender, args) =>
            {
                Cleared = true;
                Time += TimeSpan.FromSeconds(BonusTime);
            };
            World.Failed += (sender, args) => Failed = true;
            foreach (var enemy in World.Enemies)
            {
                enemy.Damaged += (sender, args) =>
                {
                    switch (((Enemy)sender).Type)
                    {
                        case EnemyType.Silver:
                            Score += 50;
                            break;
                        case EnemyType.Gold:
                            Score += 100;
                            break;
                    }
                };
                enemy.Died += (sender, args) =>
                {
                    var e = (Enemy)sender;
                    int score = 0;
                    switch (e.Type)
                    {
                        case EnemyType.Green:
                            score = 100;
                            break;
                        case EnemyType.Blue:
                            score = 200;
                            break;
                        case EnemyType.Red:
                            score = 300;
                            break;
                        case EnemyType.Silver:
                            score = 400;
                            break;
                        case EnemyType.Gold:
                            score = 500;
                            break;
                    }
                    if (e.State == Enemy.MovingState.SpinAttack)
                        score *= 4;
                    else if (e.State != Enemy.MovingState.Group)
                        score *= 2;
                    Score += score;
                };
            }
        }
    }
}