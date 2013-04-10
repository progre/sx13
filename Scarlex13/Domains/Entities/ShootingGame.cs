using System;
using System.Collections.Generic;
using Progressive.Scarlex13.Domains.ValueObjects;
using Progressive.Scarlex13.Infrastructures;

namespace Progressive.Scarlex13.Domains.Entities
{
    internal class ShootingGame
    {
        private const int ReadyTime = 10;
        private const int ClearedTime = 50;
        private const int FailedTime = 50;

        private readonly StageFactory _stageFactory
            = StageFactory.FromData(new File().GetStages());

        private int _intervalFrame = -1;
        private bool _newWorld = true;
        private int _secondTimeKeeper;

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

        public int HitRatioPercent
        {
            get
            {
                return World.Player.ShotCount == 0 ? 0 : World.AllHitCount * 100 / World.Player.ShotCount;
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
                    Time = Time.Add(TimeSpan.FromMilliseconds(-17));
                }
                else
                {
                    Time = Time.Add(TimeSpan.FromMilliseconds(-16));
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
                _newWorld = true;
                return;
            }
        }

        private void CreateWorld()
        {
            Cleared = false;
            Failed = false;
            _intervalFrame = -1;
            World = new ShootingWorld(_stageFactory.GetEnemies(StageNo));
            World.Cleared += (sender, args) =>
            {
                Cleared = true;
                Time = Time.Add(TimeSpan.FromSeconds(BonusTime));
            };
            World.Failed += (sender, args) => Failed = true;
        }
    }
}